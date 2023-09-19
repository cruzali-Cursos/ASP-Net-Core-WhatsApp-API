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
                string accessToken = "EAACAkOtc2IQBOZBXgliJL7hkHfGlSsahSzl5aocHp5vD1k7otSdo7wLbwIk1uorlGeJqZBakywRFhErMgbZBQuGpz28ZCEl8iVKyEPeI9T42ToyB657KNjbTBKHPFQOoNpVZBDVIPmlU8WnVVgYQfnIoBcfpqTVVuc1XdsSoCmBzkZAAYFKEFY3WjpieJBoHUH4YTmvC7yOgY8ajZB8gmUZD";
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
