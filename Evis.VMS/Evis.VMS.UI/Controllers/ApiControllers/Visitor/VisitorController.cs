/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/


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
    public partial class VisitorController : BaseApiController
    {
        public static IList<VisitorDetailsVM> lstVisitorDetails = new List<VisitorDetailsVM>();
       

        public readonly VisitorHelper _visitorHelper = null;

        public VisitorController()
        {
            _visitorHelper = new VisitorHelper();
        }

        [Route("~/Api/Visitor/SaveVisitor")]
        [HttpPost]
        public ReturnResult SaveVisitor([FromBody] VisitorDetailsVM visitorDetailsVM)
        {
            if (visitorDetailsVM.IsInsert)
            {
                var companyId = 1;
                if (lstVisitorDetails.Count() > 0)
                {
                    companyId = lstVisitorDetails.Max(item => item.CompanyId);
                }
                lstVisitorDetails.Add(new VisitorDetailsVM
                {
                    CompanyId = companyId,
                    ContactAddress = visitorDetailsVM.ContactAddress,
                    ContactNo = visitorDetailsVM.ContactNo,
                    DOB = visitorDetailsVM.DOB,
                    EmailAddress = visitorDetailsVM.EmailAddress,
                    Gender = visitorDetailsVM.Gender,
                    IdNo = visitorDetailsVM.IdNo,
                    IsInsert = visitorDetailsVM.IsInsert,
                    Nationality = visitorDetailsVM.Nationality,
                    TypeOfCard = visitorDetailsVM.TypeOfCard,
                    VisitorName = visitorDetailsVM.VisitorName
                });
            }
            else
            {
                var lstVisitorNew = new List<VisitorDetailsVM>();
                foreach (var item in lstVisitorDetails)
                {
                    var visitorToAdd = new VisitorDetailsVM();
                    if (item.CompanyId == visitorDetailsVM.CompanyId)
                    {
                        visitorToAdd.CompanyId = visitorDetailsVM.CompanyId;
                        visitorToAdd.ContactAddress = visitorDetailsVM.ContactAddress;
                        visitorToAdd.ContactNo = visitorDetailsVM.ContactNo;
                        visitorToAdd.DOB = visitorDetailsVM.DOB;
                        visitorToAdd.EmailAddress = visitorDetailsVM.EmailAddress;
                        visitorToAdd.Gender = visitorDetailsVM.Gender;
                        visitorToAdd.IdNo = visitorDetailsVM.IdNo;
                        visitorToAdd.IsInsert = visitorDetailsVM.IsInsert;
                        visitorToAdd.Nationality = visitorDetailsVM.Nationality;
                        visitorToAdd.TypeOfCard = visitorDetailsVM.TypeOfCard;
                        visitorToAdd.VisitorName = visitorDetailsVM.VisitorName;
                    }
                    else
                    {
                        visitorToAdd = item;
                    }
                    lstVisitorNew.Add(visitorToAdd);
                }
                lstVisitorDetails.Clear();
                lstVisitorDetails = lstVisitorNew;
            }

            return new ReturnResult { Message = "Success", Success = true };
        }

        [Route("~/Api/Visitor/GetVisitorData")]
        [HttpPost]
        public string GetVisitorData(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            IList<VisitorDetailsVM> lstVisitornNew = lstVisitorDetails;

            if (lstVisitornNew.Count == 0)
            {
                lstVisitornNew = _visitorHelper.GetAllVisitorsData();
            }

            if (lstVisitornNew.Count > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    lstVisitornNew = lstVisitornNew.Where(item =>
                       item.ContactAddress.ToLower().Contains(globalSearch.ToLower()) ||
                       item.DOB.ToShortDateString().Contains(globalSearch.ToLower()) ||
                       item.ContactNo.ToLower().Contains(globalSearch.ToLower()) ||
                       item.EmailAddress.ToLower().Contains(globalSearch.ToLower()) ||
                       item.IdNo.ToLower().Contains(globalSearch.ToLower()) ||
                       item.VisitorName.ToLower().Contains(globalSearch.ToLower()) 
                       ).ToList();
                }

                bool sortAscending = (sortOrder == "ASC" ? true : false);
                if (!string.IsNullOrEmpty(sortField))
                {
                    if (!sortAscending)
                    {
                        lstVisitornNew = lstVisitornNew
                               .OrderBy(r => r.GetType().GetProperty(sortField).GetValue(r, null))
                               .ToList();
                    }
                    else
                    {
                        lstVisitornNew = lstVisitornNew
                               .OrderByDescending(r => r.GetType().GetProperty(sortField).GetValue(r, null))
                               .ToList();
                    }
                }
            }

            var data = lstVisitornNew.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            var jsonData = JsonConvert.SerializeObject(data);
            var total = lstVisitornNew.Count();
            return JsonConvert.SerializeObject(new { totalRows = total, result = jsonData });
        }

        [Route("~/Api/Visitor/DeleteVisitor")]
        [HttpPost]
        public ReturnResult DeleteDeleteVisitor([FromBody] VisitorDetailsVM visitorVM)
        {
            if (lstVisitorDetails.Count > 0)
            {
                var itemToDelete = lstVisitorDetails.Where(item => item.VisitorName == visitorVM.VisitorName).FirstOrDefault();

                if (itemToDelete != null)
                    lstVisitorDetails.Remove(itemToDelete);
            }
            return new ReturnResult { Message = "Success", Success = true };
        }
    }
}
