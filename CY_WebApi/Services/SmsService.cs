namespace CY_WebApi.Services
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class SmsService
    {
        private readonly HttpClient _httpClient;
        //For Chipyab:
        private const string ApiKey = "LXpKliZ6SF8nHs8W5FeN7ahY1woX_Ktf3I4KGfbVGN4=";
        //For Sane:
        //private readonly string ApiKey = "_Hu-7SMK8dSfiebKf8EVC06cLQNxf7tys7T6Kj0NFOM=";
        public SmsService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("apikey", $"{ApiKey}");
        }

        public async Task<string> SendRegisterSmsAsync(string recipient, int codeValue)
         {
            var requestBody = new
            {
                //For Chipyab
                code = "nkrpdz1aq77o9wg",
                //For Sane:
                //code = "fe8tc662vxt1u95",
                sender = "+983000505",
                recipient = recipient,
                variable = new
                {
                    code = codeValue
                }
            };

            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api2.ippanel.com/api/v1/sms/pattern/normal/send", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task<string> SendSmsAsync(string recipient, string formatString, string nameValue, int codeValue)
        {
            var requestBody = new
            {
                code = formatString,
                sender = "+983000505",
                recipient = recipient,
                variable = new
                {
                    code = codeValue,
                    name = nameValue
                }
            };

            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api2.ippanel.com/api/v1/sms/pattern/normal/send", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }

        public string? ConvertPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.StartsWith("+98") && phoneNumber.Length >= 13)
            {
                // Convert +98XXXXXXXXXX to 0XXXXXXXXXX
                return "0" + phoneNumber.Substring(3);
            }
            else if (phoneNumber.StartsWith("0") && phoneNumber.Length >= 11)
            {
                // Convert 0XXXXXXXXXX to +98XXXXXXXXXX
                return "+98" + phoneNumber.Substring(1);
            }
            else
            {
                // If the phone number doesn't start with +98 or 0, it is not valid
                return null;
            }
        }
    }

}
