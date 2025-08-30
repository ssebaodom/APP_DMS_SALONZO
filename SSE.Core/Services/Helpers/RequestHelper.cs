using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SSE.Core.Services.Helpers
{
    public class RequestHelper
    {
        private static HttpWebRequest createRequest(string url, Dictionary<string, string> headers, string contentType, string method)
        {
            WebRequest request = WebRequest.Create(url);
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)request;
            myHttpWebRequest.Method = method;
            myHttpWebRequest.ContentType = contentType;
            myHttpWebRequest.Accept = contentType;
            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    myHttpWebRequest.Headers.Add(header.Key, header.Value);
                }
            }
            return myHttpWebRequest;
        }

        public static T postRequest<T>(string url, Dictionary<string, string> headers, object data, string contentType = "application/json")
        {
            var httpRequest = createRequest(url, headers, contentType, "post");
            // Create POST data and convert it to a byte array.
            string postData = null;
            if (data.GetType() == typeof(string))
            {
                postData = data.ToString();
            }
            else
            {
                postData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentLength property of the WebRequest.
            httpRequest.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = httpRequest.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            WebResponse response = httpRequest.GetResponse();

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            object contentAll = "";
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                contentAll = reader.ReadToEnd();
            }
            // Close the response.
            response.Close();
            return typeof(T) == typeof(string) ? (T)contentAll : Newtonsoft.Json.JsonConvert.DeserializeObject<T>((String)contentAll);
        }

        public static T getRequest<T>(string base_url, string url)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                client.BaseAddress = new Uri(base_url);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                object contentResponse = response.Content.ReadAsStringAsync().Result;

                return typeof(T) == typeof(string) ? (T)contentResponse : Newtonsoft.Json.JsonConvert.DeserializeObject<T>((String)contentResponse);
            }
        }
    }
}