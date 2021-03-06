using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace ExecutableClient
{
    class Utils
    {
        public static string Timestamp()
        {
            return DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffffff+08:00");
        }

        public static string RandomInt4()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        public static string Signature(string secretkey, string method, string timestamp, string nonce, string uri)
        {
            string sign = string.Format("{0}\n{1}\n{2}\n{3}", method, timestamp, nonce, uri);
            return HmacSha256(secretkey, sign);
        }

        public static string PostData(NameValueCollection postData, string method, string url, Dictionary<string, string> headers)
        {
            WebClient wc = new WebClient();
            //wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            foreach (var item in headers)
            {
                wc.Headers.Add(item.Key, item.Value);
            }
            byte[] responseData = wc.UploadValues(url, method, postData);
            wc.Dispose();
            return Encoding.UTF8.GetString(responseData);
        }

        public static string HmacSha256(string secretAccessKey, string data)
        {
            UTF8Encoding enc = new UTF8Encoding();
            byte[] secretKey = enc.GetBytes(secretAccessKey);
            HMACSHA256 hmac = new HMACSHA256(secretKey);
            hmac.Initialize();
            byte[] bytes = enc.GetBytes(data);
            byte[] rawHmac = hmac.ComputeHash(bytes);
            string result = Convert.ToBase64String(rawHmac);
            return result;
        }

        public static string GetMacByNetworkInterface()
        {
            try
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in interfaces)
                {
                    return BitConverter.ToString(ni.GetPhysicalAddress().GetAddressBytes());
                }
            }
            catch (Exception)
            {
            }
            return "00-00-00-00-00-00";
        }

        public static long GetTime13()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            long time = (long)ts.TotalMilliseconds;
            return time;
        }
    }
}

