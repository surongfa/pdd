using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatRegster.listenes;

namespace pdd
{
    class Util
    {
        public static bool StrTojson(string str, out JObject jObject)
        {
            jObject = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    Encoding utf8EWithNoByteOrderMark = new UTF8Encoding(false);
                    jObject = JObject.Parse(GetUTF8String(utf8EWithNoByteOrderMark.GetBytes(str.Trim())));
                    // jObject = (JObject)JsonConvert.DeserializeObject(str.Trim());
                    return true;
                }
            }
            catch (Exception e)
            {
                Service.put("json解析异常：" + str, true);
            }
            return false;
        }

        public static string GetUTF8String(byte[] buffer)
        {
            if (buffer == null)
                return null;

            if (buffer.Length <= 3)
            {
                return Encoding.UTF8.GetString(buffer);
            }

            byte[] bomBuffer = new byte[] { 0xef, 0xbb, 0xbf };

            if (buffer[0] == bomBuffer[0]
                && buffer[1] == bomBuffer[1]
                && buffer[2] == bomBuffer[2])
            {
                return new UTF8Encoding(false).GetString(buffer, 3, buffer.Length - 3);
            }

            return Encoding.UTF8.GetString(buffer);
        }

        public static string AesDecrypt(string str, string key = "zhongguogonganju")
        {
            if (string.IsNullOrWhiteSpace(str))
                return "";
            try
            {
                Byte[] toEncryptArray = Convert.FromBase64String(str.Trim());
                using (System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = System.Security.Cryptography.CipherMode.CBC,
                    //Padding = System.Security.Cryptography.PaddingMode.PKCS7,
                    IV = Encoding.UTF8.GetBytes("0392039203920300")

                })
                {
                    System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
                    Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                    return Encoding.UTF8.GetString(resultArray);
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        public static string AesEncrypt(string str, string key = "zhongguogonganju")
        {
            if (string.IsNullOrEmpty(str))
                return "";
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
            try
            {
                using (System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = System.Security.Cryptography.CipherMode.CBC,
                    //Padding = System.Security.Cryptography.PaddingMode.PKCS7,
                    IV = Encoding.UTF8.GetBytes("0392039203920300")
                })
                {

                    System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
                    Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace, true);
            }
            return "";
        }
    }
}
