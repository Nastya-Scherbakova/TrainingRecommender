using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingRecommender.Models.DTO
{
    public class SearchTrainings: PaginationBase
    {
        public int Muscle { get; set; }
        public bool My { get; set; }
    }
}
