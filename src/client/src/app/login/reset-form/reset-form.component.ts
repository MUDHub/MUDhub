import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { PasswordErrorStateMatcher } from '../_helper/PasswordErrorStateMatcher';
import { ActivatedRoute } from '@angular/router';

@Component({
	selector: 'mh-reset-form',
	templateUrl: './reset-form.component.html',
	styleUrls: ['./reset-form.component.scss'],
})
export class ResetFormComponent implements OnInit {
	constructor(private fb: FormBuilder, private route: ActivatedRoute) {}

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
		console.log(this.resetKey);
	}

	checkPasswords(group: FormGroup) {
		const pass = group.get('password').value;
		const confirmPass = group.get('passwordRepeat').value;

		return pass === confirmPass ? null : { notSame: true };
	}

	onSubmit() {
		const newPassword = this.form.get('password').value;
		console.log('setting new password:', newPassword);
	}
}
