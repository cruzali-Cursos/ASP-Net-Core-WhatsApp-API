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
                string accessToken = "EAACAkOtc2IQBO3ZBzHHS8rZB8eS7rPaxQmkIUkDU3uHdvMaKAW232EKFmzCRIzn1T88U4vIQOuXOoQtfvpyDb1GnKTxtkB46lzc5Uxfp999qYzxx9kHnnZChadtyRsyBPZBgieYTaelpWR162o4O7jYhQ7cue9EYy1UN1KyZA2LELq8bLoNJZAhI9erNJlNZA6KxU0LGlf9wOZCIAMPRwZA8ZD";
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
