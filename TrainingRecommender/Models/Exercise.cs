using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingRecommender.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public int TrainingId { get; set; }
        public string Title { get; set; }
        public string Guide { get; set; }
        public string Attachment { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
    }
}
