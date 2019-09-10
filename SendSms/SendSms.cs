using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TygerSygnal
{
    public static class SendSms
    {
        [FunctionName("SendSms")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "tygerbytes/sygnal")] HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            var funcConfig = new FuncConfig(context);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<SendSmsRequest>(requestBody);
            if (!request.IsValid())
            {
                return new BadRequestObjectResult(request.InvalidRequestReason());
            }

            log.LogInformation(Utils.ToJson(request));

            await funcConfig.TwilioInitAsync();
            try
            {
                foreach (var number in Utils.SplitTo<PhoneNumber>(request.ToPhoneNumbers))
                {
                    var messageResource = MessageResource.Create(
                        body: $"{request.Message}\n{funcConfig.SmsSignature}",
                        from: funcConfig.TwilioPhoneNumber,
                        to: number
                    );
                    log.LogInformation($"SMS to {number} has been {messageResource.Status}.");
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = 500
                };
                throw;
            }

            return new OkObjectResult($"Message sent.");
        }
    }
}
