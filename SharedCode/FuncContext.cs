using System;

namespace TygerSygnal
{
    class FuncContext
    {
        public static readonly string TwilioAccountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
        public static readonly string TwilioAuthToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
        public static readonly string TwilioPhoneNumber = Environment.GetEnvironmentVariable("TWILIO_PHONE_NUMBER");
        public static readonly string SmsSignature = Environment.GetEnvironmentVariable("SMS_SIGNATURE");
    }
}
