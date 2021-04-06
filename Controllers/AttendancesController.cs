using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BIIT_HR.Models;

namespace BIIT_HR.Controllers
{
    public class AttendancesController : ApiController
    {
        private BIIT_HREntities6 db = new BIIT_HREntities6();
        List<List<string>> newList = new List<List<string>>();
        [HttpGet]
        public HttpResponseMessage AllAttendence()
        {
            List<Employee> employee = db.Employees.Where(x => x.emp_id != 17).ToList();
            List<Attendance> attendance = db.Attendances.ToList();


            var result = from at in attendance
                         join emp in employee on at.emp_id equals emp.emp_id
                         select new
                         {
                             leaveId = at.attendance_id,
                             EmpID = emp.emp_id,
                             staus = at.status,
                             date = at.date,
                             EmpName = emp.name,
                             desig = emp.designation,
                             pic = emp.picture,

                         };
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [HttpPost]
        public HttpResponseMessage OneAttendence(int emp_id)
        {
            var employees = db.Attendances.Where(e => e.emp_id == emp_id);
            return Request.CreateResponse(HttpStatusCode.OK, employees.ToList());
        }

        [HttpPost]
        public HttpResponseMessage PostAttendence(Attendance attendeence)
        {

            using (BIIT_HREntities6 entites = new BIIT_HREntities6())
            {
                List<Employee> empList = entites.Employees.ToList();
                List<string> csvList = attendeence.csv.Split(',').ToList();


                foreach (var a in empList)
                {
                    if (csvList.Contains(a.emp_id.ToString()))
                    {
                        db.Attendances.Add(new Attendance()
                        {
                            status = "P",
                            date = attendeence.date,
                            emp_id = Convert.ToInt32(a.emp_id),

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

                }

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, attendeence.attendance_id);

            }

        }

        [HttpPost]
        public HttpResponseMessage PostAttendenceLeave()
        {
            DateTime currentDate = DateTime.Now.Date;
            var employees = db.Employees.Select(e => e);
            if (employees != null)
            {
                foreach (var emp in employees)
                {

                    var leav = db.Leaves.Where(l => l.emp_id == emp.emp_id && l.leave_status == "Accepted").ToList();
                    if (leav != null)
                    {
                        foreach (var l in leav)
                        {
                            if (currentDate <= (Convert.ToDateTime(l.date_from)).Date && currentDate <= (Convert.ToDateTime(l.date_to)).Date)
                            {
                                List<string> elistt = new List<string>();
                                elistt.Add(emp.emp_id.ToString());
                                elistt.Add(emp.name.ToString());
                                elistt.Add(emp.designation.ToString());
                                elistt.Add(Convert.ToBase64String(emp.picture));
                                elistt.Add(emp.leave_status.ToString());
                                newList.Add(elistt);
                            }
                        }
                    }

                }
            }
            foreach (var a in newList)
                {
                db.Attendances.Add(new Attendance()
                {
                    status = "L",
                    date = currentDate,
                    emp_id = Convert.ToInt32(a[0]),

                });
            }

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, true);

            }

        }
    }
