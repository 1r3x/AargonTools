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


        public string GetClientIpAddress()
        {
            string ip = _httpContextAccessor.HttpContext.GetServerVariable("REMOTE_HOST");
            if (ip == null)
            {
                ip = _httpContextAccessor.HttpContext.GetServerVariable("REMOTE_ADDR");
            }
            return ip;
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
        string GetClientIpAddress();
    }
}
