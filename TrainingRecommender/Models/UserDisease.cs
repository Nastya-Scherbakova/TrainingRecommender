using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingRecommender.Models
{
    public class UserDisease
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DiseaseId { get; set; }
        public Disease Disease { get; set; }
    }
}
