//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using BIIT_HR.Models;

//namespace BIIT_HR.Controllers
//{
//    public class LeavesController : ApiController
//    {
//        BIIT_HREntities4 db = new BIIT_HREntities4();

//        [HttpGet]
//        public HttpResponseMessage AllLeaves()
//        {
        
//            List<Employee> employee = db.Employees.Where(x => x.emp_id != 17 ).ToList();
//            List<Leave> leave = db.Leaves.Where(l => l.leave_status != "Accepted").ToList();


//            var result = from lv in leave
//                         join emp in employee on lv.emp_id equals emp.emp_id
//                         select new { leaveId = lv.leave_id, EmpID = emp.emp_id,
//                             Type = lv.leave_type, EmpName = emp.name, From = lv.date_from,
//                             To = lv.date_to, desig=emp.designation, pic= emp.picture, des= lv.leave_description
                              
                         
//                         };
//            return Request.CreateResponse(HttpStatusCode.OK, result);
//        }
      

//        [HttpGet]
//        public HttpResponseMessage OneLeave(int emp_id)
//        {
//            //var employees = db.Employees.Where(x => x.emp_id != 17).ToList();
//            var employees = db.Leaves.Where(e => e.emp_id == emp_id);
//            return Request.CreateResponse(HttpStatusCode.OK, employees.ToList());
//        }



//        [HttpPost]
//        public HttpResponseMessage PostLeave(Leave leave)
//        {
//            var leaves = db.Leaves.Add(leave);
//            db.SaveChanges();
//            return Request.CreateResponse(HttpStatusCode.OK, leave.leave_id);
//        }
//        [HttpPost]
//        public HttpResponseMessage RejectLeave(Leave leave)
//        {
//            Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
//            lv.leave_status = "Rejected";
//            db.SaveChanges();
//            return Request.CreateResponse(HttpStatusCode.OK, leave.leave_id);
//        }
//        [HttpPost]
//        public HttpResponseMessage AcceptLeave(Leave leave)
//        {
//            EmployeeLeave empleave = db.EmployeeLeaves.FirstOrDefault(el => el.emp_id == leave.emp_id);
//            DateTime toDate = DateTime.Parse(leave.date_to.ToString());
//            DateTime fromDate = DateTime.Parse(leave.date_from.ToString());
//            int days = int.Parse(toDate.Subtract(fromDate).TotalDays.ToString());
          
//            if(empleave.remaining_leaves > 0 && days < empleave.remaining_leaves)
//            {
//                if(leave.leave_type == "Sick")
//                {
//                    if(days > empleave.sick_leave)
//                    {
//                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
//                        lv.leave_status = "Rejected";
//                        db.SaveChanges();
//                    }
//                    else
//                    {
//                        empleave.sick_leave = empleave.sick_leave - days;
//                        empleave.remaining_leaves = empleave.remaining_leaves - days;
//                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
//                        lv.leave_status = "Accepted";
//                        db.SaveChanges();
//                    }
//                }else if(leave.leave_type == "casual")
//                {
//                    if (days > empleave.casual_leave)
//                    {
//                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
//                        lv.leave_status = "Rejected";
//                        db.SaveChanges();
//                    }
//                    else
//                    {
//                        empleave.casual_leave = empleave.casual_leave - days;
//                        empleave.remaining_leaves = empleave.remaining_leaves - days;
//                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
//                        lv.leave_status = "Accepted";
//                        db.SaveChanges();
//                    }
//                }
//                else if(leave.leave_type == "earned")
//                {
//                    if (days > empleave.earned_leave)
//                    {
//                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
//                        lv.leave_status = "Rejected";
//                        db.SaveChanges();
//                    }
//                    else
//                    {
//                        empleave.earned_leave = empleave.earned_leave - days;
//                        empleave.remaining_leaves = empleave.remaining_leaves - days;
//                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
//                        lv.leave_status = "Accepted";
//                        db.SaveChanges();
//                    }
//                }
//                //else if (leave.leave_type == "Sick")
//                //{
//                //    if (days > empleave.sick_leave)
//                //    {
//                //        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
//                //        lv.leave_status = "Rejected";
//                //        db.SaveChanges();
//                //    }
//                //    else
//                //    {
//                //        empleave.sick_leave = empleave.sick_leave - days;
//                //        empleave.remaining_leaves = empleave.remaining_leaves - days;
//                //        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
//                //        lv.leave_status = "Accepted";
//                //        db.SaveChanges();
//                //    }
//                //}
//                else
//                {                   
                        
//                }
//            }
//            else
//            {
//                Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
//                lv.leave_status = "Rejected";
//                db.SaveChanges();
//            }

//            return Request.CreateResponse(HttpStatusCode.OK, "OK");
//        }



    
//    }


//}







 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BIIT_HR.Models;

namespace BIIT_HR.Controllers
{
    public class LeavesController : ApiController
    {
        BIIT_HREntities6 db = new BIIT_HREntities6();

