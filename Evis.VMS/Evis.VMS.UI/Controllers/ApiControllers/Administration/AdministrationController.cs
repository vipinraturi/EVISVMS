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
    public partial class AdministrationController : BaseApiController
    {
        public readonly OrganizationHelper _organizationHelper = null;
        private readonly UserService _userService = null;
        private readonly GenericService _genericService = null;
        private readonly ApplicationRoleService _applicationRoleService = null;

        public AdministrationController()
        {
            _organizationHelper = new OrganizationHelper();
            _userService = new UserService();
            _genericService = new GenericService();
            _applicationRoleService = new ApplicationRoleService();
        }

    }
}
