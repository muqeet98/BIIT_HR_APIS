using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BIIT_HR.Models;
namespace BIIT_HR.Controllers
{
    public class EmployeesController : ApiController
    {
        BIIT_HREntities6 db = new BIIT_HREntities6();
        List<List<string>> newList = new List<List<string>>();
        
        // this.Configuration.LazyLoadingEnabled = false;
        // this.Configuration.ProxyCreationEnabled = false;

        [HttpGet]
        public HttpResponseMessage AllEmployee()
        {
           // var employees = db.Employees.ToList();
            var employees = db.Employees.Where(x => x.emp_id != 58 && x.emp_id != 57).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, employees);
        }
        [HttpGet]
        public HttpResponseMessage AllEmployeeLeaveCheck()
        {
           
            DateTime currentDate = DateTime.Now.Date;            
            var employees = db.Employees.Select(e=>e);
            if(employees != null)
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
            return Request.CreateResponse(HttpStatusCode.OK, newList);
        }


        [HttpGet]
        public HttpResponseMessage AllEmployeeAttendence()
        {

            DateTime currentDate = DateTime.Now.Date;
            List<List<String>> nlist = new List<List<string>>();
            var employees = db.Employees.Select(e => e);
            if (employees != null)
            {
                foreach (var emp in employees)
                {
                    List<string> elistt = new List<string>();
                    elistt.Add(emp.emp_id.ToString());
                    elistt.Add(emp.name.ToString());
                    elistt.Add(emp.designation.ToString());
                    elistt.Add(Convert.ToBase64String(emp.picture));
                    elistt.Add(emp.leave_status.ToString());
                    newList.Add(elistt);
                }
                 nlist = newList.Select(s => s).Distinct().ToList();
                var leav = db.Leaves.Select(l=>l) ;
                if (leav != null)
                {
                    foreach (var l in leav)
                    {
                        if (l.leave_status == "Accepted")
                        {
                            if (currentDate >= Convert.ToDateTime(l.date_from) && currentDate <= Convert.ToDateTime(l.date_to))
                            {
                                foreach(var lis in nlist)
                                {
                                    if(lis[0] == l.emp_id.ToString())
                                    {
                                        nlist.Remove(lis);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //var list = newList.Select(s => s[0]).Distinct();
            return Request.CreateResponse(HttpStatusCode.OK, nlist);
        }



        //[HttpGet]
        //public HttpResponseMessage allemployee(int emp_id)
        //{
        //    var employees = db.Employees.Where(e => e.emp_id == emp_id);
        //    return Request.CreateResponse(HttpStatusCode.OK, employees.tolist());
        //}

        [HttpGet]
        public HttpResponseMessage OneEmployee(int emp_id)
        {
            var employees = db.Employees.Where(e => e.emp_id == emp_id);
            return Request.CreateResponse(HttpStatusCode.OK, employees.ToList());
        }
        [HttpGet]
        public HttpResponseMessage AllEmployee1(string email)
        {
            var employees = db.Employees.Where(e => e.email == email);
            return Request.CreateResponse(HttpStatusCode.OK, employees.ToList());
        }

        [HttpPost]
        public HttpResponseMessage RegisterEmployee(Employee employee)
        {
            var criteriaDetail = db.Criteria.FirstOrDefault(e => e.cri_id == 100);
            var employees = db.Employees.Add(employee);
            db.SaveChanges();
            db.EmployeeLeaves.Add(new EmployeeLeave()
            {
                emp_id = employee.emp_id,
                 total_leaves = criteriaDetail.cri_total_leave,
                 sick_leave = criteriaDetail.cri_sick_leave,
                 casual_leave = criteriaDetail.cri_casual_leave,
                 earned_leave = criteriaDetail.cri_earned_leave,
                 short_leave = criteriaDetail.cri_short_leave,
                 remaining_leaves = criteriaDetail.cri_sick_leave + criteriaDetail.cri_earned_leave + criteriaDetail.cri_casual_leave + criteriaDetail.cri_short_leave,
            });
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, employee.emp_id);
        }
       
        //[Route("api/employees/loginemployee")]


         [HttpGet,HttpPost]
        public HttpResponseMessage LoginEmployee(string email, string password)
        {
            try
              {

                var empdetail = db.Employees.FirstOrDefault(e => e.email == email && e.password == password);
                if (empdetail != null)
                    return Request.CreateResponse(HttpStatusCode.OK, empdetail);
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, empdetail);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage EmployeePut(int id, [FromBody] Employee fo)
        {
            try
            {

                var entity = db.Employees.FirstOrDefault(f => f.emp_id == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "food with " + id.ToString() + "not found");

                }
                else
                {
                    entity.name = fo.name;
                    entity.contact_no = fo.contact_no;
                    entity.email = fo.email;
                    entity.cnic = fo.cnic;
                    entity.password = fo.password;
                    entity.picture = fo.picture;
                    entity.designation = fo.designation;
                    entity.date = fo.date;

                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }
            catch (Exception ex) { return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex); }
        }

        [HttpPost]
        public HttpResponseMessage DeleteEmployee(int emp_id)
        {
            Employee employee = db.Employees.FirstOrDefault(e=>e.emp_id == emp_id);
            var attend = db.Attendances.Where(e=>e.emp_id == emp_id);
            EmployeeLeave empleave = db.EmployeeLeaves.FirstOrDefault(e => e.emp_id == emp_id);
            var leave = db.Leaves.Where(e => e.emp_id == emp_id);
            if(attend != null)
            {
                foreach (var a in attend)
                {
                    db.Attendances.Remove(a);
                }
            }
            db.SaveChanges();
            if (leave!= null)
            {
                foreach (var l in leave)
                {
                    db.Leaves.Remove(l);
                }
            }
            db.SaveChanges();
            if (empleave != null)
            {
                db.EmployeeLeaves.Remove(empleave);
                db.SaveChanges();
            }

            if(employee!= null)
            {
                db.Employees.Remove(employee);
                db.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage RemoveTeam(Employee emp)
        {
            List<Employee> employee = db.Employees.ToList();
            List<EmployeeLeave> empleave = db.EmployeeLeaves.ToList();
            var leave = db.EmployeeLeaves.Where(p => p.emp_id == emp.emp_id);
        

            foreach (var player in leave)
            {
                db.EmployeeLeaves.Remove(player);
            }

            db.SaveChanges();
            


            var team = db.Employees.FirstOrDefault(t => t.emp_id == emp.emp_id);
            db.Employees.Remove(team);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, team.emp_id);

        }

    }

    public class ReturnEmployee
    {
        string Id { get; set; }
        string Name { get; set; }
        string Designation { get; set; }

       // string Leave_status { get; set; }
        string Image { get; set; }

        public ReturnEmployee(string id, string name, string desi, string image)
        {
            Id = id;
            Name = name;
            Designation = desi;
            Image = image;
    
        }
    }

}
