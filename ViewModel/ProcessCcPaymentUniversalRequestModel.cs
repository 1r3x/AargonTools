﻿using System.ComponentModel.DataAnnotations;

namespace AargonTools.ViewModel
{
    public class ProcessCcPaymentUniversalRequestModel
    {
        [Required]
        public string? debtorAcct { get; set; }
        [Required]
        public string? ccNumber { get; set; }
        [Required]
        public string? expiredDate { get; set; }
        [Required]
        public string? cvv { get; set; }
        public string? cardHolderName { get; set; }
        [Required]
        public int? numberOfPayments { get; set; }
        [Required]
        public decimal amount { get; set; }
        [Required]
        public string? sif { get; set; }
        [Required]
        public bool? hsa { get; set; }
        //[RequiredIf("hsa", true)]
        //public string? key { get; set; }
        //[RequiredIf("hsa", true)]
        //public string? pin { get; set; }
    }
}
