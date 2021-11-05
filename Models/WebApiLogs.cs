using System;
using System.ComponentModel.DataAnnotations;

namespace AargonTools.Models
{
    public class WebApiLogs
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        [StringLength(128)]
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
        public string UserName { get; set; }
    }
}