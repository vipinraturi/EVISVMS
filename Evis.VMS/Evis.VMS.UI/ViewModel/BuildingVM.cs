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
    public class BuildingVM
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }


        public string BuildingName { get; set; }


        public int CityId { get; set; }


        public string Address { get; set; }

        public int? NationalityId { get; set; }

        public string OrganizationName { get; set; }
        public string ZipCode { get; set; }
        public int? StateId { get; set; }
        //public int CityId { get; set; }
    }
}