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
                                                        .Select(item=> new VisitorDetailsVM
                                                        {
                                                            Id = item.Id,
                                                            VisitorName= item.FirstName + " " + item.LastName,
                                                            ContactAddress = string.Empty,//TODO
                                                            ContactNo = item.ContactNo,
                                                            DOB = item.DOB??DateTime.MinValue,
                                                            EmailAddress = item.EmailId,
                                                            Gender = item.GenderId,
                                                            IdNo = string.Empty, //TODO
                                                            Nationality = item.Nationality??0,
                                                            TypeOfCard = 0//TODO
                                                        }).AsQueryable();

            return lstVisitors.ToList();
            //var list = new List<VisitorDetailsVM>();
            //list.Add(new VisitorDetailsVM { CompanyId = 1, ContactAddress = "", VisitorName = "Visitor 1", ContactNo = "0562234561", EmailAddress = "company1@test.com", DOB = DateTime.Now, Gender = 1, IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 2, ContactAddress = "", VisitorName = "Visitor 2", ContactNo = "0562234562", EmailAddress = "company2@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 3, ContactAddress = "", VisitorName = "Visitor 3", ContactNo = "0562234563", EmailAddress = "company3@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 4, ContactAddress = "", VisitorName = "Visitor 4", ContactNo = "0562234564", EmailAddress = "company4@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 5, ContactAddress = "", VisitorName = "Visitor 5", ContactNo = "0562234565", EmailAddress = "company5@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 6, ContactAddress = "", VisitorName = "Visitor 6", ContactNo = "0562234566", EmailAddress = "company6@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 7, ContactAddress = "", VisitorName = "Visitor 7", ContactNo = "0562234567", EmailAddress = "company7@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 8, ContactAddress = "", VisitorName = "Visitor 8", ContactNo = "0562234568", EmailAddress = "company8@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 9, ContactAddress = "", VisitorName = "Visitor 9", ContactNo = "0562234569", EmailAddress = "company9@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 10, ContactAddress = "", VisitorName = "Visitor 10", ContactNo = "0562234510", EmailAddress = "company10@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 11, ContactAddress = "", VisitorName = "Visitor 11", ContactNo = "0562234511", EmailAddress = "company11@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 12, ContactAddress = "", VisitorName = "Visitor 12", ContactNo = "0562234512", EmailAddress = "company12@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 13, ContactAddress = "", VisitorName = "Visitor 13", ContactNo = "0562234513", EmailAddress = "company13@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 14, ContactAddress = "", VisitorName = "Visitor 14", ContactNo = "0562234514", EmailAddress = "company14@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 15, ContactAddress = "", VisitorName = "Visitor 15", ContactNo = "0562234515", EmailAddress = "company15@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 16, ContactAddress = "", VisitorName = "Visitor 16", ContactNo = "0562234516", EmailAddress = "company16@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 17, ContactAddress = "", VisitorName = "Visitor 17", ContactNo = "0562234517", EmailAddress = "company17@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //list.Add(new VisitorDetailsVM { CompanyId = 18, ContactAddress = "", VisitorName = "Visitor 18", ContactNo = "0562234518", EmailAddress = "company18@test.com", IdNo = "ABCC", Nationality = 1, TypeOfCard = 1 });
            //return list;
        }

        public bool SaveVisitor(VisitorDetailsVM visitorDetailsVM)
        {
            _genericService
                .VisitorMaster.Insert(new Data.Model.Entities.VisitorMaster {
                    ContactNo = visitorDetailsVM.ContactNo,
                    CreatedBy = null,
                    CreatedDate = DateTime.UtcNow,
                    DOB = visitorDetailsVM.DOB,
                    EmailId = visitorDetailsVM.EmailAddress,
                    FirstName = visitorDetailsVM.VisitorName,
                    LastName = visitorDetailsVM.VisitorName,
                    Nationality = visitorDetailsVM.Nationality,
                    UpdatedBy = null,
                    UpdatedDate = DateTime.Now,
                    GenderId = visitorDetailsVM.Gender
                });
            _genericService.Commit();
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
        
    }
}