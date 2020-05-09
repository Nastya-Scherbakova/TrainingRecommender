using Microsoft.ML.Data;

namespace TrainingRecommenderML.Model
{
    public class ModelInput
    {
        [ColumnName("Weight"), LoadColumn(0)]
        public float Weight { get; set; }


        [ColumnName("Height"), LoadColumn(1)]
        public float Height { get; set; }


        [ColumnName("Age"), LoadColumn(2)]
        public float Age { get; set; }


        [ColumnName("FigureType"), LoadColumn(3)]
        public float FigureType { get; set; }


        [ColumnName("Gender"), LoadColumn(4)]
        public float Gender { get; set; }


        [ColumnName("Activity"), LoadColumn(5)]
        public float Activity { get; set; }


        [ColumnName("Goal"), LoadColumn(6)]
        public float Goal { get; set; }


        [ColumnName("TrainingRate"), LoadColumn(7)]
        public float TrainingRate { get; set; }


        [ColumnName("Level"), LoadColumn(8)]
        public float Level { get; set; }


        [ColumnName("Duration"), LoadColumn(9)]
        public float Duration { get; set; }


        [ColumnName("ExerciseIndex"), LoadColumn(10)]
        public float ExerciseIndex { get; set; }


        [ColumnName("UserId"), LoadColumn(11)]
        public string UserId { get; set; }


        [ColumnName("TrainingId"), LoadColumn(12)]
        public float TrainingId { get; set; }


        [ColumnName("Score"), LoadColumn(13)]
        public float Score { get; set; }


    }
}
