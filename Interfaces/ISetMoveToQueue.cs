﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface ISetMoveToQueue
    {
        Task<ResponseModel> SetMoveToQueue(string debtorAcct,string type, string environment);
    }
}
