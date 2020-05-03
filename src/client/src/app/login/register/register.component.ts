import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { IRegistrationRequest } from 'src/app/model/AuthDTO';
import { PasswordErrorStateMatcher } from '../_helper/PasswordErrorStateMatcher';

@Component({
	selector: 'mh-register',
	templateUrl: './register.component.html',
	styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
	constructor(
		private fb: FormBuilder,
		private authService: AuthService,
		private router: Router
	) {}

	isError = false;

	matcher = new PasswordErrorStateMatcher();

	createForm = this.fb.group(
		{
			firstname: ['', Validators.required],
			lastname: ['', Validators.required],
			email: ['', [Validators.required, Validators.email]],
			password: ['', Validators.required],
			passwordRepeat: [''],
		},
		{ validator: this.checkPasswords }
	);

	async register() {
		const user: IRegistrationRequest = {
			firstname: this.createForm.get('firstname').value,
			lastname: this.createForm.get('lastname').value,
			email: this.createForm.get('email').value,
			password: this.createForm.get('password').value,
		};

		try {
			const registeredUser = await this.authService.register(user);
			this.router.navigate(['../login']);
		} catch (err) {
			console.error('Error while sending registration request', err);
			this.isError = true;
		}
	}

	checkPasswords(group: FormGroup) {
		const pass = group.get('password').value;
		const confirmPass = group.get('passwordRepeat').value;

		return pass === confirmPass ? null : { notSame: true };
	}
}
