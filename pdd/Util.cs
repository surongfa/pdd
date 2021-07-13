using CrOcr;
using Newtonsoft.Json.Linq;
using SufeiUtil;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using WechatRegster.listenes;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

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

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="version">版本 1 ~ 40</param>
        /// <param name="pixel">像素点大小</param>
        /// <param name="icon_path">图标路径</param>
        /// <param name="icon_size">图标尺寸</param>
        /// <param name="icon_border">图标边框厚度</param>
        /// <param name="white_edge">二维码白边</param>
        /// <returns>位图</returns>
        public static Bitmap qrcode(string msg, string icon_path, int version=1, int pixel = 0, int icon_size=0, int icon_border=0, bool white_edge=true)
        {

            QRCoder.QRCodeGenerator code_generator = new QRCoder.QRCodeGenerator();

            QRCoder.QRCodeData code_data = code_generator.CreateQrCode(msg, QRCoder.QRCodeGenerator.ECCLevel.M/* 这里设置容错率的一个级别 */, true, true, QRCoder.QRCodeGenerator.EciMode.Utf8, version);

            QRCoder.QRCode code = new QRCoder.QRCode(code_data);

            Bitmap icon = new Bitmap(icon_path);

            Bitmap bmp = code.GetGraphic(pixel, Color.Black, Color.White, icon);

            return bmp;

        }
        public static Bitmap CreateQRCode(string asset, int width = 160, int height = 160)
        {
            EncodingOptions options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = width,
                Height = height
            };
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            return writer.Write(asset);
        }

       
        //base64编码的文本 转为    图片
        public static Bitmap Base64StringToImage(string inputStr)
        {
            try
            {
                /*FileStream ifs = new FileStream(txtFileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(ifs);

                String inputStr = sr.ReadToEnd();*/

                byte[] arr = Convert.FromBase64String(inputStr);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return bmp;
                //sr.Close();
                //ifs.Close();
                /*this.pictureBox2.Image = bmp;
                if (File.Exists(txtFileName))
                {
                    File.Delete(txtFileName);
                }*/
                //MessageBox.Show("转换成功！");
            }
            catch (Exception ex)
            {
                Form2.put(ex.Message);
            }
            return null;
        }
        //
        public static string reporterror(string captchaId)
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://v2-api.jsdama.com/report-error",
                Method = "post",
                Postdata = "{\"softwareId\":26062,\"softwareSecret\":\"LBVSGvxjAmeoNzvhVKocShxsps8FjKSc46UPbsAZ \",\"username\":\"a2252647\",\"password\":\"zNQji0aOkzZJrzOn\",\"captchaId\":\""+ captchaId + "\"}",
                ContentType = "application/json, text/javascript, */*; q=0.01",
                Host = "v2-api.jsdama.com",
                Accept = "*",
                KeepAlive = true,
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        //{"message":"","code":0,"data":{"captchaId":"20210712:000000000054246384239","recognition":"MEU5"}}
        public static string verCode(string base64 = "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")
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://v2-api.jsdama.com/upload",
                Method = "post",
                Postdata = "{\"softwareId\":26062,\"softwareSecret\":\"LBVSGvxjAmeoNzvhVKocShxsps8FjKSc46UPbsAZ\",\"username\":\"a2252647\",\"password\":\"zNQji0aOkzZJrzOn\",\"captchaData\":\""+base64+"\",\"captchaType\":1,\"captchaMinLength\":0,\"captchaMaxLength\":0}",
                ContentType = "application/json, text/javascript, */*; q=0.01",
                Host= "v2-api.jsdama.com",
                Accept = "*",
                KeepAlive = true,
            };
            return new HttpHelper().GetHtml(item).Html;
        }

        public static string getanicontent()
        {
            while(!Form1.isClose){
                string ani = executescript(Service.anicontentcode, "getEncode()");
                if (!string.IsNullOrEmpty(ani))
                    return ani;
                else
                {
                    Form2.put("获取不到anicontent");
                    Thread.Sleep(1000);
                }
            }
            return null;
        }

        public static string executescript(string code, string executecode)
        {
            MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControl();
            scriptControl.UseSafeSubset = true;
            scriptControl.Language = "JScript";
            scriptControl.AddCode(code);
            try
            {
                return scriptControl.Eval(executecode).ToString(); //string.Format(@"sayHello('{0}','{1}','{2}')", 23,34,"+");
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return null;
        }

        public static string verCodesuper(Bitmap bmp, out string id)
        {
            id = "";
            MemoryStream ms = new MemoryStream();
            //Bitmap bmp = new Bitmap(image);
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] photo_byte = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(photo_byte, 0, Convert.ToInt32(ms.Length));
            bmp.Dispose();
            string returnMess = Dc.RecByte_A(photo_byte, photo_byte.Length, "a2252647", "2252647", "0");//第5个参数为软件id,缺省为0
            if (returnMess.Equals("Error:No Money!"))
            {
                return "点数不足";
            }
            else if (returnMess.Equals("Error:No Reg!"))
            {
                return "账户密码错误";
            }
            else if (returnMess.Equals("Error:Put Fail!"))
            {
                return "上传失败，图片过大或图片不完整";
            }
            else if (returnMess.Equals("Error:TimeOut!"))
            {
                return "识别超时";
            }
            else if (returnMess.Equals("Error:empty picture!"))
            {
                return "上传无效图片";
            }
            else if (returnMess.Equals("Error:Account or Software Bind!"))
            {
                return "账户或IP被冻结";
            }
            else if (returnMess.Equals("Error:Software Frozen!"))
            {
                return "软件被冻结";
            }
            else if (returnMess.Equals("Error:Account or IP Frozen!"))
            {
                return "当前IP不在绑定IP列表中或IP被冻结";
            }
            else if (returnMess.Equals("Error:Parameter!"))
            {
                return"参数错误,请检查各参数";
            }
            else if (returnMess.IndexOf("|") > -1)
            {
                id = returnMess.Split('|')[2];
                return returnMess.Split('|')[0];
            }
            else
            {
                return "识别错误";
            }
        }
        public static void ReportError(string id)
        {
            Dc.ReportError("a2252647", id);
        }
        
    }
}
