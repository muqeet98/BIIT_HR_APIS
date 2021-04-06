using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BIIT_HR.Models;
namespace BIIT_HR.Controllers
{
    public class AdminController : ApiController
    {
        BIIT_HREntities6 db = new BIIT_HREntities6();

        [HttpGet, HttpPost]
        public HttpResponseMessage LoginAdmin(string admin_name, string ad_password)
        {
            try
            {
                //var empdetail = db.Employees.FirstOrDefault(e => e.admin_name == admin_name && e.password == password);
                var admindetail = db.Admins.FirstOrDefault(a => a.admin_name == admin_name && a.ad_password == ad_password);
                if (admindetail != null)
                    return Request.CreateResponse(HttpStatusCode.OK, admindetail);
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, admindetail);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
