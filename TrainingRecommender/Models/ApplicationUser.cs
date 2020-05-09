using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TrainingRecommender.Models
{
    public class ApplicationUser : IdentityUser
    {
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
    }

    public enum Activity
    {
        Minimal,
        Middle,
        High
    }

    public enum FigureType
    {
        Thin,
        Normal,
        Muscles,
        Big
    }

    public enum Goal
    {
        Thin,
        Muscles,
        Fit
    }

    public enum Gender
    {
        Male,
        Female
    }
}
