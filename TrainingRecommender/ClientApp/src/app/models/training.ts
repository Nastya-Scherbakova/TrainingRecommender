import { Exercise } from './exercise';
import { TrainingMuscle } from './training-muscle';
import { UserTraining } from './user-training';

export class Training {
  id: number;
  title: string;
  duration: number;
  trainingRate: number; // днів на тиждень
  about: string;
  level: Level;
  score: number;
  exercises: Exercise[] = [];
  muscles: TrainingMuscle[];
  isRecommended = false;
  userTrainings: UserTraining[];
}

enum Level {
  Easy,
  Medium,
  Hard
}
