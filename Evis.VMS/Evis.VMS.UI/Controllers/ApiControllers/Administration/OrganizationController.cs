/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Model.Entities;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using Evis.VMS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        public static IList<OrganizationVM> lstOrganization = new List<OrganizationVM>();

        [Route("~/Api/Administration/SaveOrganization")]
        [HttpPost]
        public ReturnResult SaveOrganization([FromBody] Organization organization)
        {
            string message = string.Empty;
            if (organization.Id == 0)
            {
                organization.IsActive = true;
                _genericService.Organization.Insert(organization);

                var proto = Request.GetRequestContext().Url.Request.RequestUri.Scheme;
                var baseUrl = Request.GetRequestContext().Url.Request.RequestUri.Authority;

                string body = "Dear Sir/Madam, <br/><br/>Your company with the name <b>" + organization.CompanyName + "</b> has been created successfully." +
                                "<br/><br/>Regards,<br/>Administrator";

                // Send email on organization creation.
                //EmailHelper.SendMail(organization.EmailId, "Company Prfile is created", body);
                message = "Organization saved successfully!!";
            }
            else
            {
                var existingOrg = _genericService.Organization.GetById(organization.Id);
                if (existingOrg != null)
                {
                    existingOrg.CompanyName = organization.CompanyName;
                    existingOrg.EmailId = organization.EmailId;
                    existingOrg.ContactNumber = organization.ContactNumber;
                    existingOrg.ContactAddress = organization.ContactAddress;
                    existingOrg.ZipCode = organization.ZipCode;
                    existingOrg.FaxNumber = existingOrg.FaxNumber;
                    existingOrg.WebSite = existingOrg.WebSite;
                    organization.IsActive = true;
                    _genericService.Organization.Update(existingOrg);
                    message = "Organization updated successfully!!";
                }
            }
            _genericService.Commit();
            return new ReturnResult { Message = message, Success = true };
        }

        [Route("~/Api/Administration/GetOrganizationsData")]
        [HttpPost]
        public string GetOrganizationsData(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var organizationsList = _genericService.Organization.GetAll().Where(x => x.IsActive == true).ToList();

            if (organizationsList.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    organizationsList = organizationsList.Where(item =>
                        item.ContactAddress.ToLower().Contains(globalSearch.ToLower()) ||
                        item.CompanyName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.ContactNumber.ToLower().Contains(globalSearch.ToLower()) ||
                        item.EmailId.ToLower().Contains(globalSearch.ToLower()) ||
                        item.FaxNumber.ToLower().Contains(globalSearch.ToLower()) ||
                        item.WebSite.ToLower().Contains(globalSearch.ToLower()) ||
                        item.ZipCode.ToLower().Contains(globalSearch.ToLower())
                        ).ToList();
                }

                bool sortAscending = (sortOrder == "ASC" ? true : false);
                if (!string.IsNullOrEmpty(sortField))
                {
                    if (!sortAscending)
                    {
                        organizationsList = organizationsList
                            .OrderBy(r => r.GetType().GetProperty(sortField).GetValue(r, null))
                            .ToList();
                    }
                    else
                    {
                        organizationsList = organizationsList
                            .OrderByDescending(r => r.GetType().GetProperty(sortField).GetValue(r, null))
                            .ToList();
                    }
                }
            }

            var data = organizationsList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            var jsonData = JsonConvert.SerializeObject(data);
            var total = organizationsList.Count();
            return JsonConvert.SerializeObject(new { totalRows = total, result = jsonData });
        }

        [Route("~/Api/Administration/DeleteOrganization")]
        [HttpPost]
        public async Task<ReturnResult> DeleteOrganization([FromBody] Organization organization)
        {
            if (organization != null)
            {
                var orgToDelete = _genericService.Organization.GetAll().Where(x => x.Id == organization.Id).FirstOrDefault();
                if (orgToDelete != null)
                {
                    if ((_genericService.BuildingMaster.SearchFor(x => x.OrganizationId == organization.Id).Any()) ||
                        (await _userService.GetManyAsync(x => x.OrganizationId != null && x.OrganizationId == organization.Id)).Any())
                    {
                        return new ReturnResult { Message = "Please first delete all the users, buildings, and gates under this organization", Success = false };
                    }
                    //orgToDelete.IsActive = false;
                    _genericService.Organization.Delete(orgToDelete);
                    _genericService.Commit();
                    return new ReturnResult { Message = "Organization deleted successfully", Success = true };
                }
            }
            return new ReturnResult { Message = "Error in deleting the organization", Success = false };
        }

        [Route("~/Api/Administration/GetOrganization")]
        [HttpGet]
        public Organization GetOrganization(int organizationId)
        {
            if (organizationId != 0)
            {
                return _genericService.Organization.GetById(organizationId);
            }
            return null;
        }


        [Route("~/Api/Administration/GetCountries")]
        [HttpGet]
        public IQueryable<GeneralDropDownVM> GetCountries()
        {
            var result = _genericService.LookUpValues.GetAll().Where(x => x.LookUpType.TypeName == "Country" && x.IsActive == true && x.LookUpType.IsActive == true)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.LookUpValue });
            return result;
        }

        [Route("~/Api/Administration/GetStatesOrCities")]
        [HttpGet]
        public IQueryable<GeneralDropDownVM> GetStatesOrCities(int id)
        {
            var result = _genericService.LookUpValues.GetAll().Where(x => x.ParentId == id && x.IsActive == true && x.LookUpType.IsActive == true)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.LookUpValue });
            return result;
        }
    }

}
