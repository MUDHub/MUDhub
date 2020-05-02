import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { IUser } from 'src/app/model/IUser';
import { IRegistrationRequest } from 'src/app/model/AuthDTO';

@Component({
	selector: 'mh-register',
	templateUrl: './register.component.html',
	styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
	constructor(private authService: AuthService) {}

	firstname = new FormControl();
	lastname = new FormControl();
	mail = new FormControl();
	password = new FormControl();

	register() {
		const user: IRegistrationRequest = {
			firstName: this.firstname.value,
			lastName: this.lastname.value,
			email: this.mail.value,
			password: this.password.value,
		};
		this.authService.register(user);
	}
}
