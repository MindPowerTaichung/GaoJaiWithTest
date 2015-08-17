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
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Hosting;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace GaoJaiUnitTest
{
    [TestClass]
    public class TestMembershipController
    {
        //[ClassInitialize]沒有作用?
        //public static void ClassInit(TestContext context)
        //{
        //    Debug.WriteLine("ClassInit");
        //    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("單元測試a"), new[] { "tester" });
        //}
        [TestInitialize()]
        public void Initialize()
        {
            Debug.WriteLine("TestMethodInit");
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("單元測試b"), new[] { "tester" });
        }

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
            Assert.IsNotNull(result.Content);
            Assert.AreEqual("管理者", result.Content.Name);
        }

        [TestMethod]
        public void PostRole_ShouldAddARole()
        {
            // arrange
            var controller = new MembershipController();            
            RoleViewModel roleTest = new RoleViewModel { Name = "測試角色" };

            // act
            var result = controller.PostRole(roleTest) as CreatedAtRouteNegotiatedContentResult<RoleViewModel>;
            
            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("GetRoleById", result.RouteName);
            Assert.IsTrue(Convert.ToInt32(result.RouteValues["id"]) > 0);
            Assert.AreEqual(roleTest.Name, result.Content.Name);
            
        }

        [TestMethod]
        public void UpdateRole_ShouldUpdateRoleName()
        {
            // arrange
            var controller = new MembershipController();
            var role = controller.GetRoles().Where(r => r.Name == "測試角色").Select(r=>r).First();
            RoleViewModel roleTest = new RoleViewModel { Id = role.Id, Name = "測試角色AAA", TimestampString=role.TimestampString };

            // act
            var result = controller.PutRole(roleTest.Id, roleTest) as OkNegotiatedContentResult<RoleViewModel>;

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Content.Id ==roleTest.Id);
            Assert.IsTrue(result.Content.Name == roleTest.Name);
            
        }

        [TestMethod]
        public void DeleteRole_ShouldDeleteARole()
        {
            // arrange
            var controller = new MembershipController();
            var roleIdToDelete = controller.GetRoles().Where(r => r.Name == "測試角色AAA").Select(r => r.Id).First();

            // act
            var result = controller.DeleteRole(roleIdToDelete) as OkNegotiatedContentResult<RoleViewModel>;

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Content.Id > 0);

        }
    }
}
