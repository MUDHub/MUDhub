import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validator, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
	selector: 'mh-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
	constructor(
		private fb: FormBuilder,
		private authService: AuthService,
		private router: Router
	) {}

	error;

	form = this.fb.group({
		email: ['', [Validators.required, Validators.email]],
		password: ['', Validators.required],
	});

	isLoading = false;


	ngOnInit() {
		this.form.valueChanges.subscribe(() => {
			this.error = undefined;
		});
	}

	async login() {
		this.error = undefined;
		this.isLoading = true;
		try {
			await this.authService.login(
				this.form.get('email').value,
				this.form.get('password').value
			);
			this.router.navigate(['']);
		} catch (err) {
			console.error('Error while logging in', err);
			this.error = err;
		} finally {
			this.isLoading = false;
		}
	}
}
