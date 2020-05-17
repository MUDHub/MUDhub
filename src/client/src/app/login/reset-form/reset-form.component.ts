import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { PasswordErrorStateMatcher } from '../_helper/PasswordErrorStateMatcher';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import Swal from 'sweetalert2';


@Component({
	selector: 'mh-reset-form',
	templateUrl: './reset-form.component.html',
	styleUrls: ['./reset-form.component.scss'],
})
export class ResetFormComponent implements OnInit {
	constructor(private fb: FormBuilder, private route: ActivatedRoute, private auth: AuthService, private router: Router) {}


	matcher = new PasswordErrorStateMatcher();

	resetKey: string;

	form = this.fb.group(
		{
			password: ['', Validators.required],
			passwordRepeat: ['', Validators.required],
		},
		{ validators: this.checkPasswords }
	);

	ngOnInit() {
		this.resetKey =
			this.route.snapshot.queryParams.key ||
			this.route.snapshot.queryParams.resetkey;
	}

	checkPasswords(group: FormGroup) {
		const pass = group.get('password').value;
		const confirmPass = group.get('passwordRepeat').value;

		return pass === confirmPass ? null : { notSame: true };
	}

	async onSubmit() {
		const newPassword = this.form.get('password').value;

    if (newPassword && this.resetKey) {
			try {
				await this.auth.resetPassword(this.resetKey, newPassword);
				this.router.navigate(['/login']);
			} catch (err) {
				console.error('Error while resetting password', err);
				await Swal.fire({
					icon: 'error',
					title: 'Fehler',
					text: 'Fehler beim Zurücksetzen des Passwortes'
				});
			}
		} else {
			await Swal.fire({
				icon: 'error',
				title: 'Fehler',
				text: 'Das Zurücksetzen des Passwortes konnte nicht durchgeführt werden, bitte rufen Sie das Formular nur von dem Link in der Email auf'
			});
		}
	}
}
