using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Common;
using Twilio.TwiML;
using Twilio.AspNet.Core;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/twilio")]
    public class SmsController : ControllerBase
    {
        /// <summary>
        /// If the user submitting a transaction text message is not a current user,
        /// send them a tokenized link to create their account. If the phone number is associated with
        /// a household invite jump to that workflow. We can store the message in a caching layer so that when the signup happens,
        /// the message is submitted and the transaction record is created. After this, all text messages will go through
        /// </summary>
       /* public void SendAccountSignUpSms()
        {

        }*/

        /// <summary>
        /// This is simple but it gets the job done if the user doesn't want to give Plaid access to
        /// financial data.
        /// The message is formatted as Date Amount Category(optional).
        /// if the category isn't found on the user record, create a new rule until we can
        /// implement some NLP to auto categorize transactions.
        /// </summary>
        [HttpPost("transaction")]
        public IActionResult CreateTransaction([FromForm] SmsRequest incomingMessage)
        {
            Console.WriteLine(incomingMessage.Body);
            var messagingResponse = new MessagingResponse();
            return Ok(incomingMessage);
        }
    }
}