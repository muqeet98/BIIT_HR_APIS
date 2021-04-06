using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BIIT_HR.Models;
namespace BIIT_HR.Controllers
{
    public class EmployeeLeaveController : ApiController
    {
        BIIT_HREntities6 db = new BIIT_HREntities6();
        [HttpGet]
        public HttpResponseMessage OneEmpLeave(int emp_id)
        {
            var employeesLeave = db.EmployeeLeaves.Where(e => e.emp_id == emp_id);
            return Request.CreateResponse(HttpStatusCode.OK, employeesLeave.ToList());
        }
    }
}
