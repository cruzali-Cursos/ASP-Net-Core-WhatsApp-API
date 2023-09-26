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
                string accessToken = "EAACAkOtc2IQBO9OM3ZChjGy7ueZB0t9UN6ZCHKZCnaQUovRjCXFhMF9tYyel9LwZC3ykBYUSy0EDaIkZC67zswEeFiOBQZAOZAkbQMlFTfIL04rWfeg7ZADphaInMSgEuS9ZAKmI3ZBCgrpZAAHvoBe6YCECR5XBtobupvWGI6FzSZAYdLzhlRhq4FKYPcOl6NZBGRtk7SXU9UZCZCwbaFijbGhn5ZCgZD";
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
