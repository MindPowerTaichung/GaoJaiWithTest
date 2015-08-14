using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPERP2015.Controllers;
using System.Collections.Generic;
using MPERP2015.MP;
using System.Web.Http.Results;
using System.Net;
using System.Web;
using System.IO;
using System.Security.Principal;
using System.Web.Hosting;
using MPERP2015.MP.Log;
using System.Web.Routing;
using System.Web.Http.Controllers;

namespace GaoJaiUnitTest
{
    [TestClass]
    public class TestMembershipController
    {
        [TestMethod]
        public void GetAllRoles_ShouldReturnRoleViewModel()
        {
            // arrange
            var controller = new MembershipController();
            // act
            var result = controller.GetRoles();
            // assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<RoleViewModel>));
        }

        [TestMethod]
        public void GetRolesById1_ShouldReturnRoleAdmin()
        {
            // arrange
            var controller = new MembershipController();
            // act
            var result = controller.GetRoles(1) as OkNegotiatedContentResult<RoleViewModel>;
            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("管理者", result.Content.Name);
        }

        [TestMethod]
        public void PostRole_ShouldAddARole()
        {
            // arrange
            MPAccessLog.SetLogPath(string.Format("{1}\\App_Data\\AccessLog{0:yyyyMM}.txt", DateTime.Now, @"C:\Users\鄭淑芬\Source\Repos\GaoJaiWithTest\GaoJai"));
            SimpleWorkerRequest request = new SimpleWorkerRequest("", "", "", null, new StringWriter());
            HttpContext.Current = new HttpContext(request); 
            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity("admin"),
                new string[0]
                );
            var controller = new MembershipController();
            RoleViewModel roleTest = new RoleViewModel { Name = "測試角色" };
            // act
            var result = controller.PostRole(roleTest);
            // assert
            //Assert.AreEqual(HttpStatusCode.Created, (result as StatusCodeResult).StatusCode);
            //Assert.IsTrue((result as OkNegotiatedContentResult<RoleViewModel>).Content.Id>0);
            //Assert.AreEqual(roleTest.Name,(result as OkNegotiatedContentResult<RoleViewModel>).Content.Name);
            Assert.IsNotNull(result);
        }
    }
}
