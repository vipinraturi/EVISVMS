    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class OrganisationVM
    {

        public int Id { get; set; }

        public string CompanyName { get; set; }

        public int? CountryId { get; set; }

        public string Country { get; set; }

        public string CreatedOn { get; set; }

        public string WebSite { get; set; }
                
    }
}