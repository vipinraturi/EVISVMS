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
    public class GatesVM
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public int CityId { get; set; }
        public string GateNumber { get; set; }
        public string BuildingName { get; set; }

        public string CityName { get; set; }
    }
}