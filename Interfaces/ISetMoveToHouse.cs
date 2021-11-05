﻿using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface ISetMoveToHouse
    {
        Task<ResponseModel> SetMoveToHouse(string debtorAcct, string environment);
    }
}
