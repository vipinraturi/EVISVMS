using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.HelperClasses
{
    public class OrganizationHelper
    {
        public IList<OrganizationVM> GetAllOrganizations()
        {
            var list = new List<OrganizationVM>();
            list.Add(new OrganizationVM { CompanyId=1, Address = "", CompanyName = "Company 1", ContactNo = "0562234561", EmailAddress = "company1@test.com", FaxNo = "0562234566", WebSite = "www.company1.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=2, Address = "", CompanyName = "Company 2", ContactNo = "0562234562", EmailAddress = "company2@test.com", FaxNo = "0562234566", WebSite = "www.company2.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=3, Address = "", CompanyName = "Company 3", ContactNo = "0562234563", EmailAddress = "company3@test.com", FaxNo = "0562234566", WebSite = "www.company3.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=4, Address = "", CompanyName = "Company 4", ContactNo = "0562234564", EmailAddress = "company4@test.com", FaxNo = "0562234566", WebSite = "www.company4.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=5, Address = "", CompanyName = "Company 5", ContactNo = "0562234565", EmailAddress = "company5@test.com", FaxNo = "0562234566", WebSite = "www.company5.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=6, Address = "", CompanyName = "Company 6", ContactNo = "0562234566", EmailAddress = "company6@test.com", FaxNo = "0562234566", WebSite = "www.company6.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=7, Address = "", CompanyName = "Company 7", ContactNo = "0562234567", EmailAddress = "company7@test.com", FaxNo = "0562234566", WebSite = "www.company7.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=8, Address = "", CompanyName = "Company 8", ContactNo = "0562234568", EmailAddress = "company8@test.com", FaxNo = "0562234566", WebSite = "www.company8.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=9, Address = "", CompanyName = "Company 9", ContactNo = "0562234569", EmailAddress = "company9@test.com", FaxNo = "0562234566", WebSite = "www.company9.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=10, Address = "", CompanyName = "Company 10", ContactNo = "0562234510", EmailAddress = "company10@test.com", FaxNo = "0562234566", WebSite = "www.company10.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=11, Address = "", CompanyName = "Company 11", ContactNo = "0562234511", EmailAddress = "company11@test.com", FaxNo = "0562234566", WebSite = "www.company11.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=12, Address = "", CompanyName = "Company 12", ContactNo = "0562234512", EmailAddress = "company12@test.com", FaxNo = "0562234566", WebSite = "www.company12.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=13, Address = "", CompanyName = "Company 13", ContactNo = "0562234513", EmailAddress = "company13@test.com", FaxNo = "0562234566", WebSite = "www.company13.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=14, Address = "", CompanyName = "Company 14", ContactNo = "0562234514", EmailAddress = "company14@test.com", FaxNo = "0562234566", WebSite = "www.company14.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=15, Address = "", CompanyName = "Company 15", ContactNo = "0562234515", EmailAddress = "company15@test.com", FaxNo = "0562234566", WebSite = "www.company15.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=16, Address = "", CompanyName = "Company 16", ContactNo = "0562234516", EmailAddress = "company16@test.com", FaxNo = "0562234566", WebSite = "www.company16.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=17, Address = "", CompanyName = "Company 17", ContactNo = "0562234517", EmailAddress = "company17@test.com", FaxNo = "0562234566", WebSite = "www.company17.com", ZipCode = "123221" });
            list.Add(new OrganizationVM { CompanyId=18, Address = "", CompanyName = "Company 18", ContactNo = "0562234518", EmailAddress = "company18@test.com", FaxNo = "0562234566", WebSite = "www.company18.com", ZipCode = "123221" });
            return list;
        }
    }
}