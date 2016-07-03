/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class CommonController : BaseApiController
    {
        private readonly GenericService _genericService = null;

        public CommonController()
        {
            _genericService = new GenericService();
        }

        [Route("~/Api/Common/GetLookUpData")]
        [HttpPost]
        public IList<LookUpTypeValues> GetLookUpData(string[] lookUpTypes)
        {
            var query = (from lookupType in _genericService.LookUpType.GetAll()
                         join lookupValues in _genericService.LookUpValues.GetAll() on lookupType.Id equals lookupValues.LookUpTypeId
                         where lookUpTypes.Contains(lookupType.TypeName)
                         select new LookUpTypeValues
                         {
                             Id = lookupValues.Id,
                             Description = lookupValues.Description,
                             LookUpType = new LookUpType
                                                {
                                                    Description = lookupType.Description,
                                                    Id = lookupType.Id,
                                                    TypeCode = lookupType.TypeCode,
                                                    TypeName = lookupType.TypeName
                                                },
                             LookUpTypeId = lookupValues.LookUpTypeId,
                             LookUpValue = lookupValues.LookUpValue
                             //,ParentId = lookupValues.ParentId
                         });
            return query.ToList();
        }

    }
}
