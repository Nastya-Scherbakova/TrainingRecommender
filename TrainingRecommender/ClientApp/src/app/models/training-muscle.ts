import { Muscle } from './muscle';

export interface TrainingMuscle {
  id: number;
  trainingId: number;
  muscleId: number;
  muscle: Muscle;
}
