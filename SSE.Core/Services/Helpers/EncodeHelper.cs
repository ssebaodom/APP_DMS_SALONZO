using System.Text;

namespace SSE.Core.Services.Helpers
{
    public class EncodeHelper
    {
        public static string Encode(string input)
        {
            var buffer = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            int num = (buffer.Length - 1);
            int i = 0;
            while (i <= num)
            {
                builder.Append(buffer[i].ToString("x2"));
                i += 1;
            }
            return builder.ToString();
        }
    }
}