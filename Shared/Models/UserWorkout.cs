using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyHands.Shared.Models
{
    public class UserWorkout
    {
        public string UserWorkoutId { get; set; }
        public string WorkoutName { get; set; }
        public int WorkoutType { get; set; }
        public int Intensity { get; set; }
        public int Length { get; set; }
        public DateTime WorkoutDate { get; set; }
        public int CaloriesBurned { get; set; }

    }
}
