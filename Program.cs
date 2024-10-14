using ASP.NetCore_WhatsApp_1.Services.Firebase;
using ASP.NetCore_WhatsApp_1.Services.OpenAI.ChatGPT;
using ASP.NetCore_WhatsApp_1.Services.WhatsappCloud.GetAccessToken;
using ASP.NetCore_WhatsApp_1.Services.WhatsappCloud.SendMessage;
using ASP.NetCore_WhatsApp_1.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();

builder.Services.AddControllers();
// inyeccion de dependencias
builder.Services.AddSingleton<IWhatsappCloudSendMessage,  WhatsappCloudSendMessage>();
// Se creo primero la clase, luego la interfaz IUtil, la clase hereda de IUtil, Inyeccion de dependencias.
builder.Services.AddSingleton<IUtil, Util>();
builder.Services.AddSingleton<IChatGPTService, ChatGPTService>();
// De esta forma se deja disponible para inyección de dependencias.
builder.Services.AddSingleton<FirebaseService>();

// Background Service
builder.Services.AddHostedService<GetToken>();

// Cargar la configuración desde appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
