import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
	selector: 'mh-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
	constructor(private authService: AuthService) {}

	mail = new FormControl();
	password = new FormControl();

	login() {
		this.authService.login(this.mail.value, this.password.value);
	}
}
