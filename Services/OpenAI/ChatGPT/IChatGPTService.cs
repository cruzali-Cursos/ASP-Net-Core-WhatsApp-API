namespace ASP.NetCore_WhatsApp_1.Services.OpenAI.ChatGPT
{
    public interface IChatGPTService
    {
        Task<string> Execute(string textUser);

    }
}
