import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../../api-authorization/authorize.service';
import { Observable } from 'rxjs';
import { UsersService } from '../../services/users.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.sass']
})
export class NavMenuComponent implements OnInit {
  public isAuthenticated: Observable<boolean>;
  isExpanded = false;
  isAdmin = false;

  constructor(private authorizeService: AuthorizeService,
    private readonly usersService: UsersService) {
  }

  ngOnInit() {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.isAuthenticated.subscribe((val) => {
      if (val) {
        this.usersService.current().subscribe(u => this.isAdmin = u.roles.includes('admin'));
      }
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
