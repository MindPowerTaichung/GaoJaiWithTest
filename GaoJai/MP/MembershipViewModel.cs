using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPERP2015.MP
{
    public class UserPasswordViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TimestampString { get; set; }
    }
    public class UserViewModel
    {
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string TimestampString { get; set; }
    }

    public class RoleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TimestampString { get; set; }
    }

    public class MenuTreeViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ContentUrl { get; set; }
        public int ParentId { get; set; }
        public string CssClass { get; set; }
        public string TimestampString { get; set; }
        public bool Checked { get; set; }
        public bool HasChildren { get; set; }
        public List<MenuTreeViewModel> SubMenus { get; set; }
    }
    public class MenuAuthorizedViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ParentId { get; set; }
        public string ContentUrl { get; set; }
        public string CssClass { get; set; }
        public bool HasChildren { get; set; }
        public List<MenuAuthorizedViewModel> SubMenus { get; set; }
    }

    public class MenuViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ContentUrl { get; set; }
        public int ParentId { get; set; }
        public string CssClass { get; set; }
        public string TimestampString { get; set; }        
    }

    public class RoleMenuViewModel
    {
        public int RoleId { get; set; }
        public int MenuId { get; set; }
    }

    public class UserMenuViewModel
    {
        public string UserName { get; set; }
        public int MenuId { get; set; }
    }
}