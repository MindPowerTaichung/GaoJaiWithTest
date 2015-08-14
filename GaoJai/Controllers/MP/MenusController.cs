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

namespace MPERP2015.Controllers
{
    [Authorize]
    public class MenusController : ApiController
    {
        MembershipModelContainer db = new MembershipModelContainer();

        // GET: api/Menus/Json/Role/1
        [Route("api/Menus/Json/Role/{roleId}")]
        public IEnumerable<MenuTreeViewModel> GetRoleMenuJson(int roleId)
        {
            int[] menusOfRole;
            var role = db.Roles.Find(roleId);
            if (role==null)
	            menusOfRole=new int[0];
            else
                menusOfRole = role.Menus.Select(item => item.Id).ToArray();            

            var items = GetMenus(db.Menus.ToList(), 0, menusOfRole);
            return items;
        }
        // GET: api/Menus/Json/User/apple
        [Route("api/Menus/Json/User/{userName}")]
        public IEnumerable<MenuTreeViewModel> GetUserMenuJson(string userName)
        {
            var user = db.Users.Find(userName);

            //使用者+角色的選單
            //var role = db.Roles.Find(user.Role_Id);
            //var menusOfUser = user.Menus.Union(role.Menus).Select(item => item.Id).ToArray();

            //使用者選單
            var menusOfUser = user.Menus.Select(item => item.Id).ToArray();

            var items = GetMenus(db.Menus.ToList(), 0, menusOfUser);
            return items;
        }
        List<MenuTreeViewModel> GetMenus(List<Menu> list, int parentId, int[] menusChecked)
        {
            var items= list.Where(x => x.ParentId == parentId).Select(x => new MenuTreeViewModel
            {
                Id = x.Id,
                Text = x.Text,
                ParentId = x.ParentId,
                ContentUrl= x.ContentUrl,
                CssClass = x.CssClass,
                Checked = menusChecked.Contains(x.Id),
                SubMenus = GetMenus(list, x.Id, menusChecked),
                TimestampString = Convert.ToBase64String( x.Timestamp)
            }).ToList();

            foreach (var item in items)
            {
                item.HasChildren = item.SubMenus.Count > 0;    
            }
            return items;
        }

        [Authorize]
        [Route("api/Menus/Json/Authorized")]
        public IEnumerable<MenuAuthorizedViewModel> GetAuthorizedMenuJson()
        {
            //var identity = User.Identity as ClaimsIdentity;
            //var userName = identity.Claims.Where(item => item.Type == "sub").Select(item=>item.Value).SingleOrDefault();
            //int roleId;
            //int.TryParse(identity.Claims.Where(item => item.Type == "roleId").Select(item => item.Value).SingleOrDefault(), out roleId);
            var userName = User.Identity.Name;
            
            var user = db.Users.Find(userName);
            //var role = user.Role;
            //var menusOfUser=user.Menus.Union(role.Menus).Select(item => item.Id).ToArray();
            var menusOfUser = user.Menus.Select(item => item.Id).ToArray();
            var items = GetAuthorizedMenus(db.Menus.ToList(), 1, menusOfUser);

            return items;
        }
        List<MenuAuthorizedViewModel> GetAuthorizedMenus(List<Menu> list, int parentId, int[] menusOfUser)
        {
            var items = list.Where(x => x.ParentId == parentId && menusOfUser.Contains(x.Id)).Select(x => new MenuAuthorizedViewModel
            {
                Id = x.Id,
                Text = x.Text,
                ParentId = x.ParentId,
                ContentUrl=x.ContentUrl,
                CssClass = x.CssClass,
                SubMenus = GetAuthorizedMenus(list, x.Id, menusOfUser)
            }).ToList();

            foreach (var item in items)
            {
                item.HasChildren = item.SubMenus.Count > 0;
            }
            return items;
        }

        // GET: api/Menus
        public IEnumerable<MenuViewModel> Get()
        {
            var items = db.Menus.ToArray<Menu>().Select(item => ToMenuViewModel(item));
            return items;
        }
        // GET: api/Menus/1
        [Route("api/Menus/Role/{roleId}")]
        public IEnumerable<MenuViewModel> GetMenusByRole(int roleId)
        {
            return db.Roles.Find(roleId).Menus.ToArray<Menu>().Select(item => ToMenuViewModel(item));
        }

        // GET: api/Menus/5
        public IHttpActionResult Get(int id)
        {
            Menu item = db.Menus.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(ToMenuViewModel(item));
        }

        // POST: api/Menus
        [HttpPost]
        public IHttpActionResult Post(MenuViewModel item_viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Menu item = new Menu { Id = item_viewModel.Id, 
                                                 Text = item_viewModel.Text, 
                                                 ContentUrl=item_viewModel.ContentUrl,
                                                 CssClass=item_viewModel.CssClass,
                                                 ParentId= item_viewModel.ParentId};
            db.Menus.Add(item);
            try
            {
                db.SaveChanges();

                //寫入AccessLog
                MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Create, "Menu", JsonConvert.SerializeObject(new { item.Id, item.Text,item.ContentUrl,item.ParentId }));
            }
            catch (DbEntityValidationException ex)
            {
                var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var getFullMessage = string.Join("; ", entityError);
                var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, exceptionMessage));
            }
            catch (DbUpdateException ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.Message));
            }

            return CreatedAtRoute("DefaultApi", new { id = item.Id }, ToMenuViewModel(item));
        }

        // PUT: api/Menus/5
        [HttpPut]
        public IHttpActionResult Put(int id, MenuViewModel item_viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != item_viewModel.Id)
                return BadRequest();

            //把資料庫中的那筆資料讀出來
            var item_db = db.Menus.Find(id);
            if (item_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    item_db.Text = item_viewModel.Text;
                    item_db.ContentUrl = item_viewModel.ContentUrl;
                    item_db.CssClass = item_viewModel.CssClass;
                    item_db.ParentId = item_viewModel.ParentId;
                    db.Entry(item_db).OriginalValues["Timestamp"] = Convert.FromBase64String(item_viewModel.TimestampString);
                    db.SaveChanges();

                    //寫入AccessLog
                    MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Update, "Menu",
                        JsonConvert.SerializeObject(new { item_db.Id, item_db.Text, item_db.ContentUrl, item_db.ParentId }));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.Menus.Find(id) == null)
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));
                }
            }

            return Ok(ToMenuViewModel(item_db));

        }

        // DELETE: api/Menus/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Menu item_db = db.Menus.Find(id);
            if (item_db == null)
            {
                return NotFound();
            }

            //db.Menus.Remove(item_db);            
            db.Menus.RemoveRange(db.Menus.Where(item=> item.ParentId==id || item.Id==id));
            db.SaveChanges();

            //寫入AccessLog
            MPAccessLog.WriteEntry(User.Identity.Name, AccessAction.Delete, "Menu",
                JsonConvert.SerializeObject(new { item_db.Id, item_db.Text, item_db.ContentUrl, item_db.ParentId }));

            return Ok(new MenuViewModel { Id = id });
        }

        private MenuViewModel ToMenuViewModel(Menu item)
        {
            return new MenuViewModel { Id = item.Id, Text = item.Text, TimestampString = Convert.ToBase64String(item.Timestamp),
                                                      ContentUrl=item.ContentUrl, CssClass=item.CssClass, ParentId=item.ParentId};
        }

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

