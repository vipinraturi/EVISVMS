using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class ResetPasswordVM
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}