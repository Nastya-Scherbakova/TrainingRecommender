using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingRecommender.Data.Migrations
{
    public partial class CreateViewForML : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER VIEW UserTrainingsView
                                    AS
                                    SELECT
                                        u.Weight,
	                                    u.Height,
	                                    u.Age,
	                                    u.FigureType,
	                                    u.Gender,
	                                    u.Activity,
	                                    u.Goal,
	                                    t.TrainingRate,
	                                    t.Level,
	                                    t.Duration,
	                                    ut.ExerciseIndex,
	                                    ut.UserId,
	                                    ut.TrainingId,
	                                    ut.Score
                                    FROM
                                        AspNetUsers AS u
                                    INNER JOIN UserTraining AS ut
                                        ON u.Id = ut.UserId
                                    INNER JOIN Training AS t
                                        ON t.Id = ut.TrainingId;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW UserTrainingsView;");
        }
    }
}
