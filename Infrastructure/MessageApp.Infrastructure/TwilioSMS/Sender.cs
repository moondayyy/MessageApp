using MessageApp.Application.Twilio;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MessageApp.Infrastructure.TwilioSMS
{
    public class Sender:ISenderSMS
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        public Sender(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        public async Task SendSMSAsync(string phoneNumber)
        {
            var verifyCode = new Random().Next(100000, 999999).ToString();
            string accountSid = _configuration.GetValue<string>("Twilio:TWILIO_ACCOUNT_SID");
            string authToken = _configuration.GetValue<string>("Twilio:TWILIO_AUTH_TOKEN");
            string messagingServiceSid = _configuration.GetValue<string>("Twilio:MessagingServiceSid");
            TwilioClient.Init(accountSid, authToken);

            var message = await MessageResource.CreateAsync(
                body: verifyCode,
                messagingServiceSid: messagingServiceSid,
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
            Debug.WriteLine(message.Status);
            _cache.Set(phoneNumber, verifyCode, TimeSpan.FromSeconds(300));
        }

        public string VerifyCodeCheck(string phoneNumber, string reqVerifyCode)
        {
            if (!string.IsNullOrEmpty((string)_cache.Get(phoneNumber))){
                if (reqVerifyCode.Trim() == _cache.Get(phoneNumber).ToString())
                {
                    _cache.Remove(phoneNumber);
                    return "Giriş başarılı..";
                }
                else
                {
                    return "Giriş başarısız!";
                }
            }
            else
            {
                return "Bu telefona ait doğrulama kodu bulunamadı!";
            }
            

        }


        
    }
}
