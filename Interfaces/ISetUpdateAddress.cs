using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface ISetUpdateAddress
    {
        Task<ResponseModel> SetUpdateAddress(SetUpdateAddressRequestModel setUpdateAddressRequestModelModel, string environment);
    }
}
