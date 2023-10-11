using ASP.NetCore_WhatsApp_1.Models.WhatsappCloud;
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

        public WhatsappController(IWhatsappCloudSendMessage whatsappCloudSendMessage,
                                    IUtil iutil)
        {
            _whatsappCloudSendMessage = whatsappCloudSendMessage;
            _iutil = iutil;
        }


        [HttpGet("test")]
        public async Task<IActionResult> Sample()
        {
            var data = new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = "522222996577",
                type = "text",
                text = new
                {
                    body = "Mensaje texto desde API .NEt core"
                }
            };

            var result = await _whatsappCloudSendMessage.Execute(data);
            return Ok("Ok Sample");
        }

        [HttpGet]
        public IActionResult VerifyToken()
        {
            string accessToken = "DFGDFGRFGFGHFGH567345DFGDFG2345DFGHDFGH456DFGH";
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

        [HttpPost]
        public async Task<IActionResult> ReceivedMessage([FromBody] WhatsappCloudModel body)
        {
            try
            {
                var message = body.Entry[0]?.Changes[0]?.Value?.Messages[0];

                if (message != null)
                {
                    var userNumber = message.From.Length == 13 ? message.From.Remove(2, 1) : message.From; //message.From;
                    var userText = GetUserText(message);

                    object objectMessage;

                    // TO-DO
                    // Armar un arry de palabras frecuentes con una intención para responder mejor


                    if (userText.ToUpper().Contains("HOLA"))
                    {
                        objectMessage = _iutil.TextMessage("Hola! ¿En qué puedo ayudarte? 😄", userNumber);
                    } else if (userText.ToUpper().Contains("GRACIAS"))
                    {
                        objectMessage = _iutil.TextMessage("Estoy aquí para servirte. 😊", userNumber);
                    } else if (userText.ToUpper().Contains("ADIOS") || userText.ToUpper().Contains("BYE"))
                    {
                        objectMessage = _iutil.TextMessage("Aquí seguiré para ayudar nuevamente cuando lo necesites", userNumber);
                    }
                    else
                    {
                        objectMessage = _iutil.TextMessage("Lo siento, no puedo entenderte. Intenta decirmelo de otra forma", userNumber);
                    }

                    // Categorizar mensajes
                    switch (userText.ToUpper())
                    {
                        case "TEXT":
                            objectMessage = _iutil.TextMessage("Mensaje personalizado 23435", userNumber);
                            break;
                        case "IMAGE":
                            objectMessage = _iutil.ImageMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/image_whatsapp.png", userNumber);
                            break;
                        case "AUDIO":
                            objectMessage = _iutil.AudioMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/audio_whatsapp.mp3", userNumber);
                            break;
                        case "VIDEO":
                            objectMessage = _iutil.VideoMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/video_whatsapp.mp4", userNumber);
                            break;
                        case "DOCUMENT":
                            objectMessage = _iutil.DocumentMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/document_whatsapp.pdf", userNumber);
                            break;
                        case "LOCATION":
                            objectMessage = _iutil.LocationMessage(userNumber);
                            break;
                        case "BUTTON":
                            objectMessage = _iutil.ButtonsMessage(userNumber);
                            break;
                        //default:
                        //    objectMessage = _iutil.TextMessage("Caso no contemplado", userNumber);
                        //    break;
                    }

                    await _whatsappCloudSendMessage.Execute(objectMessage);
                }


                return Ok("EVENT_RECEIVED");
                
            } catch (Exception ex)
            {
                return Ok("EVENT_RECEIVED");
            }
        }

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
