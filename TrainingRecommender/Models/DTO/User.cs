using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TrainingRecommender.Models.DTO
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        // частота тренувань (кількість тренувань / кількість днів, наприклад 2/7 - двічі на тиждень)
        public double TrainingRate { get; set; }
        public Activity Activity { get; set; }
        public Goal Goal { get; set; }
        public FigureType FigureType { get; set; }
        public IEnumerable<UserDisease> UserDiseases { get; set; }
        public IEnumerable<UserTraining> UserTrainings { get; set; }
        public  IEnumerable<string> Roles { get; set; }
    }
}
