using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyHands.Shared.Models
{
    public class UserWeight
    {
        public string UserWeightId { get; set; }
        public double Weight { get; set; }
        public DateTime WeightDate { get; set; }
    }
}
