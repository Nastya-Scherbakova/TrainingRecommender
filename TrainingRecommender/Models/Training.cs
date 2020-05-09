using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingRecommender.Models
{
    public class Training
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; } // днів
        public double TrainingRate { get; set; } // днів на тиждень
        public string About { get; set; }
        public int Score { get; set; }
        public Level Level { get; set; }
        public IEnumerable<Exercise> Exercises { get; set; }
        public IEnumerable<TrainingMuscle> Muscles { get; set; }
        public IEnumerable<UserTraining> UserTrainings { get; set; }
    }

    public enum Level
    {
        Easy,
        Medium,
        Hard
    }
}
