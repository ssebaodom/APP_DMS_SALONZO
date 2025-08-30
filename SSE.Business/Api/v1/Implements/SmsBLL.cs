using Microsoft.Extensions.Configuration;
using SSE.Business.Api.v1.Interfaces;
using SSE.Core.AuthenticationIdentity;
using SSE.Core.Common.Constants;
using SSE.Core.Services.Helpers;
using System;

namespace SSE.Business.Api.v1.Implements
{
    public class SmsBLL : ISmsBLL
    {
        private readonly IConfiguration configuration;

        public SmsBLL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private SMSData createSmsData(string content, string phones)
        {
            SMSData data = new SMSData();
            var smsConfig = this.configuration.GetSection(CONFIGURATION_KEYS.SMS_CONFIG);
            string userName = smsConfig.GetValue<string>(CONFIGURATION_KEYS.SMS_USER_NAME);
            string passWord = CryptHelper.GetHashMD5(smsConfig.GetValue<string>(CONFIGURATION_KEYS.SMS_PASSWORD));
            string brandName = smsConfig.GetValue<string>(CONFIGURATION_KEYS.SMS_BRAND_NAME);
            data.UserName = userName;
            data.Password = passWord;
            data.SmsContent = content;
            data.Phones = phones;
            data.BrandName = brandName;
            data.CheckSum = "";
            data.TimeSend = "";
            data.ClientId = Guid.NewGuid().ToString();
            return data;
        }

        public T sendMessage<T>(string content, string phones)
        {
            SMSData data = createSmsData(content, phones);
            string url = this.configuration.GetSection(CONFIGURATION_KEYS.SMS_CONFIG).GetValue<string>(CONFIGURATION_KEYS.SMS_URL);
            return RequestHelper.postRequest<T>(url, null, data);
        }
    }
}