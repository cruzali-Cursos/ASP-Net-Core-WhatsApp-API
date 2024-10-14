using ASP.NetCore_WhatsApp_1.Services.Firebase;

namespace ASP.NetCore_WhatsApp_1.Services.WhatsappCloud.GetAccessToken
{
    public class GetToken : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;
        private readonly FirebaseService _firebaseService;

        public GetToken(IHttpClientFactory httpClientFactory, IConfiguration configuration, FirebaseService firebaseService)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _firebaseService = firebaseService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RenewTokenAsync();
                // Esperar hasta que sea necesario renovar el token
                await Task.Delay(TimeSpan.FromHours(23), stoppingToken); // 23 horas antes de la renovación
            }
        }


        // Obtener un nuevo token, este se guarda en Firebase
        private async Task RenewTokenAsync()
        {
            var client = _httpClientFactory.CreateClient();            
            var requestUrl = $"https://graph.facebook.com/v17.0/oauth/access_token?grant_type=fb_exchange_token&client_id={_configuration["AppSettings:_appId"]}&client_secret={_configuration["AppSettings:_appSecret"]}&fb_exchange_token={_configuration["AppSettings:_refreshToken"]}";
            
            
            Console.WriteLine($"\nclient: {client}");
            Console.WriteLine($"\nrequestUrl: {requestUrl}");
            Console.WriteLine("\n_appId: " + _configuration["AppSettings:_appId"]);

            var response = await client.GetAsync(requestUrl);
            Console.WriteLine($"\n\n response: {response}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                // Procesa el contenido y actualiza el token en tu sistema
                Console.WriteLine($"******************************* Nuevo token de acceso obtenido: {content}");
                // Guarda el nuevo token en tu sistema para su uso posterior

                await _firebaseService.SaveDataAsync("accessToken", new { Token = content });
            }
            else
            {
                Console.WriteLine($"******************************* Error al renovar el token: {response.StatusCode}\n\n");
            }
        }
    }
}
