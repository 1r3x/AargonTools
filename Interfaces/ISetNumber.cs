﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface ISetNumber
    {
        Task<ResponseModel> SetNumber(string debtorAcct, string cellPhoneNo, string environment);
    }
}