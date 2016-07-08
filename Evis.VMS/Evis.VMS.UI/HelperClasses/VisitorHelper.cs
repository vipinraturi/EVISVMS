/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.HelperClasses
{
    public class VisitorHelper
    {
        GenericService _genericService = null;
        public VisitorHelper()
        {
            _genericService = new GenericService();
        }

        public IList<VisitorDetailsVM> GetAllVisitorsData(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {

            var lstVisitors = _genericService.VisitorMaster.GetAll()
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
                                                            TypeOfCard = item.TypeOfCardId
                                                        }).AsQueryable();

            return lstVisitors.ToList();
        }

        public bool SaveVisitor(VisitorDetailsVM visitorDetailsVM)
        {
            if (visitorDetailsVM.IsInsert)
            {
                _genericService
                    .VisitorMaster.Insert(new Data.Model.Entities.VisitorMaster
                    {
                        ContactNo = visitorDetailsVM.ContactNo,
                        CreatedBy = null,
                        CreatedDate = DateTime.UtcNow,
                        DOB = visitorDetailsVM.DOB,
                        EmailId = visitorDetailsVM.EmailAddress,
                        VisitorName = visitorDetailsVM.VisitorName,
                        Nationality = visitorDetailsVM.Nationality,
                        UpdatedBy = null,
                        UpdatedDate = DateTime.Now,
                        GenderId = visitorDetailsVM.Gender,
                        TypeOfCardId = visitorDetailsVM.TypeOfCard,
                        IdNo = visitorDetailsVM.IdNo,
                        Address = visitorDetailsVM.ContactAddress
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


        public bool IsVisitorExist(VisitorDetailsVM visitorDetailsVM)
        {
            var visitor = _genericService.VisitorMaster.GetAll().Where(item => item.EmailId == visitorDetailsVM.EmailAddress).FirstOrDefault();

            return visitor == null ? false : true;
        }
    }
}