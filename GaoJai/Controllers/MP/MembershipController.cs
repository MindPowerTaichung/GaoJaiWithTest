using MPERP2015.Membership.Models;
using MPERP2015.MP;
using MPERP2015.MP.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace MPERP2015.Controllers
{
    [Authorize]
    public class MembershipController : ApiController
    {
        MembershipModelContainer db = new MembershipModelContainer();

        #region Membership/Roles
        // GET: api/Roles
        [Route("Membership/Roles")]
        public IEnumerable<RoleViewModel> GetRoles()
        {
            var roles = db.Roles.ToArray<Role>().Select(item => ToRoleViewModel(item));
            return roles;
        }

        // GET: api/Roles/5
        [Route("Membership/Roles", Name="GetRoleById")]
        public IHttpActionResult GetRoles(int id)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(ToRoleViewModel(role));
        }

        // POST: api/Roles
        [HttpPost]
        [Route("Membership/Roles")]
        public IHttpActionResult PostRole(RoleViewModel role_viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Role role = new Role { Id = role_viewModel.Id, Name = role_viewModel.Name };
            db.Roles.Add(role);
            try
            {
                db.SaveChanges();

                //寫入AccessLog
                MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Create, "Role", JsonConvert.SerializeObject(new {role.Id,role.Name }));
            }
            catch (DbEntityValidationException ex)
            {
                var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var getFullMessage = string.Join("; ", entityError);
                var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, exceptionMessage));
            }

            return CreatedAtRoute("GetRoleById", new { id = role.Id }, ToRoleViewModel(role));
        }

        // PUT: api/Roles/5
        [HttpPut]
        [Route("Membership/Roles/{id}")]
        public IHttpActionResult PutRole(int id, RoleViewModel role_viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != role_viewModel.Id)
                return BadRequest();

            //把資料庫中的那筆資料讀出來
            var role_db = db.Roles.Find(id);
            if (role_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    role_db.Name = role_viewModel.Name;
                    db.Entry(role_db).OriginalValues["Timestamp"] = Convert.FromBase64String(role_viewModel.TimestampString);
                    db.SaveChanges();

                    //寫入AccessLog
                    MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Update, "Role", JsonConvert.SerializeObject(new { role_db.Id, role_db.Name }));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.Roles.Find(id) == null)
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));
                }
            }

            return Ok(ToRoleViewModel(role_db));

        }

        // DELETE: api/Roles/5
        [Route("Membership/Roles/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteRole(int id)
        {
            Role role_db = db.Roles.Find(id);
            if (role_db == null)
            {
                return NotFound();
            }

            try
            {
                db.Roles.Remove(role_db);
                db.SaveChanges();

                //寫入AccessLog
                MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Delete, "Role", role_db.Name);
            }
            catch (DbEntityValidationException ex)
            {
                var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var getFullMessage = string.Join("; ", entityError);
                var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, exceptionMessage));
            }


            return Ok(new RoleViewModel { Id = id });
        }

        private RoleViewModel ToRoleViewModel(Role role)
        {
            return new RoleViewModel { Id = role.Id, Name = role.Name, TimestampString = Convert.ToBase64String(role.Timestamp) };
        }
        #endregion

        #region Membership/RoleMenu
        // POST: Membership/RoleMenu
        [HttpPost]
        [Route("Membership/RoleMenu")]
        public IHttpActionResult PostRoleMenu(RoleMenuViewModel roleMenu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var role = db.Roles.Find(roleMenu.RoleId);
            if (role == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "不存在的RoleId!"));

            var menus = db.Menus.Where(item => item.Id==roleMenu.MenuId || item.ParentId==roleMenu.MenuId);

            foreach (var item in menus)
            {
                role.Menus.Add(item);
                //role.Menus.Add(db.Menus.Find( item.ParentId));
            }

            try
            {
                db.SaveChanges();

                //寫入AccessLog
                MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Create, "RoleMenu", JsonConvert.SerializeObject(new { role.Id, role.Name, Menus = menus.Select(m => m.Id+m.Text).ToArray() }));

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
            return Ok();
        }

        // DELETE: Membership/RoleMenu
        [Route("Membership/RoleMenu")]
        [HttpDelete]
        public IHttpActionResult DeleteRoleMenu(RoleMenuViewModel roleMenu)
        {
            var role = db.Roles.Find(roleMenu.RoleId);
            if (role == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "不存在的RoleId!"));

            var menus = db.Menus.Where(item => item.Id == roleMenu.MenuId || item.ParentId == roleMenu.MenuId);

            foreach (var item in menus)
            {
                role.Menus.Remove(item);
                //role.Menus.Remove(db.Menus.Find(item.ParentId));
            }
            
            db.SaveChanges();
            
            //寫入AccessLog
            MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Delete, "RoleMenu", JsonConvert.SerializeObject(new { role.Id, role.Name, Menus = menus.Select(m => m.Id + m.Text).ToArray() }));

            return Ok();
        }
        #endregion

        #region Membership/UserMenu
        // POST: Membership/UserMenu
        [HttpPost]
        [Route("Membership/UserMenu")]
        public IHttpActionResult PostUserMenu(UserMenuViewModel userMenu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Users.Find(userMenu.UserName);
            if (user == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "不存在的使用者!"));

            var menus = db.Menus.Where(item => item.Id == userMenu.MenuId || item.ParentId == userMenu.MenuId);

            foreach (var item in menus)
            {
                user.Menus.Add(item);
            }

            try
            {
                db.SaveChanges();

                //寫入AccessLog
                MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Create, "UserMenu", JsonConvert.SerializeObject(new { user.UserName, Menus = menus.Select(m => m.Id + m.Text).ToArray() }));

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
            return Ok();
        }

        // DELETE: Membership/UserMenu
        [Route("Membership/UserMenu")]
        [HttpDelete]
        public IHttpActionResult DeleteUserMenu(UserMenuViewModel userMenu)
        {
            var user = db.Users.Find(userMenu.UserName);
            if (user == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "不存在的使用者!"));

            var menus = db.Menus.Where(item => item.Id == userMenu.MenuId || item.ParentId == userMenu.MenuId);

            foreach (var item in menus)
            {
                user.Menus.Remove(item);
            }

            db.SaveChanges();

            //寫入AccessLog
            MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Delete, "UserMenu", JsonConvert.SerializeObject(new { user.UserName, Menus = menus.Select(m => m.Id + m.Text).ToArray() }));


            return Ok();
        }
        #endregion

        #region Membership/Users
        [Route("Membership/Users/UserInfo")]
        public IEnumerable<UserViewModel> GetUsers()
        {
            var users = db.Users.ToArray<User>().Select(u => ToUserViewModel(u));
            return users.AsQueryable();
        }

        [Route("Membership/Users/UserInfo/{userName}", Name = "GetUserByUserName")]
        public UserViewModel GetUser(string userName)
        {
            var user = db.Users.Find(userName);
            if (user==null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "找不到資料")); 
            }
            return ToUserViewModel(user);
        }

        // POST: Membership/Users
        [ResponseType(typeof(UserViewModel))]
        [Route("Membership/Users")]
        [HttpPost]
        public IHttpActionResult PostUser(UserViewModel user_view_model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = db.Roles.Find(user_view_model.RoleId);
            if (role==null)
	        {
		        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "沒有對應的角色!"));
	        }

            User user = db.Users.Find(user_view_model.UserName);
            if (user == null)
            {
                try
                {
                    //新增使用者
                    user = new User { UserName = user_view_model.UserName, Password = user_view_model.UserName, Role = role };
                    db.Users.Add(user);

                    //新增使用者角色的功能選單
                    foreach (var menu in role.Menus)
                    {
                        user.Menus.Add(menu);
                    }

                    //寫入資料庫
                    db.SaveChanges();

                    //寫入AccessLog
                    MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Create, "User",
                                                        JsonConvert.SerializeObject(new { user.UserName, roleName=user.Role.Name }));
                }
                catch (Exception ex)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message)); 
                }
            }
            return CreatedAtRoute("GetUserByUserName", new { userName = user.UserName }, ToUserViewModel(user));
        }

        // PUT: Membership/Users/{userName}
        [ResponseType(typeof(UserViewModel))]
        [Route("Membership/Users/{userName}")]
        [HttpPut]
        public IHttpActionResult PutUser(string userName, UserPasswordViewModel user_view_model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(user_view_model.Password))
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotModified,"取消變更"));

            if (userName != user_view_model.UserName)
                return BadRequest();

            //把資料庫中的那筆資料讀出來
            var user_db = db.Users.Find(userName);
            if (user_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    user_db.Password = user_view_model.Password;
                    //db.Entry(user_db).OriginalValues["Timestamp"] = Convert.FromBase64String(user_view_model.TimestampString);
                    db.SaveChanges();

                    //寫入AccessLog
                    MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.PasswordChanged, "User",user_db.UserName);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userName))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));// ""
                }
            }

            return Ok(ToUserViewModel(user_db));

        }

        // PUT: Membership/Users/UserInfo/{userName}
        [ResponseType(typeof(UserViewModel))]
        [Route("Membership/Users/UserInfo/{userName}")]
        [HttpPut]
        public IHttpActionResult PutUser(string userName, UserViewModel user_view_model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (userName != user_view_model.UserName)
                return BadRequest();

            //把資料庫中的那筆資料讀出來
            var user_db = db.Users.Find(userName);
            if (user_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    //移除舊角色功能選單
                    foreach (var item in user_db.Role.Menus)
                    {
                        user_db.Menus.Remove(item);
                    }

                    //更新新角色
                    user_db.Role_Id = user_view_model.RoleId;
                    db.Entry(user_db).OriginalValues["Timestamp"] = Convert.FromBase64String(user_view_model.TimestampString);
                    db.SaveChanges();

                    //加入新角色功能選單
                    var roleMenus = user_db.Role.Menus;
                    foreach (var item in roleMenus)
                    {
                        user_db.Menus.Add(item);
                    }
                    db.SaveChanges();

                    //寫入AccessLog
                    MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Update, "User", 
                        JsonConvert.SerializeObject(new { user_db.UserName, roleName=user_db.Role.Name }));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userName))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));// ""
                }
                catch (Exception ex)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
                }
            }

            return Ok(ToUserViewModel(user_db));

        }

        // DELETE: Membership/Users/{userName}
        [ResponseType(typeof(UserViewModel))]
        [Route("Membership/Users/{userName}")]
        [HttpDelete]
        public IHttpActionResult DeleteUser(string userName)
        {
            User user = db.Users.Find(userName);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            try
            {
                db.SaveChanges();

                //寫入AccessLog
                MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Delete, "User", userName);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }

            return Ok(new UserViewModel { UserName=userName});
        }

        private UserViewModel ToUserViewModel(User user)
        {
            return new UserViewModel { UserName = user.UserName, 
                                                        RoleId= user.Role_Id.HasValue ? Convert.ToInt32(user.Role_Id) :-1, 
                                                        RoleName = user.Role==null ? "" : user.Role.Name,
                                                        TimestampString = Convert.ToBase64String(user.Timestamp) };
        }

        private bool UserExists(string userName)
        {
            return db.Users.Count(e => e.UserName == userName) > 0;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
