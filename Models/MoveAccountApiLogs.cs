using System;

namespace AargonTools.Models
{
    public class MoveAccountApiLogs
    {
        public int Id { get; set; }
        public string DebtorAcct { get; set; }
        public int FromQueue { get; set; }
        public int ToQueue { get; set; }
        public DateTime MoveDate { get; set; }
        public string Requestor { get; set; }
    }
}