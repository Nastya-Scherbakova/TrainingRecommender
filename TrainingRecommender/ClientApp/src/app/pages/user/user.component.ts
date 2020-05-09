import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { User } from '../../models/user';
import { Disease } from '../../models/disease';
import { UserDisease } from '../../models/user-disease';
import { DiseasesService } from '../../services/diseases.service';

@Component({
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.sass']
})
export class UserComponent implements OnInit {
  private readonly numberFields = ['activity', 'figureType', 'goal'];

  editMode = false;
  user: User;
  diseases: Disease[];
  rate: number;
  ratePer: string;
  userDiseases: Disease[] = [];
  constructor(private readonly userService: UsersService,
    private readonly diseasesService: DiseasesService) { }

  ngOnInit() {
    this.diseasesService.search().subscribe(d => this.diseases = d);
    this.userService.current().subscribe(user => this.user = user);
  }

  public save() {
    this.processValues();
    this.editMode = false;
    this.userService.put(this.user).subscribe();
  }

  private processValues() {
    this.transformRate();
    this.user.gender = this.user.gender ? 1 : 0;
    this.numberFields.forEach(el => {
      // tslint:disable-next-line: radix
      this.user[el] = parseInt(this.user[el]) || 0;
    });
    this.user.userDiseases = this.user.userDiseases.filter(el => this.userDiseases.findIndex(k => k.id === el.diseaseId) !== -1);
    this.userDiseases.forEach(el => {
      if (!this.user.userDiseases.find(k => k.diseaseId === el.id)) {
        this.user.userDiseases.push({
          userId: this.user.id,
          diseaseId: el.id,
        } as UserDisease);
      }
    });
  }

  private transformRate() {
    if (this.rate) {
      switch (this.ratePer) {
        case '0': {
          this.user.trainingRate = this.rate / 7;
          break;
        }
        case '1': {
          this.user.trainingRate = this.rate / 30;
          break;
        }
        case '0': {
          this.user.trainingRate = this.rate / 365;
          break;
        }
      }
    }
  }

}
