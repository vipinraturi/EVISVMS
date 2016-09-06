using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class UserSessionDataVM
    {
        public string ThemeName { get; set; }
        public string ImageLogoPath { get; set; }
        public string ProfilePicPath { get; set; }
        public string FullName { get; set; }
        public RoleType UserRole { get; set; }
    }

    public enum RoleType
    {
        SuperAdmin,
        SuperVisor,
        Security,
        BuildingAdmin
    }
}