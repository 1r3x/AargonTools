using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AargonTools.Manager.GenericManager
{
    public class UserService: IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetLoginUserName()
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims;
            var emailCheck = "";
            foreach (var x in claims)
            {
                if (!x.Value.Contains("@")) continue;
                var eMailAddress = new System.Net.Mail.MailAddress(x.Value);
                emailCheck = eMailAddress.Address;

            }


            var email = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated ? emailCheck : "anonymous";

            return email;
        }
    }

    public interface IUserService
    {
        string GetLoginUserName();
    }
}
