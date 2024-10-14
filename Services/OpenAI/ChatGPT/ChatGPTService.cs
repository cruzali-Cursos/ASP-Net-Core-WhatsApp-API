using OpenAI_API;
using OpenAI_API.Completions;

namespace ASP.NetCore_WhatsApp_1.Services.OpenAI.ChatGPT
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly IConfiguration _configuration;
        public ChatGPTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> Execute(string textUser)
        {
            try
            {                
                string openIAApiKey = _configuration["AppSettings:_openIAApiKey"];
                var openaiService = new OpenAIAPI(openIAApiKey);

                var completion = new CompletionRequest
                {
                    Prompt = textUser,
                    Model = "text-davinci-003",
                    NumChoicesPerPrompt = 1,
                    MaxTokens = 200
                };

                var result = await openaiService.Completions.CreateCompletionAsync(completion);

                if (result != null && result.Completions.Count > 0)
                {
                    return result.Completions[0].Text;
                }

                return "Lo siento, sucedió un prolema. Intentalo más tarde.";
            } catch (Exception ex) { 
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return "Lo siento, sucedió un prolema. Intentalo más tarde.";
            }
        }
    }
}
