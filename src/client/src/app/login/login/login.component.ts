import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
	selector: 'mh-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
	constructor(private authService: AuthService, private router: Router) {}

	mail = new FormControl();
	password = new FormControl();

	async login() {
		const success = await this.authService.login(this.mail.value, this.password.value);
		console.log('success:', success);

		if (success) {
			this.router.navigate(['']);
		}
	}
}
