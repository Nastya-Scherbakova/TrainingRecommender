using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TrainingRecommender.Models;
using TrainingRecommender.Models.DTO;

namespace TrainingRecommender.Data
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<ApplicationUser, User>().ReverseMap();
        }
    }
}
