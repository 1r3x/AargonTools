using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface ISetDialing
    {
        Task<ResponseModel> SetDialing(SetDialingRequestModel request, string environment);
    }
}
