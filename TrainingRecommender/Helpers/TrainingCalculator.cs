using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingRecommender.Models;

namespace TrainingRecommender.Helpers
{
    public class TrainingCalculator
    {
        public static double CalculateExercise(ApplicationUser user, Training training)
        {
            // індекс маси тіла
            double bmi = user.Weight / ((user.Height / 100d) * (user.Height / 100d));
            // індекс маси тіла адаптований (тобто де значення 1 - максимальне навантаження, 0 - мінімальне)
            double bmiAdapted = Math.Min((((bmi + 20) / 100d) - 1) * -1, 1);
            // індекс навантаження в залежності від статі (менше навантаження на дівчат)
            double genderIndex = user.Gender == Gender.Male ? 1 : 0.8d;
            // індекс навантаження в залежності від віку (менше навантаження на старших)
            double ageIndex = Math.Min((((user.Age - 10) / 100d) - 1) * -1, 1);

            // індекси навантаження в залежності від типу фігури і мети тренувань
            double figureIndex = 0.5d;
            switch (user.FigureType)
            {
                case FigureType.Thin:
                    figureIndex = 0.4d;
                    break;
                case FigureType.Normal:
                    figureIndex = 0.5d;
                    break;
                case FigureType.Muscles:
                    figureIndex = 0.6d;
                    break;
                case FigureType.Big:
                    figureIndex = 0.7d;
                    break;
            }
            double goalIndex = 0.2d;
            switch (user.Goal)
            {
                case Goal.Thin:
                    goalIndex = 0.3d;
                    break;
                case Goal.Fit:
                    goalIndex = 0.2d;
                    break;
                case Goal.Muscles:
                    goalIndex = 0.6d;
                    break;
            }

            // індекс навантаження в залежності від складності (нестача компенсуєсться попередніми індексами)
            double levelIndex = 1;
            switch (training.Level)
            {
                case Level.Easy:
                    levelIndex = 1;
                    break;
                case Level.Medium:
                    levelIndex = 0.8d;
                    break;
                case Level.Hard:
                    levelIndex = 0.6d;
                    break;
            }

            var levelFigureGoalIndex = Math.Min((figureIndex + goalIndex + levelIndex) / 2, 1);

            // індекс навантаження в залежності від наявності і кількості захворювань
            var diseaseCount = Math.Min(user.UserDiseases.Count(), 4);
            double diseaseIndex = 1 - (0.2d * diseaseCount);
            double finalIndex = (levelFigureGoalIndex + ageIndex + genderIndex + bmiAdapted + diseaseIndex + user.TrainingRate) / 6d;

            return finalIndex;
        }
    }
}
