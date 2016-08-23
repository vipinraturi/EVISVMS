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
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        public static IList<OrganizationVM> lstOrganization = new List<OrganizationVM>();

        [Route("~/Api/Administration/SaveOrganization")]
        [HttpPost]
        public ReturnResult SaveOrganization([FromBody] Organization organization)
        {
            string message = "Error occured, please try again with valid entered data again!";
            string currentUserId = HttpContext.Current.User.Identity.GetUserId();
            bool success = false;
            if (organization.Id == 0)
            {
                var existOrganization = _genericService.Organization.GetAll().Where(x => x.CompanyName.ToLower().Equals(organization.CompanyName.ToLower()));
                if (existOrganization != null && existOrganization.Count() > 0)
                {
                    message = "Organization with this name is already exist! Please some other use other name.";
                    return new ReturnResult { Message = message, Success = success };
                }
                organization.IsActive = true;
                organization.CreatedBy = currentUserId;
                organization.CreatedOn = DateTime.UtcNow;
                organization.UpdatedBy = currentUserId;
                organization.UpdatedOn = DateTime.UtcNow;
                _genericService.Organization.Insert(organization);

                var proto = Request.GetRequestContext().Url.Request.RequestUri.Scheme;
                var baseUrl = Request.GetRequestContext().Url.Request.RequestUri.Authority;

                var emailFormat = _genericService.EmailFormats.GetAll().Where(x => x.Category == "OrganizationCreation").FirstOrDefault();
                string body = string.Format(emailFormat.Format, organization.CompanyName);

                // Send email on organization creation.
                //EmailHelper.SendMail(organization.EmailId, "Company Prfile is created", body);
                message = "Organization saved successfully!!";
                success = true;
            }
            else
            {
                var existingOrg = _genericService.Organization.GetById(organization.Id);
                if (existingOrg != null)
                {
                    existingOrg.CompanyName = organization.CompanyName;
                    existingOrg.CountryId = organization.CountryId;
                    existingOrg.WebSite = organization.WebSite;
                    existingOrg.UpdatedBy = currentUserId;
                    existingOrg.UpdatedOn = DateTime.UtcNow;
                    organization.IsActive = true;
                    _genericService.Organization.Update(existingOrg);
                    message = "Organization updated successfully!!";
                    success = true;
                }
            }
            _genericService.Commit();
            return new ReturnResult { Message = message, Success = true };
        }

        [Route("~/Api/Administration/GetOrganizationsData")]
        [HttpPost]
        public string GetOrganizationsData(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var organizationsList = _genericService.Organization.GetAll().Where(x => x.IsActive == true)
            .Select(x => new OrganisationVM
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                CountryId = x.CountryId,
                Country = x.CountryMaster.LookUpValue,
                WebSite = x.WebSite
            });

            if (organizationsList.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    organizationsList = organizationsList.Where(item =>
                        item.CompanyName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.Country.ToLower().Contains(globalSearch.ToLower()) ||
                        item.WebSite.ToLower().Contains(globalSearch.ToLower())
                        ).AsQueryable();
                }

                //creating pager object to send for filtering and sorting
                var paginationRequest = new PaginationRequest
                {
                    PageIndex = (pageIndex - 1),
                    PageSize = pageSize,
                    SearchText = globalSearch,
                    Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
                };

                int totalCount = 0;
                IList<OrganisationVM> result =
                    GenericSorterPager.GetSortedPagedList<OrganisationVM>(organizationsList, paginationRequest, out totalCount);

                var jsonData = JsonConvert.SerializeObject(result.OrderByDescending(x => x.Id));
                return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
            }

            return null;
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

        [Route("~/Api/Administration/GetTheme")]
        [HttpGet]
        public IQueryable<GeneralDropDownVM> GetTheme()
        {
            var result = _genericService.LookUpValues.GetAll().Where(x => x.LookUpType.TypeName == "Theme" && x.IsActive == true && x.LookUpType.IsActive == true)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.LookUpValue });
            return result;
        }
    }

}
