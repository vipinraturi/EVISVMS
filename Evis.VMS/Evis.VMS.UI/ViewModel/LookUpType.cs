/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Vipin Raturi
 * Created On   : 03/07/2016
 * Description  : 
 *******************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class LookUpType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string TypeCode { get; set; }
        public string Description { get; set; }
    }

    public class LookUpTypeValues
    {
        public int Id { get; set; }
        public int LookUpTypeId { get; set; }
        public int? ParentId { get; set; }
        public string LookUpValue { get; set; }
        public string Description { get; set; }
        public virtual LookUpType LookUpType { get; set; }
    }
}