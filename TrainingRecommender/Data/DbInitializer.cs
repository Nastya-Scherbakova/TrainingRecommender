using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using TrainingRecommender.Models;

namespace TrainingRecommender.Data
{
    public static class DbInitializer
    {
        public static void Init(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.Muscle.Any())
            {
                context.Muscle.AddRange(
                    new Muscle
                    {
                        Name = "прес"
                    }, new Muscle
                    {
                        Name = "руки"
                    }, new Muscle
                    {
                        Name = "ноги"
                    }, new Muscle
                    {
                        Name = "грудна клітина"
                    }, new Muscle
                    {
                        Name = "сідниці"
                    });
            }
            if (!context.Disease.Any())
            {
                context.Disease.AddRange(
                    new Disease
                    {
                        Name = "сколіоз"
                    }, new Disease
                    {
                        Name = "грижа"
                    }, new Disease
                    {
                        Name = "кіфоз"
                    }, new Disease
                    {
                        Name = "остеохондроз"
                    }, new Disease
                    {
                        Name = "варикоз"
                    },
                    new Disease
                    {
                        Name = "діастаз"
                    });
            }

            context.SaveChanges();
            if (!context.Training.Any())
            {
                var trainings = JsonConvert.DeserializeObject<Training[]>(File.ReadAllText(Environment.CurrentDirectory + "/Data/InitializeData/trainings.json"));
                foreach (var training in trainings)
                {
                    foreach (var muscle in training.Muscles)
                    {
                        var dbMuscle = context.Muscle.FirstOrDefault(el => el.Name == muscle.Muscle.Name);
                        if (dbMuscle == null)
                        {
                            dbMuscle = context.Muscle.Add(muscle.Muscle).Entity;
                        }
                        muscle.MuscleId = dbMuscle.Id;
                        muscle.Muscle = null;
                    }
                }
                context.Training.AddRange(trainings);
            }
            context.SaveChanges();
            if (!context.Users.Any())
            {
                var users = JsonConvert.DeserializeObject<ApplicationUser[]>(File.ReadAllText(Environment.CurrentDirectory + "/Data/InitializeData/users.json"));
                foreach (var user in users)
                {
                    user.UserName = user.Email;
                    var res = userManager.CreateAsync(user, "SafePass_1234567").Result;
                }
                context.SaveChanges();
            }
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole {Name = "admin", NormalizedName = "ADMIN"});
                context.SaveChanges();
            }
            
            if (!context.UserRoles.Any())
            {
                var res = userManager.AddToRoleAsync(context.Users.First(), "admin").Result;
            }
            context.SaveChanges();
        }
    }
}
