using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyHands.Shared.Models
{
    public class UserDemographics
    {
        public string UserDemographicsId { get; set; }
        public int Height { get; set; }
        public int Gender { get; set; }
        public int ActivityLevel { get; set; }
        public DateTime BirthDay { get; set; }

    }
}
