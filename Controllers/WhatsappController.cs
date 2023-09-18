using ASP.NetCore_WhatsApp_1.Models.WhatsappCloud;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NetCore_WhatsApp_1.Controllers
{
    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsappController : Controller
    {
        [HttpGet("test")]
        public IActionResult Sample()
        {
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
                    var userNumber = message.From;
                    var userText = GetUserText(message);
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
