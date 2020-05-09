import { UserDisease } from './user-disease';
import { UserTraining } from './user-training';

export interface User {
  id: string;
  email: string;
  userName: string;
  name: string;
  surname: string;
  gender: Gender;
  age: number;
  weight: number;
  height: number;
  // частота тренувань (кількість тренувань / кількість днів, наприклад 2/7 - двічі на тиждень)
  trainingRate: number;
  activity: Activity;
  goal: Goal;
  figureType: FigureType;
  userDiseases: UserDisease[];
  userTrainings: UserTraining[];
  roles: string[];
}

export enum Activity {
  Minimal,
  Middle,
  High
}

export enum FigureType {
  Thin,
  Normal,
  Muscles,
  Big
}

export enum Goal {
  Thin,
  Muscles,
  Fit
}

export enum Gender {
  Male,
  Female
}
