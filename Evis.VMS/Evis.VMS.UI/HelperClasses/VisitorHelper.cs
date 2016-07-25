/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.ViewModel;
using Evis.VMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Evis.VMS.UI.HelperClasses
{
    public class VisitorHelper
    {
        GenericService _genericService = null;
        public VisitorHelper()
        {
            _genericService = new GenericService();
        }

        public IList<VisitorDetailsVM> GetAllVisitorsData(string globalSearch, int pageIndex, int pageSize, string sortField, string sortOrder, out int totalCount, int? organizationId)
        {

            var qryVisitors = _genericService.VisitorMaster.GetAll()
                                                        .Where(item => (organizationId == null || (item.ApplicationUser.OrganizationId != null && item.ApplicationUser.OrganizationId == organizationId)) &&
                                                            (item.Address.Contains(globalSearch) ||
                                                            item.ContactNo.Contains(globalSearch) ||
                                                            item.EmailId.Contains(globalSearch) ||
                                                            item.IdNo.Contains(globalSearch) ||
                                                            item.VisitorName.Contains(globalSearch))
                                                        )
                                                        .Select(item => new VisitorDetailsVM
                                                        {
                                                            Id = item.Id,
                                                            VisitorName = item.VisitorName,
                                                            ContactAddress = item.Address,
                                                            ContactNo = item.ContactNo,
                                                            DOB = item.DOB ?? DateTime.MinValue,
                                                            EmailAddress = item.EmailId,
                                                            Gender = item.GenderId,
                                                            IdNo = item.IdNo,
                                                            Nationality = item.Nationality ?? 0,
                                                            TypeOfCard = item.TypeOfCardId,
                                                            ImagePath = item.ImagePath
                                                        })
                                                        .AsQueryable();


            //creating pager object to send for filtering and sorting
            var paginationRequest = new PaginationRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchText = globalSearch,
                Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
            };


            IList<VisitorDetailsVM> result =
                GenericSorterPager.GetSortedPagedList<VisitorDetailsVM>(qryVisitors, paginationRequest, out totalCount);

            return result;
        }

        public bool SaveVisitor(VisitorDetailsVM visitorDetailsVM)
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            if (visitorDetailsVM.IsInsert)
            {
                _genericService
                    .VisitorMaster.Insert(new Data.Model.Entities.VisitorMaster
                    {
                        ContactNo = visitorDetailsVM.ContactNo,
                        CreatedBy = userId,
                        CreatedDate = DateTime.UtcNow,
                        DOB = visitorDetailsVM.DOB,
                        EmailId = visitorDetailsVM.EmailAddress,
                        VisitorName = visitorDetailsVM.VisitorName,
                        Nationality = visitorDetailsVM.Nationality,
                        UpdatedBy = userId,
                        UpdatedDate = DateTime.Now,
                        GenderId = visitorDetailsVM.Gender,
                        TypeOfCardId = visitorDetailsVM.TypeOfCard,
                        IdNo = visitorDetailsVM.IdNo,
                        Address = visitorDetailsVM.ContactAddress,
                        ImagePath = visitorDetailsVM.ImagePath
                    });
                _genericService.Commit();
            }
            else
            {
                var visitor =
                    _genericService.VisitorMaster.GetAll()
                    .Where(item => item.EmailId == visitorDetailsVM.EmailAddress)
                    .FirstOrDefault();

                if (visitor != null)
                {
                    visitor.ContactNo = visitorDetailsVM.ContactNo;
                    visitor.DOB = visitorDetailsVM.DOB;
                    visitor.EmailId = visitorDetailsVM.EmailAddress;
                    visitor.VisitorName = visitorDetailsVM.VisitorName;
                    visitor.Nationality = visitorDetailsVM.Nationality;
                    visitor.GenderId = visitorDetailsVM.Gender;
                    visitor.TypeOfCardId = visitorDetailsVM.TypeOfCard;
                    visitor.IdNo = visitorDetailsVM.IdNo;
                    visitor.Address = visitorDetailsVM.ContactAddress;
                    visitor.UpdatedBy = userId;
                    visitor.UpdatedDate = DateTime.Now;
                    _genericService.VisitorMaster.Update(visitor);
                    _genericService.Commit();
                }
            }

            return true;
        }

        public bool DeleteVisitor(VisitorDetailsVM visitorDetailsVM)
        {
            var visitorFromDb = _genericService
                .VisitorMaster.GetAll().Where(item => item.Id == visitorDetailsVM.Id).FirstOrDefault();

            if (visitorFromDb != null)
            {
                _genericService.VisitorMaster.Delete(visitorFromDb);
                _genericService.Commit();
                return true;
            }

            return false;
        }


        public string IsVisitorEmailExist(VisitorDetailsVM visitorDetailsVM)
        {
            var visitors =
                _genericService.VisitorMaster.GetAll();


            if (visitors.Where(item => item.EmailId == visitorDetailsVM.EmailAddress).FirstOrDefault() != null)
            {
                return "Email already exist";
            }
            else if (visitors.Where(item => item.EmailId == visitorDetailsVM.ContactNo).FirstOrDefault() != null)
            {
                return "Contact Number already exist";
            }
            else if (visitors.Where(item => item.EmailId == visitorDetailsVM.IdNo).FirstOrDefault() != null)
            {
                return "Identity provided already exist";
            }
            return string.Empty;
        }

    }
}