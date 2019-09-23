using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Twilio;

namespace TygerSygnal
{
    class FuncConfig
    {
        private readonly IConfigurationRoot config;

        private string TwilioAccountSid => this.config["TWILIO_ACCOUNT_SID"];
        public string TwilioPhoneNumber => this.config["TWILIO_PHONE_NUMBER"];
        public string SmsSignature => this.config["SMS_SIGNATURE"];

        public FuncConfig(ExecutionContext context)
        {
            this.config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            this.InitTwilio();
        }

        public void InitTwilio()
        {
            var twilioAuthToken = this.config["TWILIO_AUTH_TOKEN"];
            TwilioClient.Init(this.TwilioAccountSid, twilioAuthToken);
        }
    }
}