        [HttpGet]
        public HttpResponseMessage AllLeaves()
        {
        
            List<Employee> employee = db.Employees.Where(x => x.emp_id != 17 ).ToList();
            List<Leave> leave = db.Leaves.Where(l => l.leave_status != "Accepted" && l.leave_status != "Rejected").ToList();


            var result = from lv in leave
                         join emp in employee on lv.emp_id equals emp.emp_id
                         select new { leaveId = lv.leave_id, EmpID = emp.emp_id,
                             Type = lv.leave_type, EmpName = emp.name, From = lv.date_from,
                             To = lv.date_to, desig=emp.designation, pic= emp.picture, des= lv.leave_description
                              
                         
                         };
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
      

        [HttpGet]
        public HttpResponseMessage OneLeave(int emp_id)
        {
            //var employees = db.Employees.Where(x => x.emp_id != 17).ToList();
            var employees = db.Leaves.Where(e => e.emp_id == emp_id);
            return Request.CreateResponse(HttpStatusCode.OK, employees.ToList());
        }



        [HttpPost]
        public HttpResponseMessage PostLeave(Leave leave)
        {
            var leaves = db.Leaves.Add(leave);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, leave.leave_id);
        }
        [HttpPost]
        public HttpResponseMessage RejectLeave(Leave leave)
        {
            Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
            lv.leave_status = "Rejected";
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, leave.leave_id);
        }

        public HttpResponseMessage AcceptShort(Leave leave)
        {
            EmployeeLeave employeeS = db.EmployeeLeaves.FirstOrDefault(e => e.emp_id == leave.emp_id);
            double shortLeave = 0.5;
            if ( employeeS.short_leave != null )
            {
                employeeS.short_leave = employeeS.short_leave - shortLeave;
                employeeS.remaining_leaves = employeeS.remaining_leaves - shortLeave;
                Leave Sleave = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
                Sleave.leave_status = "Accepted";
                db.SaveChanges();
            }
           
            return Request.CreateResponse(HttpStatusCode.OK, leave.leave_id);
        }
        [HttpPost]
        public HttpResponseMessage AcceptLeave(Leave leave)
        {
            //Attendance attend = db.Attendances.FirstOrDefault(a1 => a1.emp_id == leave.emp_id);
            EmployeeLeave empleave = db.EmployeeLeaves.FirstOrDefault(el => el.emp_id == leave.emp_id);
            DateTime toDate = DateTime.Parse(leave.date_to.ToString());
            DateTime fromDate = DateTime.Parse(leave.date_from.ToString());
            int days = int.Parse(toDate.Subtract(fromDate).TotalDays.ToString());
           
            if(empleave.remaining_leaves > 0 && days < empleave.remaining_leaves)
            {
                if(leave.leave_type == "Sick")
                {
                    if(days > empleave.sick_leave)
                    {
                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
                        lv.leave_status = "Rejected";
                        db.SaveChanges();
                    }
                    else
                    {
                        empleave.sick_leave = empleave.sick_leave - days;
                        empleave.remaining_leaves = empleave.remaining_leaves - days;
                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
                        lv.leave_status = "Accepted";
                        db.SaveChanges();
                    }
                }else if(leave.leave_type == "casual")
                {
                    if (days > empleave.casual_leave)
                    {
                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
                        lv.leave_status = "Rejected";
                        db.SaveChanges();
                    }
                    else
                    {
                        empleave.casual_leave = empleave.casual_leave - days;
                        empleave.remaining_leaves = empleave.remaining_leaves - days;
                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
                        lv.leave_status = "Accepted";
                        db.SaveChanges();
                    }
                }
                else if(leave.leave_type == "earned")
                {
                    if (days > empleave.earned_leave)
                    {
                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
                        lv.leave_status = "Rejected";
                        db.SaveChanges();
                    }
                    else
                    {
                        empleave.earned_leave = empleave.earned_leave - days;
                        empleave.remaining_leaves = empleave.remaining_leaves - days;
                        Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
                        lv.leave_status = "Accepted";
                        db.SaveChanges();
                    }
                }
                else
                {                   
                        
                }
            }
            else
            {
                Leave lv = db.Leaves.FirstOrDefault(l => l.leave_id == leave.leave_id);
                lv.leave_status = "Rejected";
                db.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }






    
    }


}

    


// [HttpPut]
//public HttpResponseMessage LeavePut(int id, [FromBody]Leave fo)
//{
//    try
//    {
//            var entity = db.Leaves.FirstOrDefault(f => f.leave_id == id);
//            if (entity == null)
//            {
//                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "food with " + id.ToString() + "not found");

//            }
//            else
//            {
//            entity.total_leave = fo.total_leave;
//            entity.sick_leave = fo.sick_leave;
//            entity.earned_leave = fo.earned_leave;
//            entity.casual_leave = fo.casual_leave;
//            entity.short_leave = fo.short_leave;


//                db.SaveChanges();
//                return Request.CreateResponse(HttpStatusCode.OK, entity);
//            }

//    }
//    catch (Exception ex) { return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex); }
//}

/*foreach(var a in empList)
                {
                    if(a.emp_id == )
                    {
                        db.Attendances.Add(new Attendance()
                        {
                              status="P",
                              date=attendeence.date,
                              emp_id=Convert.ToInt32(a.emp_id),

                        });
                     }
                    else
                    {
                        db.Attendances.Add(new Attendance()
                        {
                            status = "A",
                            date = attendeence.date,
                            emp_id = Convert.ToInt32(a.emp_id),
                        });
                    }
                    i = i + 1;
                }*/
