import { Component, OnInit, TemplateRef } from '@angular/core';
import { TrainingsService } from '../../services/trainings.service';
import { Training } from '../../models/training';
import { UserTrainingsService } from '../../services/user-trainings.service';
import { User } from '../../models/user';
import { UserTraining } from '../../models/user-training';
import { UsersService } from '../../services/users.service';
import { ActivatedRoute } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { MusclesService } from '../../services/muscles.service';
import { Muscle } from '../../models/muscle';
import { TrainingMuscle } from '../../models/training-muscle';
import { Exercise } from '../../models/exercise';

@Component({
  templateUrl: './training.component.html',
  styleUrls: ['./training.component.sass']
})
export class TrainingComponent implements OnInit {
  rateHover: number = 0;
  id: number;
  training: Training;
  user: User;
  isAdmin = false;
  userTraining: UserTraining;
  modalRef: BsModalRef;
  tempEx: Exercise;
  isTempShown = false;
  isAdding = false;
  startedByUser = false;
  muscles: Muscle[];
  trainingMuscles: Muscle[];
  constructor(private readonly trainingsService: TrainingsService,
    private readonly userTrainingsService: UserTrainingsService,
    private readonly userService: UsersService,
    private readonly musclesService: MusclesService,
    private modalService: BsModalService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.id = +params['id'];
      this.trainingsService.getById(this.id).subscribe(training => this.training = training);
      this.userService.current().subscribe(user => {
        this.user = user;
        this.isAdmin = user.roles.includes('admin');
        if (this.isAdmin) {
          this.musclesService.search().subscribe(m => this.muscles = m);
        }
        this.userTraining = this.user.userTrainings.find(el => el.trainingId === this.id);
        this.startedByUser = this.userTraining ? true : false;
      });
   });
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

  start() {
    this.userTrainingsService.post({userId: this.user.id, trainingId: this.id} as UserTraining).subscribe(el => {
      this.userTraining = el;
      this.startedByUser = true;
    });
  }

  calculate() {
    this.userTrainingsService.calculate({userId: this.user.id, trainingId: this.id} as UserTraining).subscribe(el => {
      this.userTraining = el;
    });
  }

  round(n: number) {
    return Math.round(n);
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, { backdrop: 'static', keyboard: false });
  }

  rate(rating: number) {
    this.userTraining.score = rating;
    this.modalRef.hide();
    this.userTrainingsService.put(this.userTraining).subscribe();
  }

  max(a: number, b: number) {
    return a > b ? a : b;
  }

  save() {
    this.modalRef.hide();
    this.training.trainingRate = this.training.trainingRate / 7;

    if (this.trainingMuscles) {
      this.training.muscles = this.training.muscles.filter(el => this.trainingMuscles.findIndex(k => k.id === el.muscleId) !== -1);
      this.trainingMuscles.forEach(el => {
        if (!this.training.muscles.find(k => k.muscleId === el.id)) {
          this.training.muscles.push({
            trainingId: this.training.id,
            muscleId: el.id,
          } as TrainingMuscle);
        }
      });
    }
    this.trainingsService.put(this.training).subscribe();
  }

  delete() {
    this.trainingsService.delete(this.training.id).subscribe();
  }
}
