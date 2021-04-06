using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BIIT_HR.Models;
//hello
namespace BIIT_HR.Controllers
{
    public class CriteriaController : ApiController
    {
        BIIT_HREntities6 db = new BIIT_HREntities6();

        [HttpGet]

        public HttpResponseMessage getCriteria(int cri_id)
        {
            var criteriaV = db.Criteria.Where(e => e.cri_id == cri_id);
            return Request.CreateResponse(HttpStatusCode.OK, criteriaV.ToList());
        }


        [HttpPost]
        public HttpResponseMessage CriteriaPut(Criterion fo)
        {
            try
            {
                var entity = db.Criteria.FirstOrDefault(f => f.cri_id == fo.cri_id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, fo.cri_id.ToString() + "not found");
                }
                else
                {
                    entity.cri_total_leave = fo.cri_total_leave;
                    entity.cri_sick_leave = fo.cri_sick_leave;
                    entity.cri_earned_leave = fo.cri_earned_leave;
                    entity.cri_casual_leave = fo.cri_casual_leave;
                    entity.cri_short_leave = fo.cri_short_leave / 2;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }
            catch (Exception ex) { return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex); }
        }


        [HttpGet]
        public HttpResponseMessage getHoliday(int h_id)
        {
            var criteriaH = db.Holidays.Where(e => e.h_id == h_id);
            return Request.CreateResponse(HttpStatusCode.OK, criteriaH.ToList());
        }

        [HttpPost]
        public HttpResponseMessage PostDates(Leave leave)
        {
            var leaves = db.Leaves.Add(leave);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, leave.leave_id);
        }

        [HttpPost]
        public HttpResponseMessage HolidayCri(Holiday ho)
        {

            try
            {
                var entity = db.Holidays.FirstOrDefault(f => f.h_id == ho.h_id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ho.h_id.ToString() + "not found");
                }
                else
                {
                    entity.hajj = ho.hajj;
                    entity.EidF = ho.EidF;
                    entity.EidAdha = ho.EidAdha;

                    entity.hajj_from = ho.hajj_from;
                    entity.hajj_to = ho.hajj_to;

                    entity.eid_from = ho.eid_from;
                    entity.eid_to = ho.eid_to;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }
            catch (Exception ex) { return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex); }
        }





  

       
        [HttpGet]
        public HttpResponseMessage HajjCal(int h_id)
        {
            Holiday employeeS = db.Holidays.FirstOrDefault(e => e.h_id == h_id);
           
         
                DateTime toDate = DateTime.Parse(employeeS.hajj_to.ToString());
                DateTime fromDate = DateTime.Parse(employeeS.hajj_from.ToString());
                int days = int.Parse(toDate.Subtract(fromDate).TotalDays.ToString());

                employeeS.hajj = days ;
                
                db.SaveChanges();
            

            return Request.CreateResponse(HttpStatusCode.OK, employeeS);
        }

        [HttpGet]
        public HttpResponseMessage EidCal(int h_id)
        {
            Holiday eid = db.Holidays.FirstOrDefault(e => e.h_id == h_id);

                DateTime toDate = DateTime.Parse(eid.eid_to.ToString());
                DateTime fromDate = DateTime.Parse(eid.eid_from.ToString());
                int days = int.Parse(toDate.Subtract(fromDate).TotalDays.ToString());


                eid.EidF = days;

                db.SaveChanges();
            

            return Request.CreateResponse(HttpStatusCode.OK, eid);
        }

        [HttpGet]
        public HttpResponseMessage EidAdCal(int h_id)
        {
            Holiday employeeS = db.Holidays.FirstOrDefault(e => e.h_id == h_id);

     
                DateTime toDate = DateTime.Parse(employeeS.eidAd_to.ToString());
                DateTime fromDate = DateTime.Parse(employeeS.eidAd_from.ToString());
                int days = int.Parse(toDate.Subtract(fromDate).TotalDays.ToString());


                employeeS.EidAdha = days;

                db.SaveChanges();
            

            return Request.CreateResponse(HttpStatusCode.OK, employeeS);
        }






    }
}
