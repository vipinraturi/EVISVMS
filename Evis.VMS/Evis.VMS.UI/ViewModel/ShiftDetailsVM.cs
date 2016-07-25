/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class ShiftDetailsVM
    {
        public int Id { get; set; }
        public string ShitfName { get; set; }


        public DateTime FromTime { get; set; }


        public DateTime ToTime { get; set; }


        public string strFromTime { get; set; }


        public string strToTime { get; set; }
    }
}