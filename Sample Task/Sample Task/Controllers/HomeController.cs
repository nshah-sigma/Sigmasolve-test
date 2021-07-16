using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Sample_Task.Models;
namespace Sample_Task.Controllers
{
    public class HomeController : Controller
    {

        //public HomeController()
        //{
        //    string constr = ConfigurationManager.AppSettings["connectionString"];
        //    // MongoClient dbClient = new MongoClient("mongodb://127.0.0.1:27017");
        //    MongoClient dbClient = new MongoClient(constr);
        //    var connectionString = "mongodb://localhost";
        //    var mongoClient = new MongoClient(connectionString);
        //    var dbList = dbClient.ListDatabases().ToList();

        //}

        [HttpGet]

        public ActionResult Index()
        {
           List<EmployeeDetails> emp = new List<EmployeeDetails>();

            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase("Employee");
            var collection = db.GetCollection<EmployeeDetails>("EmployeeDetails").Find(new BsonDocument()).ToList();

           
            
            return View(collection);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost ]
        public ActionResult Create(EmployeeDetails Emp)
        {
            if (ModelState.IsValid)
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);

                var DB = Client.GetDatabase("Employee");
                var collection = DB.GetCollection<EmployeeDetails>("EmployeeDetails");
              


                collection.InsertOneAsync(Emp);
                return RedirectToAction("Index");
            }
            return View();
           
        }
        
        
        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("Employee");
                ObjectId Id1 = MongoDB.Bson.ObjectId.Parse(id);
                var collection = DB.GetCollection<EmployeeDetails>("EmployeeDetails");
                var DeleteRecored = collection.DeleteOneAsync(
                               Builders<EmployeeDetails>.Filter.Eq("Id", Id1));
                return RedirectToAction("Index");
            }
            return View();
            
        }

        public ActionResult Details(string id)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase("Employee");
            var collection = db.GetCollection<EmployeeDetails>("EmployeeDetails").Find(new BsonDocument()).ToList();
            ObjectId Id1 = MongoDB.Bson.ObjectId.Parse(id) ;
            var editcollection = collection.Where(x => x.Id == Id1).FirstOrDefault();

            return View(editcollection);

            

        }
        public ActionResult Edit(string id)
        {
             string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase("Employee");

            ObjectId Id1 = MongoDB.Bson.ObjectId.Parse(id);
            var collection = db.GetCollection<EmployeeDetails>("EmployeeDetails").Find(new BsonDocument()).ToList();
           var  editcollection = collection.Where(x => x.Id == Id1).FirstOrDefault();

            return View(editcollection);
           
        }
        [HttpPost]
        public ActionResult Edit(EmployeeDetails Emp)
        {
            try
            {

            
            Emp.Id= MongoDB.Bson.ObjectId.Parse(Emp.SID);
           
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var Db = Client.GetDatabase("Employee");
                var collection = Db.GetCollection<EmployeeDetails>("EmployeeDetails");

                var update = collection.FindOneAndUpdateAsync(Builders<EmployeeDetails>.Filter.Eq("Id", Emp.Id), Builders<EmployeeDetails>.Update.Set("FirstName", Emp.FirstName).Set("LastName", Emp.LastName).Set("Department", Emp.Department).Set("Mobile", Emp.Mobile).Set("Address", Emp.Address).Set("Pincode", Emp.Pincode));

                return RedirectToAction("Index");

            }
            catch (Exception)
            {

                return View();
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}