import { Training } from './training';

export interface UserTraining {
  id: number;
  score: number;
  // індекс вправ впливає на кількість вправ (обчислюється за даними користувача)
  exerciseIndex: number;
  userId: string;
  trainingId: number;
  training: Training;
}
