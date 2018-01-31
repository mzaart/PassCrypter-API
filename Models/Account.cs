using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PassCrypter.Models
{
      public class Account
      {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
            public int ID { get; set; }

            [Required]
            public int userID { get; set; }

            [Required]
            public string tag { get; set; } // website tag
            [Required]
            public string websiteUrl { get; set; } // url

            // the following hex values are encrypted
            public string email { get; set; }
            public string username { get; set; }
            public string password { get; set; }

            // the iv used for incryption
            [Required]
            public string iv { get; set; }
      }
}