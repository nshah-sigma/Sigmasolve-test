using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

using MongoDB.Bson.Serialization.Attributes;
using sampleAPITask.Models;
namespace sampleAPITask.Controllers
{
    public class HomeController : Controller
    {

        string constr = ConfigurationManager.AppSettings["connectionString"];
        [HttpGet]
        public ActionResult Index()
        {
            List<EmployeeDetail> emp = new List<EmployeeDetail>();


            var Client = new MongoClient(constr);
            var db = Client.GetDatabase("Employee");

            var collection = db.GetCollection<EmployeeDetail>("EmployeeDetails").Find(new BsonDocument()).ToList();

            return View(collection);
        }

        public ActionResult Task2()
        {

            return View();
        }
        public ActionResult APIList()
        {

            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(EmployeeDetail Emp)
        {
            if (ModelState.IsValid)
            {
                var client = new MongoClient(constr);

                //Create a session object that is used when leveraging transactions
                var session = client.StartSession();
                // var Client = new MongoClient(constr);

                var DB = session.Client.GetDatabase("Employee");
                //   var collection = DB.GetCollection<EmployeeDetail>("EmployeeDetails");
                var collection = session.Client.GetDatabase("Employee").GetCollection<EmployeeDetail>("EmployeeDetails");
                //Begin transaction
                //session.StartTransaction();
                try
                {


                     collection.InsertOneAsync(Emp);
                   // session.CommitTransaction();
                }
                catch (Exception ex)
                {

                 //   session.StartTransaction();
                }
                
                return RedirectToAction("Index");
            }
            return View();

        }


        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {

                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("Employee");
                ObjectId Id1 = MongoDB.Bson.ObjectId.Parse(id);
                var collection = DB.GetCollection<EmployeeDetail>("EmployeeDetails");
                var DeleteRecored = collection.DeleteOneAsync(
                               Builders<EmployeeDetail>.Filter.Eq("Id", Id1));
                return RedirectToAction("Index");
            }
            return View();

        }

        public ActionResult Details(string id)
        {

            var Client = new MongoClient(constr);
            var db = Client.GetDatabase("Employee");
            var collection = db.GetCollection<EmployeeDetail>("EmployeeDetails").Find(new BsonDocument()).ToList();
            ObjectId Id1 = MongoDB.Bson.ObjectId.Parse(id);
            var editcollection = collection.Where(x => x.Id == Id1).FirstOrDefault();

            return View(editcollection);



        }
        public ActionResult Edit(string id)
        {

            var Client = new MongoClient(constr);
            var db = Client.GetDatabase("Employee");

            ObjectId Id1 = MongoDB.Bson.ObjectId.Parse(id);
            var collection = db.GetCollection<EmployeeDetail>("EmployeeDetails").Find(new BsonDocument()).ToList();
            var editcollection = collection.Where(x => x.Id == Id1).FirstOrDefault();

            return View(editcollection);

        }
        [HttpPost]
        public ActionResult Edit(EmployeeDetail Emp)
        {
            try
            {


                Emp.Id = MongoDB.Bson.ObjectId.Parse(Emp.SID);


                var Client = new MongoClient(constr);
                var Db = Client.GetDatabase("Employee");
                var collection = Db.GetCollection<EmployeeDetail>("EmployeeDetails");

                var update = collection.FindOneAndUpdateAsync(Builders<EmployeeDetail>.Filter.Eq("Id", Emp.Id), Builders<EmployeeDetail>.Update.Set("FirstName", Emp.FirstName).Set("LastName", Emp.LastName).Set("Department", Emp.Department).Set("Mobile", Emp.Mobile).Set("Address", Emp.Address).Set("Pincode", Emp.Pincode));

                return RedirectToAction("Index");

            }
            catch (Exception)
            {

                return View();
            }
        }
    }
}
