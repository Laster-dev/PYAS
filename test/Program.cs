using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class FileScanner
{
    private const string Boundary = "---------------------------7d83e2d7a141e";
    private const string Url = "http://qup.f.360.cn/file_health_info.php";

    public async Task<string> HashScanAsync(string filePath)
    {
        try
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    var text = BitConverter.ToString(hash).Replace("-", "").ToLower();

                    var content = new MultipartFormDataContent(Boundary)
                    {
                        { new StringContent(text), "md5s" },
                        { new StringContent("XML"), "format" },
                        { new StringContent("360zip"), "product" },
                        { new StringContent("360zip_main"), "combo" },
                        { new StringContent("2"), "v" },
                        { new StringContent("5.1"), "osver" },
                        { new StringContent("a03bc211"), "vk" },
                        { new StringContent("8a40d9eff408a78fe9ec10a0e7e60f62"), "mid" }
                    };

                    using (var client = new HttpClient())
                    {
                        var response = await client.PostAsync(Url, content);
                        if (response.IsSuccessStatusCode)
                        {
                            var xml = await response.Content.ReadAsStringAsync();
                            var doc = XDocument.Parse(xml);
                            var level = doc.Root?.Element(".//e_level")?.Value;
                            if (level != null && float.TryParse(level, out var levelValue))
                            {
                                if (levelValue > 50)
                                    return "Virus";
                                else if (levelValue <= 10)
                                    return "Safe";
                                return "Unknown";
                            }
                        }
                        return $"Error: {response.StatusCode}";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        var scanner = new FileScanner();
        var filePath = "C:\\Users\\laster\\Desktop\\ISOPacker.exe"; // 替换为实际文件路径  
        var result = await scanner.HashScanAsync(filePath);
        Console.WriteLine(result);
        Console.ReadLine();
    }
}