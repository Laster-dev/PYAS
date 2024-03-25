using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MotaAntivirus.Antivirus.ScanFile
{
    public class S360Cloud
    {
        public static string Api360Url;
        private static HttpWebRequest Api360request;

        public static string Api360CloudScan(string md5)
        {
            Console.WriteLine(md5);
            Api360Url = "http://qup.f.360.cn/file_health_info.php";
            Api360request = (HttpWebRequest)WebRequest.Create(Api360Url);
            Api360request.Method = "POST";
            Api360request.ContentType = "multipart/form-data";
            string StrBody = // DON'T USE TAB OR SPACE
      $@"-------------------------------7d83e2d7a141e
Content-Disposition: form-data; name=""md5s""

{md5}
-------------------------------7d83e2d7a141e
Content-Disposition: form-data; name=""format""

XML
-------------------------------7d83e2d7a141e
Content-Disposition: form-data; name=""product""

360zip
-------------------------------7d83e2d7a141e
Content-Disposition: form-data; name=""combo""

360zip_main
-------------------------------7d83e2d7a141e
Content-Disposition: form-data; name=""v""

2
-------------------------------7d83e2d7a141e
Content-Disposition: form-data; name=""osver""

5.1
-------------------------------7d83e2d7a141e
Content-Disposition: form-data; name=""vk""

a03bc211
-------------------------------7d83e2d7a141e
Content-Disposition: form-data; name=""mid""

8a40d9eff408a78fe9ec10a0e7e60f62
-------------------------------7d83e2d7a141e--";

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(StrBody);
                using (Stream stream = Api360request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (WebResponse response = Api360request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string body = reader.ReadToEnd();
                    Console.WriteLine(body);
                    XElement Xmlbody = XElement.Parse(body);
                    string MalwareName = Xmlbody.Descendants("e_level").FirstOrDefault()?.Value;
                    response.Close();
                    stream.Close();
                    reader.Close();

                    if (MalwareName != null)
                    {
                        return MalwareName;
                    }
                    else
                    {
                        return "MalwareName != null";
                    }
                }
            }
            catch
            {
                return "            catch\r\n";
            }
        }
        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                //新建文件流
                using (FileStream file = new FileStream(fileName, System.IO.FileMode.Open))
                {
                    //MD5加密服务提供器
                    MD5 md5 = new MD5CryptoServiceProvider();

                    //对文件进行计算MD5
                    byte[] retVal = md5.ComputeHash(file);

                    //保存输出结果
                    StringBuilder sb = new StringBuilder();
                    //转为2进制
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public static void Main(string[] args) {
            var folderPath = Console.ReadLine();
            // 遍历文件夹下的所有文件  
            foreach (string filePath in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".exe") || s.EndsWith(".dll")))
            {
                var str = Api360CloudScan(GetMD5HashFromFile(filePath));
                Console.WriteLine(str);
            }
           

            Console.ReadKey();
        }
    }
}