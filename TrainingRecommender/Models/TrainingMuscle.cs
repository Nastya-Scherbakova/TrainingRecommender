using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingRecommender.Models
{
    public class TrainingMuscle
    {
        public int Id { get; set; }
        public int TrainingId { get; set; }
        public int MuscleId { get; set; }
        public Muscle Muscle { get; set; }
    }
}
