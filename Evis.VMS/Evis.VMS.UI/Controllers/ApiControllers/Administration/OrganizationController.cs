﻿/********************************************************************************
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
            if (organization.Id == 0)
            {
                _genericService.Organization.Insert(organization);
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
                    _genericService.Organization.Update(existingOrg);
                }
            }
            _genericService.Commit();
            return new ReturnResult { Message = "Success", Success = true };
        }

        [Route("~/Api/Administration/GetOrganizationsData")]
        [HttpPost]
        public string GetOrganizationsData(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var organizationsList = _genericService.Organization.GetAll().ToList();

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
        public ReturnResult DeleteOrganization([FromBody] Organization organization)
        {
            if (organization != null)
            {
                _genericService.Organization.Delete(organization);
                _genericService.Commit();
                return new ReturnResult { Message = "Success", Success = true };
            }
            return new ReturnResult { Message = "Failure", Success = false };
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
    }
}
