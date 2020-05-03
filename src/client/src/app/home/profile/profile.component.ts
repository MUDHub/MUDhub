import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { IUser } from 'src/app/model/IUser';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UsersService } from 'src/app/services/users.service';
import { findReadVarNames } from '@angular/compiler/src/output/output_ast';

@Component({
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

	constructor(private auth: AuthService, private user: UsersService) { }

	isLoading = false;

	userForm = new FormGroup({
		firstname: new FormControl('', Validators.required),
		lastname: new FormControl(''),
		email: new FormControl({ value: '', disabled: true }),
	});

	ngOnInit(): void {
		const user = this.auth.user;
		this.userForm.setValue({
			firstname: user.firstName,
			lastname: user.lastName,
			email: user.email
		});
	}


	async onSubmit() {
		try {
			this.isLoading = true;
			await this.user.update(this.auth.user.id, {
				firstname: this.userForm.get('firstname').value,
				lastname: this.userForm.get('lastname').value
			});
		} catch (err) {
			console.error('Error while updating user', err);
		} finally {
			this.isLoading = false;
		}
	}
}
