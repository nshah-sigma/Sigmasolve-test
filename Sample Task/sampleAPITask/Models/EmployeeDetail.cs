using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sampleAPITask.Models
{
    public class EmployeeDetail
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }
        public string SID { get; set; }
        [Required(ErrorMessage = "The First Name field is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The Last Name field is required.")]
        public string LastName { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Pincode { get; set; }

    }
}