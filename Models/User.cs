using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PassCrypter.Models
{
      public class User
      {
            public User() {}

            public User(string name, string email, string passHash)
            {
                  this.name = name;
                  this.email = email;
                  this.passHash = passHash;

                  this.settings = JsonConvert.SerializeObject(new Settings());
            }

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
            public int ID { get; set; }

            [Required]
            public string email { get; set; }
            [Required]
            public string passHash { get; set; }
            [Required]
            public string name { get; set; }

            [Required]
            public string settings { get; set; }
      }
}