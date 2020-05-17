import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
	selector: 'mh-reset',
	templateUrl: './reset.component.html',
	styleUrls: ['./reset.component.scss'],
})
export class ResetComponent {
	constructor(private authService: AuthService, private router: Router) {}

	mail = new FormControl('', [ Validators.required, Validators.email ]);
	showSuccess = false;

	wasSent = false;

	async reset() {
		try {
			await this.authService.requestReset(this.mail.value);
			this.wasSent = true;
		} catch (err) {
			console.error('Error while sending password reset request', err);
		}
	}
}
