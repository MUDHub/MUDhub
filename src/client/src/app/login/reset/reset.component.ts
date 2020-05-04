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

	async reset() {
		await this.authService.reset(this.mail.value);
		this.router.navigate(['/login']);
	}
}
