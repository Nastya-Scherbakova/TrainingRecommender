import { Component, OnInit, TemplateRef } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { MusclesService } from '../../services/muscles.service';
import { DiseasesService } from '../../services/diseases.service';
import { PaginationBase } from '../../models/search-models';
import { IService } from '../../services/iservice';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Disease } from '../../models/disease';
import { UserDisease } from '../../models/user-disease';
import { TrainingsService } from '../../services/trainings.service';
import { Training } from '../../models/training';

@Component({
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.sass']
})
export class AdminComponent implements OnInit {
  private readonly numberFields = ['activity', 'figureType', 'goal'];
  tabs = ['Користувачі', `Групи м'язів`, 'Захворювання'];
  tabsFields = [ ['name', 'surname'], ['name'], ['name'] ];
  services: IService<any>[];
  stopScroll = false;
  menuTab = 0;
  obj: any;
  data = [];
  errText: string;
  diseases: Disease[];
  userDiseases: Disease[];
  paging = new PaginationBase();
  modalRef: BsModalRef;
  rate: number;
  ratePer: string;
  isEdit = false;
  recommended: Training[];
  constructor(private readonly usersService: UsersService,
    private readonly musclesService: MusclesService,
    private readonly trainingsServie: TrainingsService,
    private modalService: BsModalService,
    private readonly diseasesService: DiseasesService) {
      this.services = [usersService, musclesService, diseasesService];
  }

  ngOnInit() {
    this.diseasesService.search().subscribe(s => this.diseases = s);
  }

  switchTab(index: number) {
    this.paging.page = 0;
    this.stopScroll = false;
    this.menuTab = index;
    this.services[index].search(this.paging).subscribe(data => this.data = data);
  }

  onScroll() {
    if (this.stopScroll) {
      return;
    }
    this.paging.page++;
    this.services[this.menuTab].search(this.paging).subscribe(data => {
      this.data.push(...data);
      if (this.data.length < this.paging.pageSize) {
        this.stopScroll = true;
      }
    });
  }

  getUserRecommended(id: string, template: TemplateRef<any>) {
    this.trainingsServie.recommend(id).subscribe(r => {
      this.recommended = r;
      this.errText = '';
    }, err => {
      this.errText = err.error; this.recommended = [];
    });
    this.openModal(template);
  }

  add(template: TemplateRef<any>) {
    this.isEdit = false;
    this.obj = {};
    this.openModal(template);
  }

  edit(index: number, template: TemplateRef<any>) {
    this.isEdit = true;
    this.obj = this.data[index];
    this.openModal(template);
  }

  delete(index: number) {
    this.services[this.menuTab].delete(this.data[index].id).subscribe(() => this.data.splice(index, 1));
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, { backdrop: 'static', keyboard: false });
  }

  save() {
    if (this.menuTab === 0) {
      this.processValues();
    }
    if (this.isEdit) {
      this.services[this.menuTab].put(this.obj).subscribe();
    } else {
      this.services[this.menuTab].post(this.obj).subscribe();
    }
    this.modalRef.hide();
  }

  private processValues() {
    this.transformRate();
    if (this.obj.roles) {
      this.obj.roles = [this.obj.roles];
    }
    this.obj.gender = this.obj.gender ? 1 : 0;
    this.numberFields.forEach(el => {
      // tslint:disable-next-line: radix
      this.obj[el] = parseInt(this.obj[el]) || 0;
    });
    this.obj.userDiseases = this.obj.userDiseases.filter(el => this.userDiseases.findIndex(k => k.id === el.diseaseId) !== -1);
    this.userDiseases.forEach(el => {
      if (!this.obj.userDiseases.find(k => k.diseaseId === el.id)) {
        this.obj.userDiseases.push({
          userId: this.obj.id,
          diseaseId: el.id,
        } as UserDisease);
      }
    });
  }

  private transformRate() {
    if (this.rate) {
      switch (this.ratePer) {
        case '0': {
          this.obj.trainingRate = this.rate / 7;
          break;
        }
        case '1': {
          this.obj.trainingRate = this.rate / 30;
          break;
        }
        case '0': {
          this.obj.trainingRate = this.rate / 365;
          break;
        }
      }
    }
  }
}
