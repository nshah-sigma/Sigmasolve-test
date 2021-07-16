using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using sampleAPITask.Models;
using static sampleAPITask.Areas.HelpPage.Models.Utility;

namespace sampleAPITask.Areas.HelpPage.Controllers
{
    public class EmolpyeeController : ApiController
    {
        string constr = ConfigurationManager.AppSettings["connectionString"];
        [HttpGet]
        [Route("api/GetAllEmployee/")]
        public IHttpActionResult GetAllEmployee()
        {
            JsonResponse response = new JsonResponse();

            List<EmployeeDetail> emp = new List<EmployeeDetail>();

            try
            {
                var Client = new MongoClient(constr);
                var db = Client.GetDatabase("Employee");
                var collection = db.GetCollection<EmployeeDetail>("EmployeeDetails").Find(new BsonDocument()).ToList();
                response.data = collection;
                response.Message = "Successfully";

                response.HttpStatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.data = "";
                response.Message = ex.Message.ToString();

                response.HttpStatusCode = (int)HttpStatusCode.BadRequest;

            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter));
        }
        [Route("api/GetEmployeeById/{id}")]
        public IHttpActionResult GetEmployeeById(string id)
        {
            JsonResponse response = new JsonResponse();



            try
            {
                var Client = new MongoClient(constr);
                var db = Client.GetDatabase("Employee");
                var collection = db.GetCollection<EmployeeDetail>("EmployeeDetails").Find(new BsonDocument()).ToList();
                ObjectId oID = MongoDB.Bson.ObjectId.Parse(id);

                response.data = collection.Where(x => x.Id == oID).FirstOrDefault();
                response.Message = "Successfully";

                response.HttpStatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.data = "";
                response.Message = ex.Message.ToString();

                response.HttpStatusCode = (int)HttpStatusCode.BadRequest;

            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter));
        }
        [Route("api/CreateEmployee")]
        [HttpPost]
        public IHttpActionResult CreateEmployee(EmployeeDetail Emp)
        {
            JsonResponse response = new JsonResponse();

            try
            {
                var Client = new MongoClient(constr);

                var DB = Client.GetDatabase("Employee");
                var collection = DB.GetCollection<EmployeeDetail>("EmployeeDetails");

                collection.InsertOneAsync(Emp);

                response.data = Emp;
                response.Message = "create employee  Successfully";

                response.HttpStatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.data = "";
                response.Message = ex.Message.ToString();

                response.HttpStatusCode = (int)HttpStatusCode.BadRequest;

            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter));
        }

        [Route("api/DeleteEmployeeById/{id}")]
        public IHttpActionResult DeleteEmployeeById(string id)
        {
            JsonResponse response = new JsonResponse();

            try
            {
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("Employee");
                ObjectId Id1 = MongoDB.Bson.ObjectId.Parse(id);
                var collection = DB.GetCollection<EmployeeDetail>("EmployeeDetails");
                var DeleteRecored = collection.DeleteOneAsync(
                               Builders<EmployeeDetail>.Filter.Eq("Id", Id1));

                response.data = "";
                response.Message = "Delete record Successfully";

                response.HttpStatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.data = "";
                response.Message = ex.Message.ToString();

                response.HttpStatusCode = (int)HttpStatusCode.BadRequest;

            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter));
        }

        [Route("api/GetEmployeeByName/{Name}")]
        public IHttpActionResult GetEmployeeByName(string Name)
        {
            JsonResponse response = new JsonResponse();



            try
            {
                var Client = new MongoClient(constr);
                var db = Client.GetDatabase("Employee");
                var collection = db.GetCollection<EmployeeDetail>("EmployeeDetails").Find(new BsonDocument()).ToList();
                //ObjectId oID = MongoDB.Bson.ObjectId.Parse(id);

                response.data = collection.Where(x => x.FirstName.ToLower() == Name.ToLower() || x.LastName.ToLower() == Name.ToLower()).ToList();
                response.Message = "Successfully";

                response.HttpStatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.data = "";
                response.Message = ex.Message.ToString();

                response.HttpStatusCode = (int)HttpStatusCode.BadRequest;

            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter));
        }
        [Route("api/GetEmployeeByDepartment/{Department}")]
        public IHttpActionResult GetEmployeeByDepartment(string Department)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                var Client = new MongoClient(constr);
                var db = Client.GetDatabase("Employee");
                var collection = db.GetCollection<EmployeeDetail>("EmployeeDetails").Find(new BsonDocument()).ToList();
                //ObjectId oID = MongoDB.Bson.ObjectId.Parse(id);

                response.data = collection.Where(x => x.Department.ToLower() == Department.ToLower()).ToList();
                response.Message = "Successfully";

                response.HttpStatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.data = "";
                response.Message = ex.Message.ToString();

                response.HttpStatusCode = (int)HttpStatusCode.BadRequest;

            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter));
        }

        //Aggregate function use department=====
        [Route("api/GetDepartmentwiseemployeecnt")]
        public IHttpActionResult GetDepartmentwiseemployeecntt()
        {
            JsonResponse response = new JsonResponse();
            try
            {
                var Client = new MongoClient(constr);
                var db = Client.GetDatabase("Employee");
               // var collection = db.GetCollection<EmployeeDetail>("EmployeeDetails").Find(new BsonDocument()).ToList();
                //ObjectId oID = MongoDB.Bson.ObjectId.Parse(id);

                //response.data = collection.Where(x => x.Department.ToLower() == Department.ToLower()).ToList();
              

                var docs = db.GetCollection<EmployeeDetail>("EmployeeDetails").Aggregate()
                                 .Group(y => y.Department,
                                        z => new {
                                            Department = z.Key,
                                            Total = z.Sum(a => 1)
                                        }
                                 ).ToList();

                response.data = docs;
               
                response.Message = "Successfully";

                response.HttpStatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.data = "";
                response.Message = ex.Message.ToString();

                response.HttpStatusCode = (int)HttpStatusCode.BadRequest;

            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter));
        }


    }
}
