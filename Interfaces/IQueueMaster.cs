using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Interfaces
{
    public interface IQueueMaster
    {
         int? Employee { get; set; }
         int? Priority { get; set; }
         string DebtorAcct { get; set; }
    }
}
