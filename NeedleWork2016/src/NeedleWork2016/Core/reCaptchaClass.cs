﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeedleWork2016.Core
{
    public class reCaptchaClass
    {
        private string m_Success;
        [JsonProperty("success")] 
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }           
        }

        private List<string> m_ErrorCodes;

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }

        //Method that send request for the check CAPTCHA validation
        public static string Validate(string EncodedResponse)
        {
            var client = new System.Net.WebClient();
            string PrivateKey = "6LeTQxcTAAAAACs7Za-iYjtPzOTOBBHxdx4zW29E";
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));
            var captchaResponse = JsonConvert.DeserializeObject<reCaptchaClass>(reply);
            return captchaResponse.Success;
        }

    }
}
