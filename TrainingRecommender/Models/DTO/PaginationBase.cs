using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingRecommender.Models.DTO
{
    public class PaginationBase
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
