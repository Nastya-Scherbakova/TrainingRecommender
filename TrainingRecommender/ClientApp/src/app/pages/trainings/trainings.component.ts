import { Component, OnInit, TemplateRef } from '@angular/core';
import { Muscle } from '../../models/muscle';
import { MusclesService } from '../../services/muscles.service';
import { TrainingsService } from '../../services/trainings.service';
import { Training } from '../../models/training';
import { SearchTrainings } from '../../models/search-models';
import { UsersService } from '../../services/users.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { User } from '../../models/user';
import { TrainingMuscle } from '../../models/training-muscle';
import { Exercise } from '../../models/exercise';

@Component({
  templateUrl: './trainings.component.html',
  styleUrls: ['./trainings.component.sass']
})
export class TrainingsComponent implements OnInit {
  muscles: Muscle[];
  trainings: Training[];
  searchObj = new SearchTrainings();
  stopScroll = false;
  tempEx: Exercise;
  isTempShown = false;
  isAdding = false;
  user: User;
  training = new Training();
  isAdmin = false;
  modalRef: BsModalRef;
  trainingMuscles: Muscle[];
  private recommended: Training[] = [];
  constructor(
    private readonly userService: UsersService,
    private readonly musclesService: MusclesService,
    private modalService: BsModalService,
    private readonly trainingsService: TrainingsService
  ) { }

  ngOnInit() {
    this.userService.current().subscribe(user => {
      this.user = user;
      this.isAdmin = user.roles.includes('admin');
      this.trainingsService.recommend(user.id).subscribe(r => {
      this.recommended = r;
        this.trainingsService.search(this.searchObj).subscribe(t => this.trainings = this.mapToRecommended(t));
      });
    });
    this.musclesService.search().subscribe(m => this.muscles = m);
  }

  addEx() {
    this.isTempShown = true;
    this.isAdding = true;
    this.tempEx = new Exercise();
  }

  saveEx() {
    this.isTempShown = false;
    if (this.isAdding) {
      this.training.exercises.push(this.tempEx);
    }
  }

  editEx(i: number) {
    this.isAdding = false;
    this.isTempShown = true;
    this.tempEx = this.training.exercises[i];
  }

  deleteEx(i: number) {
    this.training.exercises.splice(i, 1);
  }

  search() {
    this.searchObj.page = 0;
    this.stopScroll = false;
    // tslint:disable-next-line: radix
    this.searchObj.muscle = this.searchObj.muscle ? parseInt(this.searchObj.muscle.toString()) : 0;
    this.trainingsService.search(this.searchObj).subscribe(t => this.trainings = this.mapToRecommended(t));
  }

  private mapToRecommended(trainings: Training[]): Training[] {
    trainings.forEach(
      el => {
        if (this.recommended.findIndex(f => f.id === el.id) !== -1) {
          el.isRecommended = true;
        } else {
          el.isRecommended = false;
        }
      }
    );
    return trainings;
  }

  onScroll() {
    if (this.stopScroll) {
      return;
    }

    this.searchObj.page++;
    this.trainingsService.search(this.searchObj).subscribe(t => {
      this.trainings.push(...this.mapToRecommended(t));
      if (t.length < this.searchObj.pageSize) {
        this.stopScroll = true;
      }
    });
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, { backdrop: 'static', keyboard: false });
  }

  save() {
    this.modalRef.hide();
    this.training.trainingRate = this.training.trainingRate / 7;
    this.training.muscles = [];
    this.trainingMuscles.forEach(el => {
      if (!this.training.muscles.find(k => k.muscleId === el.id)) {
        this.training.muscles.push({
          trainingId: this.training.id,
          muscleId: el.id,
        } as TrainingMuscle);
      }
    });
    this.trainingsService.post(this.training).subscribe();
    this.training = new Training();
  }

}
