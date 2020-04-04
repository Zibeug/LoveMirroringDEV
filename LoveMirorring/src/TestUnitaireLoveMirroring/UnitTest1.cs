using Microsoft.VisualStudio.TestTools.UnitTesting;
using IdentityServerAspNetIdentity;
using IdentityServerAspNetIdentity.Models;
using IdentityServerAspNetIdentity.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using IdentityModel.Client;
using Microsoft.AspNetCore;
using IdentityServerAspNetIdentity.Services;
using Microsoft.AspNetCore.Mvc;

namespace TestUnitaireLoveMirroring
{
    [TestClass]
    public class UnitTest1
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        [TestMethod]
        public void TestMethodSignupAsync() {
            ApplicationUser user = new ApplicationUser();
            user.UserName = "TestUnit";
            user.PasswordHash = "sadisisi/8383833";
            user.PhoneNumber = "+41790001122";
            user.Email = "test@test.com";
            user.Firstname = "Test";
            user.LastName = "Test";

            user.Sexeid = 1;

            
            AccountController ac1 = new AccountController(_emailSender, _userManager);
            Task t1 = ac1.SignUpSend(user);
            Assert.IsTrue(t1.IsCompleted);

        }
    }
}
