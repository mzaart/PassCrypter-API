using System.Collections.Generic;

namespace PassCrypter.Models
{
      public class Settings 
      {
            public Settings()
            {
                  alert = true;
                  recentActivity = new List<Activity>();
            }

            public bool alert { get; set; }
            public List<Activity> recentActivity { get; set; }
      }

      public class Activity
      {
            public int time { get; set; }
            public string browser { get; set; }
            public string os { get; set; }
      }
}