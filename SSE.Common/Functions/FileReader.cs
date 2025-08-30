using Newtonsoft.Json;
using System;
using System.IO;

namespace SSE.Common.Functions
{
    public class FileReader
    {
        public static T LoadFileJson<T>(string path, string file)
        {
#if DEBUG || TEST
            string mainpath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\");
#else
            string mainpath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "");
#endif
            using (StreamReader r = new StreamReader(mainpath + "/" + path + '/' + file))
            {
                string json = r.ReadToEnd();
                T obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
        }
    }
}