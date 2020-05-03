import { Component } from '@angular/core';
import { FormBuilder, Validator, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
	selector: 'mh-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
	constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) { }

	form = this.fb.group({
		email: ['', Validators.required],
		password: ['', Validators.required],
	});

	isLoading = false;

	async login() {
		this.isLoading = true;
		const success = await this.authService.login(this.form.get('email').value, this.form.get('password').value);
		this.isLoading = false;

		console.log('success:', success);

		if (success) {
			this.router.navigate(['']);
		}
	}
}
