using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AargonTools.Models.Helper
{
    public class TokenizeResponseModelForInstaMed
    {
        public TokenizeResponseModelForInstaMed(string jsonResponse)
        {
            var jObject = JObject.Parse(jsonResponse);
            var cardResult = (JObject)jObject["CardResult"];
            if (cardResult == null) return;
            var card = new LcgCardInfo()
            {
                PaymentMethodId = (string)jObject["PaymentPlanID"],
                EntryMode = (string)cardResult["EntryMode"],
                BinNumber = (string)cardResult["BINNumber"],
                ExpirationMonth = (int)cardResult["ExpirationMonth"],
                ExpirationYear = (int)cardResult["ExpirationYear"],
                LastFour = (string)cardResult["LastFour"],
                Type = (string)cardResult["Type"],
                IsActive = true

            };
            CardInfo = card;
        }

        public LcgCardInfo CardInfo { get; set; }

    }
}
