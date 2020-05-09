import { Disease } from './disease';

export interface UserDisease {
  id: number;
  userId: string;
  diseaseId: number;
  disease: Disease;
}
