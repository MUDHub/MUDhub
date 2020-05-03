import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { IRegistrationRequest } from 'src/app/model/AuthDTO';

@Component({
	selector: 'mh-register',
	templateUrl: './register.component.html',
	styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
	constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) { }

	createForm = this.fb.group({
		firstname: ['', Validators.required],
		lastname: ['', Validators.required],
		email: ['', Validators.required],
		password: ['', Validators.required],
		passwordRepeat: [''],
	}, { validator: this.checkPasswords });

	register() {

		const user: IRegistrationRequest = {
			firstName: this.createForm.get('firstname').value,
			lastName: this.createForm.get('lastname').value,
			email: this.createForm.get('email').value,
			password: this.createForm.get('password').value,
		};
		this.authService.register(user);
		this.router.navigate(['../login']);
	}

	checkPasswords(group: FormGroup) {
		const pass = group.get('password').value;
		const confirmPass = group.get('passwordRepeat').value;

		return pass === confirmPass ? null : { notSame: true };
	}
}
