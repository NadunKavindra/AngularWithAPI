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
using Employee_API.Models;
using System.Web.Http.Cors;
using System.Web;
using Newtonsoft.Json;

namespace Employee_API.Controllers
{
    //To enable Cross domain request blocking  CORS
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]

        /// <summary>
        /// This is Employee controller 
        /// </summary>
    public class EmployeesController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/Employees
        public IQueryable<Employee> GetEmployees()
        {
            return db.Employees;
        }


        /*
        //Pagination
        [HttpGet]
        public IEnumerable<Employee> GetEmployee([FromUri]PagingPaameterModel pagingparametermodel)
        {
            //Return List of Eployee
            var source = (from employee in db.Employees
                          .OrderBy(a => a.EmployeeID)
                          select employee).AsQueryable();

            //Get Number of row count
            int count = source.Count();

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pagingparametermodel.pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pagingparametermodel.pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";


            //************************************************************************************************************************
            //Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };


            // Setting Header  
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            //************************************************************************************************************************


            // Returing List of Customers Collections  
            return items;
        }
        */

        // GET: api/Employees/5
        /// <summary>
        /// Get specific Employee details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Employee))]
        public IHttpActionResult GetEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        /// <summary>
        /// Put Employees for the Database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            //Validation done in Angular App
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Employees
        [ResponseType(typeof(Employee))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            //Validation done in Angular App
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            db.Employees.Add(employee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employee.EmployeeID }, employee);
        }

        // DELETE: api/Employees/5
        /// <summary>
        /// Delete Specific Employee using his Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.EmployeeID == id) > 0;
        }
    }
}



//Nav Query For Order List
//SELECT  [FLINTEC$Sales Header].No_ As [Order No],FLINTEC$Customer.Name, [FLINTEC$Sales Line].[Shipment Date], [FLINTEC$Sales Line].[Outstanding Quantity], [FLINTEC$Sales Line].Quantity,  [FLINTEC$Sales Line].[Shipment Method Code], [FLINTEC$Sales Header].[Customer Order No_] As [Customer Ord No], [FLINTEC$Sales Line].Description,[FLINTEC$Sales Line].[Description 2], [FLINTEC$Sales Line].No_ AS Part_No,  FLINTEC$Item.[No_ 2] as [FCUS P.No], FLINTEC$Item.[Item Cross Reference] as [FCDE P.No] FROM [FLINTEC$Sales Line] INNER JOIN [FLINTEC$Sales Header] ON [FLINTEC$Sales Line].[Document No_] = [FLINTEC$Sales Header].No_ INNER JOIN FLINTEC$Customer ON [FLINTEC$Sales Header].[Sell-to Customer No_] = FLINTEC$Customer.No_ INNER JOIN FLINTEC$Item ON [FLINTEC$Sales Line].No_ = FLINTEC$Item.No_ COLLATE SQL_Latin1_General_CP1_CI_AS WHERE ([FLINTEC$Sales Line].[Shipment Date] BETWEEN @fdate AND @tdate) AND ([FLINTEC$Sales Line].No_ LIKE 'OMRS%' OR [FLINTEC$Sales Line].No_ LIKE 'IZRS%') AND ([FLINTEC$Sales Line].Description LIKE '%ILIAD%') order by  [FLINTEC$Sales Line].[Shipment Date]

//Nav Con string
//dynamics_str = "Data Source=Dynamics\\Nav;Initial Catalog=FLINTEC;User ID=GSP;Password=dcdcrb123";