using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class ResetPasswordVM
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(14, ErrorMessage = "Password is having less than 8 characters!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}