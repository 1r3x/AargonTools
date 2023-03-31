﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Models.Helper
{
    public class SaleRequestModelForInstamed
    {
        public InstaMedOutlet Outlet { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentPlanType { get; set; }
        public Card Card { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public Patient Patient { get; set; }
        public String PaymentMethodID { get; set; }
    }

    public class Card
    {
        public string EntryMode { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVN { get; set; }
        public string CardHolderName { get; set; }
        public string CardHolderEmail { get; set; }
        public bool IsCardDataEncrypted { get; set; }
        public bool IsEMVCapableDevice { get; set; }
    }

    public class BillingAddress
    {
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class Patient
    {
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}