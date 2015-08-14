using MPERP2015.MP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MPERP2015.Controllers
{
    public class AuthenticateViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class AuthenticationController : ApiController
    {
        //[Route("authenticate")]
        public IHttpActionResult Authenticate(AuthenticateViewModel viewModel)
        {
            if (!ValidUser(viewModel.Username, viewModel.Password))
            {
                return Ok(new { success = false, message = "請確認您的帳號密碼!" });
            }

            return Ok(new { success = true, token=viewModel.Username });
        }
        bool ValidUser(string userName, string password)
        {
            bool result = false;

            MembershipModelContainer db = new MembershipModelContainer();
            var user = db.Users.Find(userName);
            if ((user != null) && (user.Password == password))
            {
                result = true;
            }
            return result;
        }
    }
}
