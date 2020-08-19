using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User
    {
        
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string password { get; set; }
        public bool isDeleted { get; set; }
        public string mailId { get; set; }
        public string role { get; set; }
        //public string token { get; set; }
    }
}
