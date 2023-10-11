using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text;

namespace ASP.NetCore_WhatsApp_1.Services.WhatsappCloud.SendMessage
{
    public class WhatsappCloudSendMessage : IWhatsappCloudSendMessage
    {
        public async Task<bool> Execute(object model)
        {
            var client = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));

            using (var content = new ByteArrayContent(byteData))
            {
                string endpoint = "https://graph.facebook.com";
                string phoneNumberId = "124978000694990";
                string accessToken = "EAACAkOtc2IQBO0CGYTFv1DZB6XQwRdMietkmucpYeuUumEr4JlkQsv0eopI9zDPZCiPP61PiChZAv4zAnWaRjfrap0k1mZCly9qil0B3MM9Its4qMZCGHjv6rRlrWqezshM76ZAXZAJJo7ZALg3dTeuf1POFFdIBXHB7ZAoDYp3iM9fVzOAQkf4sS6WGVXCMKuUrp3668TMuvVnPrMc9p59MZD";
                string uri = $"{endpoint}/v17.0/{phoneNumberId}/messages?";

                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;                
            }


        }
        
    }
}
