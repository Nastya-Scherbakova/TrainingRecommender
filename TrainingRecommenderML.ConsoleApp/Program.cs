using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using TrainingRecommenderML.Model;

namespace TrainingRecommenderML.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            ModelBuilder.CreateModel();

            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = CreateSingleDataSample();

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Score with predicted Score from sample data...\n\n");
            Console.WriteLine($"Weight: {sampleData.Weight}");
            Console.WriteLine($"Height: {sampleData.Height}");
            Console.WriteLine($"Age: {sampleData.Age}");
            Console.WriteLine($"FigureType: {sampleData.FigureType}");
            Console.WriteLine($"Gender: {sampleData.Gender}");
            Console.WriteLine($"Activity: {sampleData.Activity}");
            Console.WriteLine($"Goal: {sampleData.Goal}");
            Console.WriteLine($"TrainingRate: {sampleData.TrainingRate}");
            Console.WriteLine($"Level: {sampleData.Level}");
            Console.WriteLine($"Duration: {sampleData.Duration}");
            Console.WriteLine($"ExerciseIndex: {sampleData.ExerciseIndex}");
            Console.WriteLine($"UserId: {sampleData.UserId}");
            Console.WriteLine($"TrainingId: {sampleData.TrainingId}");
            Console.WriteLine($"\n\nActual Score: {sampleData.Score} \nPredicted Score: {predictionResult.Score}\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }

        // Change this code to create your own sample data
        #region CreateSingleDataSample
        // Method to load single row of dataset to try a single prediction
        private static ModelInput CreateSingleDataSample()
        {
            // Create MLContext
            MLContext mlContext = new MLContext();

            // Load dataset
            var loader = mlContext.Data.CreateDatabaseLoader<ModelInput>();
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Database=TrainingRecommender;Integrated Security=True;Connect Timeout=30";

            string sqlCommand = @"SELECT CAST(Weight as REAL) as Weight, 
                                    CAST(Height as REAL) as Height, CAST(Age as REAL) as Age, CAST(FigureType as REAL) as FigureType, CAST(Gender as REAL) as Gender,
                                    CAST(Activity as REAL) as Activity, CAST(Goal as REAL) as Goal, CAST(TrainingRate as REAL) as TrainingRate, CAST(Level as REAL) as Level,
                                    CAST(Duration as REAL) as Duration, CAST(ExerciseIndex as REAL) as ExerciseIndex,
                                    UserId, CAST(TrainingId as REAL) as TrainingId, 
                                    CAST(Score as REAL) as Score FROM UserTrainingsView";

            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlCommand);

            IDataView dataView = loader.Load(dbSource);

            // Use first line of dataset as model input
            // You can replace this with new test data (hardcoded or from end-user application)
            ModelInput sampleForPrediction = mlContext.Data.CreateEnumerable<ModelInput>(dataView, false)
                                                                        .First();
            return sampleForPrediction;
        }
        #endregion
    }
}
