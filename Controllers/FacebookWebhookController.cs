using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Paynter.FacebookMessenger.Models.Webhooks;
using Paynter.FacebookMessenger.Services;

namespace DotNetCoreChatBots.Controllers
{
    public class FacebookWebhookController : Controller
    {
        private FacebookMessengerService _facebookMessengerService;
        private ILogger<FacebookWebhookController> _logger;
        private ChatBotHelper _chatBotHelper;

        public FacebookWebhookController(ILogger<FacebookWebhookController> logger, FacebookMessengerService facebookMessengerService, ChatBotHelper chatBotHelper)
        {
            _facebookMessengerService = facebookMessengerService;
            _logger = logger;
            _chatBotHelper = chatBotHelper;
        }

        [HttpGet("api/facebookwebhook")]
        public object Index(WebhookHubRequest hub)
        {
            if (_facebookMessengerService.ValidateHubRequest(hub))
            {
                return hub.Challenge;
            }
            _logger.LogInformation("Failed webhook subscription attempt - {@request}", hub);
            HttpContext.Response.StatusCode = 403;
            return null;
        }

        [HttpPost("api/facebookwebhook")]
        public void Index([FromBody]WebhookCallback request)
        {
            _facebookMessengerService.ProcessWebhookRequest(request);
        }
        


        // private async Task SendGenericMessage(string recipientId)
        // {
        //     var messageData = new
        //     {
        //         recipient = new
        //         {
        //             id = recipientId
        //         },
        //         message = new
        //         {
        //             attachment = new
        //             {
        //                 type = "template",
        //                 payload = new
        //                 {
        //                     template_type = "generic",
        //                     elements = new[]{
        //                     new {
        //                         title = "rift",
        //                         subtitle = "Next-generation virtual reality",
        //                         item_url = "https://www.oculus.com/en-us/rift/",
        //                         image_url = "http://messengerdemo.parseapp.com/img/rift.png",
        //                         buttons = new[]{
        //                             new {
        //                                 type = "web_url",
        //                                 url = "https://www.oculus.com/en-us/rift/",
        //                                 title = "Open Web URL"
        //                             }

        //                         }
        //                     }
        //                 }
        //                 }
        //             }
        //         }
        //     };

        //     await _facebookMessengerService.CallSendApi(messageData);
        // }
    }
}
