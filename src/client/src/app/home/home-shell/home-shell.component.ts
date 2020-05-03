import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { IUser } from 'src/app/model/IUser';

@Component({
	templateUrl: './home-shell.component.html',
	styleUrls: ['./home-shell.component.scss'],
})
export class HomeShellComponent implements OnInit {
	constructor(private auth: AuthService, private router: Router) { }

	get name() {
		return this.auth.user.firstName + ' ' + this.auth.user.lastName;
	}

	user = this.auth.user;

	ngOnInit(): void { }

	logout() {
		this.auth.logout();
		this.router.navigate(['/login']);
	}
}
