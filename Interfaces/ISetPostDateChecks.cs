﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface ISetPostDateChecks
    {
        Task<ResponseModel> SetPostDateChecks(SetPostDateChecksRequestModel request,string environment);
    }
}
