using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingRecommender.Models
{
    public class UserTraining
    {
        public int Id { get; set; }
        public int Score { get; set; }
        // індекс вправ впливає на кількість вправ (обчислюється за даними користувача)
        public double ExerciseIndex { get; set; }
        public string UserId { get; set; }
        public int TrainingId { get; set; }
        public Training Training { get; set; }
    }
}
