using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FoolProof.Core;

namespace AargonTools.ViewModel
{
    public class ProcessCcPaymentUniversalRequestModel
    {
        [Required]
        public string? debtorAcc { get; set; }
        [Required]
        public string? ccNumber { get; set; }
        [Required]
        public string? expiredDate { get; set; }
        [Required]
        public string? cvv { get; set; }
        [Required]
        public int? numberOfPayments { get; set; }
        [Required]
        public decimal amount { get; set; }
        [Required]
        public string? sif { get; set; }
    }
}
