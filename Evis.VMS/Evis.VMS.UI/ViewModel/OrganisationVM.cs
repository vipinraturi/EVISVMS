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

        public int? StateId { get; set; }

        public int? CityId { get; set; }

        public string EmailId { get; set; }

        public string ContactNumber { get; set; }

        public string ContactAddress { get; set; }

        public string ZipCode { get; set; }

        public string FaxNumber { get; set; }

        public string Website { get; set; }
                
    }
}