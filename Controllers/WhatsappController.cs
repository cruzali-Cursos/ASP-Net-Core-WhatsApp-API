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
        public async Task<IActionResult> ReceivedMessage([FromBody] )
        {

        }

    }
}
