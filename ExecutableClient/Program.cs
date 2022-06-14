using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ExecutableClient
{
    class Program
    {
        private static string apikey = "api key";
        private static string secretkey = "secret key";
        private static string method = "POST";
        private static string baseUrl = "https://openplatform.dg-work.cn";
        private static string uri = "/gettoken.json";

        static void Main(string[] args)
        {
            string timestamp = Utils.Timestamp();
            string nonce = Utils.GetTime13() + Utils.RandomInt4();
            string url = baseUrl + uri;
            var headers = new Dictionary<string, string>();
            headers["X-Hmac-Auth-Timestamp"] = timestamp;
            headers["X-Hmac-Auth-Version"] = "1.0";
            headers["X-Hmac-Auth-Nonce"] = nonce;
            headers["apiKey"] = apikey;
            headers["X-Hmac-Auth-Signature"] = Utils.Signature(secretkey, method, timestamp, nonce, uri);
            headers["X-Hmac-Auth-IP"] = "127.0.0.1";
            headers["X-Hmac-Auth-MAC"] = Utils.GetMacByNetworkInterface();
            NameValueCollection formData = new NameValueCollection();
            string res = Utils.PostData(formData, method, url, headers);
            Console.WriteLine(res);
        }

    }
}
