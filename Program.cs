using ASP.NetCore_WhatsApp_1.Services.OpenAI.ChatGPT;
using ASP.NetCore_WhatsApp_1.Services.WhatsappCloud.SendMessage;
using ASP.NetCore_WhatsApp_1.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// inyeccion de dependencias
builder.Services.AddSingleton<IWhatsappCloudSendMessage,  WhatsappCloudSendMessage>();
builder.Services.AddSingleton<IUtil, Util>();
builder.Services.AddSingleton<IChatGPTService, ChatGPTService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
