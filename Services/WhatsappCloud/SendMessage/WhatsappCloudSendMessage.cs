using ASP.NetCore_WhatsApp_1.Services.Firebase;
using Newtonsoft.Json;
using System.Text;

namespace ASP.NetCore_WhatsApp_1.Services.WhatsappCloud.SendMessage
{
    public class WhatsappCloudSendMessage : IWhatsappCloudSendMessage
    {
        private readonly IConfiguration _configuration;
        private readonly FirebaseService _firebaseService;

        public WhatsappCloudSendMessage(IConfiguration configuration, FirebaseService firebaseService)
        {
            _configuration = configuration;
            _firebaseService = firebaseService;
        }


        // 
        public async Task<bool> Execute(object model)
        {
            var client = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));

            using (var content = new ByteArrayContent(byteData))
            {
                string accessMeta_WA_Token = await _firebaseService.GetAccessTokenAsync("Meta_WA_ASPNetCore", "accessToken");
                string endpoint = _configuration["AppSettings:_endPoint"];
                string phoneNumberId = _configuration["AppSettings:_phoneNumberId"];
                string versionAPI = _configuration["AppSettings:_versionAPI"];

                string uri = $"{endpoint}/{versionAPI}/{phoneNumberId}/messages?";

                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessMeta_WA_Token}");

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
