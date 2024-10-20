using System.Text;

namespace BumsBot
{
    public class Tools
    {
        public static string getUserAgents(int Index, bool Plat = false)
        {
            string[] userAgents = ["Mozilla/5.0 (Linux; Android 7.0; SM-G925F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.45 Mobile Safari/537.36", "Mozilla/5.0 (iPhone; CPU iPhone OS 17_5_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3096.19 Safari/537.36", "Mozilla/5.0 (Linux; Android 11; SAMSUNG SM-A202F) AppleWebKit/537.36 (KHTML, like Gecko) SamsungBrowser/14.2 Chrome/87.0.4280.141 Mobile Safari/537.36", "Mozilla/5.0 (Linux; Android 9) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/87.0.4280.86 Mobile DuckDuckGo/5 Safari/537.36", "Mozilla/5.0 (Linux; Android 11; GM1901) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Mobile Safari/537.36", "Mozilla/5.0 (Linux; Android 10; SOV40) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.127 Mobile Safari/537.36", "Mozilla/5.0 (Linux; Android 8.0.0; SM-A520F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.46 Mobile Safari/537.36", "Dalvik/2.1.0 (Linux; U; Android 10; Redmi 7 Build/QQ3A.200605.002.A1)", "Mozilla/5.0 (Linux; Android 10; SM-J610G) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.85 Mobile Safari/537.36", "MMozilla/5.0 (Linux; Android 8.1.0; B450) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.136 Mobile Safari/537.36", "Opera/9.80 (Android; Opera Mini/10.0.1884/191.227; U; en) Presto/2.12.423 Version/12.16", "Mozilla/5.0 (Linux; arm_64; Android 8.1.0; DUA-L22) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 YaBrowser/20.4.4.76.00 SA/1 Mobile Safari/537.36", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36 OPR/79.0.4143.66", "Mozilla/5.0 (Linux; Android 7.1.2; GT-P7300 Build/N2G48C; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/61.0.3163.81 Safari/537.36", "Mozilla/5.0 (iPad; CPU OS 14_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 EdgiOS/45.11.1 Mobile/15E148 Safari/605.1.15", "Mozilla/5.0 (iPhone; CPU OS 14_4_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) FxiOS/32.0 Mobile/15E148 Safari/605.1.15"];
            string[] platform = ["Android", "iPhone", "Windows", "Android", "Android", "Android", "Android", "Android", "Android", "Android", "Android", "Android", "Android", "Windows", "Android", "iPad", "iPhone"];
            return (Plat ? platform[Index] : userAgents[Index]);
        }

        public static string getMD5Hash(string strToHash)
        {
            using (var md5Obj = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] bytesToHash = Encoding.ASCII.GetBytes(strToHash);
                bytesToHash = md5Obj.ComputeHash(bytesToHash);
                string strResult = "";
                foreach (var b in bytesToHash)
                    strResult += b.ToString("x2");
                return strResult;
            }
        }
    }
}