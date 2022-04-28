using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Application.Twilio
{
    public interface ISenderSMS
    {
        Task SendSMSAsync(string phoneNumber);
        string VerifyCodeCheck(string phoneNumber, string reqVerifyCode);
    }
}
