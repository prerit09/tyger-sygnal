using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Twilio;

namespace TygerSygnal
{
    class FuncConfig
    {
        private readonly IConfigurationRoot config;
        private string twilioAuthToken;

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
        }

        public async Task TwilioInitAsync()
        {
            this.twilioAuthToken =
                this.config["TWILIO_AUTH_TOKEN"]
                ?? await GetVaultSecretAsync("twilio-auth-token");

            TwilioClient.Init(this.TwilioAccountSid, twilioAuthToken);
        }

        private async Task<string> GetVaultSecretAsync(string secret)
        {
            var serviceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(serviceTokenProvider.KeyVaultTokenCallback));

            var secretUri = SecretUri(secret);
            var secretBundle = await keyVaultClient.GetSecretAsync(secretUri);

            return secretBundle?.Value;
        }

        private string SecretUri(string secret)
        {
            return $"{this.config["KEY_VAULT_URI"]}/secrets/{secret}";
        }
    }
}
