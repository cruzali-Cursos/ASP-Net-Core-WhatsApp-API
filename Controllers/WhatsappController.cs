using ASP.NetCore_WhatsApp_1.Models.WhatsappCloud;
using ASP.NetCore_WhatsApp_1.Services.OpenAI.ChatGPT;
using ASP.NetCore_WhatsApp_1.Services.WhatsappCloud.SendMessage;
using ASP.NetCore_WhatsApp_1.Util;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NetCore_WhatsApp_1.Controllers
{
    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsappController : Controller
    {
        private readonly IWhatsappCloudSendMessage _whatsappCloudSendMessage;
        private readonly IUtil _iutil;
        private readonly IChatGPTService chatGPTService;
        private readonly IConfiguration _configuration;

        public WhatsappController(IWhatsappCloudSendMessage whatsappCloudSendMessage,
                                    IUtil iutil, IChatGPTService chatGPTService, IConfiguration configuration)
        {
            _whatsappCloudSendMessage = whatsappCloudSendMessage;
            _iutil = iutil;
            this.chatGPTService = chatGPTService;
            _configuration = configuration;
        }

        // API Test /api/whatsapp/test
        // Aqui se arma el payload data
        [HttpGet("test")]
        public async Task<IActionResult> Sample()
        {
            var data = new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = _configuration["AppSettings:_to"]?.ToString(),
                type = "text",
                text = new
                {
                    body = "Mensaje texto desde API .NET core"
                }
            };

            var result = await _whatsappCloudSendMessage.Execute(data);
            // Validar Estado del envio
            if (result)
            {
                return Ok("Envío de mensaje de test exitoso. destinatario: " + data.to);
            } else
            {
                return Ok("Ocurrió un error al intentar enviar el mensaje test. Destinatario: " + data.to);
            }
        }

        // Se hace verificación del token
        // Para autenticacion WA Cloud API
        [HttpGet]
        public IActionResult VerifyToken()
        {
            string accessToken = "TOKENGENERAODEMILADOERFIUHO4IIONIOEDRFNFJIKGFG";
            var token = Request.Query["hub.verify_token"].ToString();
            var challenge = Request.Query["hub.challenge"].ToString();

            if (challenge != null && token != null && token == accessToken)
            {
                return Ok(challenge);
            } else
            {
                return BadRequest();
            }
        }

        // Capturar el modelo WhatsappCloudModel
        [HttpPost]
        public async Task<IActionResult> ReceivedMessage([FromBody] WhatsappCloudModel body)
        {
            try
            {
                // Navegar el arbol de nodos ...
                var message = body.Entry[0]?.Changes[0]?.Value?.Messages[0];

                if (message != null)
                {
                    // Obtener el número desde donde se recibe el mensaje y el mensaje
                    var userNumber = message.From.Length == 13 ? message.From.Remove(2, 1) : message.From; //message.From;
                    var userText = GetUserText(message);

                    List<object> listObjectMessage = new List<object>();

                    // TO-DO
                    // Armar un array de palabras frecuentes con una intención para responder mejor
                    
                    #region SIN Chatgpt

                    if (userText.ToUpper().Contains("HOLA"))
                    {
                        var obj = _iutil.TextMessage("Hola! ¿En qué puedo ayudarte? 😄", userNumber);
                        listObjectMessage.Add(obj); 
                    } else if (userText.ToUpper().Contains("GRACIAS"))
                    {
                        listObjectMessage.Add((object)_iutil.TextMessage("Estoy aquí para servirte. 😊", userNumber));
                    } else if (userText.ToUpper().Contains("ADIOS") || userText.ToUpper().Contains("BYE"))
                    {
                        listObjectMessage.Add((object)_iutil.TextMessage("Aquí seguiré para ayudar nuevamente " +
                            "cuando lo necesites", userNumber));
                    }
                    else
                    {
                        //listObjectMessage.Add((object)_iutil.TextMessage("Lo siento, no puedo entenderte. " +
                        //   "Intenta decirmelo de otra forma", userNumber));
                    }

                    // Categorizar mensajes
                    switch (userText.ToUpper())
                    {
                        case "TEXT":
                            listObjectMessage.Add((object)_iutil.TextMessage("Mensaje personalizado 23435", userNumber));
                            break;
                        case "IMAGE":
                            listObjectMessage.Add((object)_iutil.ImageMessage(
                                "https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/image_whatsapp.png",
                                userNumber));
                            break;
                        case "AUDIO":
                            listObjectMessage.Add((object)_iutil.AudioMessage(
                                "https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/audio_whatsapp.mp3",
                                userNumber));
                            break;
                        case "VIDEO":
                            listObjectMessage.Add((object)_iutil.VideoMessage(
                                "https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/video_whatsapp.mp4",
                                userNumber));
                            break;
                        case "DOCUMENT":
                            listObjectMessage.Add((object)_iutil.DocumentMessage(
                                "https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/document_whatsapp.pdf",
                                userNumber));
                            break;
                        case "LOCATION":
                            listObjectMessage.Add((object)_iutil.LocationMessage(userNumber));
                            break;
                        case "BUTTON":
                            listObjectMessage.Add((object)_iutil.ButtonsMessage(userNumber));
                            break;
                        //default:
                        //    objectMessage = _iutil.TextMessage("Caso no contemplado", userNumber);
                        //    break;
                    }
                    #endregion

                    #region CON Chatgpt
                    var responseChatGPT = await chatGPTService.Execute(userText);
                    var objectMessage = _iutil.TextMessage(responseChatGPT, userNumber);
                    listObjectMessage.Add(objectMessage);

                    #endregion

                    foreach (var item in listObjectMessage)
                    {
                        await _whatsappCloudSendMessage.Execute(item);
                    }
                }

                // Siempre responder, sino se vuelve un ciclo intentando recibir una respuesta
                return Ok("EVENT_RECEIVED");
                
            } catch (Exception ex)
            {
                // Siempre responder, sino se vuelve un ciclo intentando recibir una respuesta
                return Ok("EVENT_RECEIVED");
            }
        }

        // Obtener el mensaje que llega desde el usuario (cliente whatsapp)
        // Se identifica el tipo de mensage recibido
        // INTERACTIVE cuando se responde a una lista de opciones o a un boton
        // Sabiendo el tipo de mensaje se sabe que propiedad del objeto message leer.
        // Se retorna un string con el contenido recibido
        private string GetUserText(Message message)
        {
            string typeMessage = message.Type;

            if (typeMessage.ToUpper() == "TEXT")
            {
                return message.Text.Body;
            } else if (typeMessage.ToUpper() == "INTERACTIVE")
            {
                string interactiveType = message.Interactive.Type;

                if (interactiveType.ToUpper() == "LIST_REPLY")
                {
                    return message.Interactive.List_Reply.Title;
                } else if (interactiveType.ToUpper() == "BUTTON_REPLY")
                {
                    return message.Interactive.Button_Reply.Title;
                } else
                {
                    return string.Empty;
                }
            } else
            {
                return string.Empty;
            }            
        }



    }
}
