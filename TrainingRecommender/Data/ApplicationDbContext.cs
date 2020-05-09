using TrainingRecommender.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TrainingRecommender.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Disease> Disease { get; set; }
        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<Muscle> Muscle { get; set; }
        public DbSet<Training> Training { get; set; }
        public DbSet<TrainingMuscle> TrainingMuscle { get; set; }
        public DbSet<UserDisease> UserDisease { get; set; }
        public DbSet<UserTraining> UserTraining { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(el => el.UserTrainings)
                .WithOne()
                .HasForeignKey(el => el.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(el => el.UserDiseases)
                .WithOne()
                .HasForeignKey(el => el.UserId);

            modelBuilder.Entity<Training>()
                .HasMany(el => el.Exercises)
                .WithOne()
                .HasForeignKey(el => el.TrainingId);

            modelBuilder.Entity<Training>()
                .HasMany(el => el.Muscles)
                .WithOne()
                .HasForeignKey(el => el.TrainingId);

            modelBuilder.Entity<TrainingMuscle>()
                .HasOne(el => el.Muscle)
                .WithMany()
                .HasForeignKey(el => el.MuscleId);

            modelBuilder.Entity<UserDisease>()
                .HasOne(el => el.Disease)
                .WithMany()
                .HasForeignKey(el => el.DiseaseId);

            modelBuilder.Entity<UserTraining>()
                .HasOne(el => el.Training)
                .WithMany(el => el.UserTrainings)
                .HasForeignKey(el => el.TrainingId);

            modelBuilder.Entity<Muscle>(entity => { entity.HasIndex(e => e.Name).IsUnique(); });

            modelBuilder.Entity<Disease>(entity => { entity.HasIndex(e => e.Name).IsUnique(); });
        }
    }
}
