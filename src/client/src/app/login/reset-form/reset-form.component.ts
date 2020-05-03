import { Component } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { PasswordErrorStateMatcher } from '../_helper/PasswordErrorStateMatcher';

@Component({
	selector: 'mh-reset-form',
	templateUrl: './reset-form.component.html',
	styleUrls: ['./reset-form.component.scss']
})
export class ResetFormComponent {

	constructor(private fb: FormBuilder) { }

	matcher = new PasswordErrorStateMatcher();

	form = this.fb.group({
		password: ['', Validators.required],
		passwordRepeat: [''],
	}, { validators: this.checkPasswords });

	checkPasswords(group: FormGroup) {
		const pass = group.get('password').value;
		const confirmPass = group.get('passwordRepeat').value;

		return pass === confirmPass ? null : { notSame: true };
	}

}
