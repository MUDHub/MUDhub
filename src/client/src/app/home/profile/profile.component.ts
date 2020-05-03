import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { IUser } from 'src/app/model/IUser';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

	constructor(private auth: AuthService) { }

	userForm = new FormGroup({
		firstname: new FormControl('', Validators.required),
		lastname: new FormControl(''),
		email: new FormControl({ value: '', disabled: true }),
	});

	ngOnInit(): void {
		const user = this.auth.user;
		console.log(user);
		this.userForm.setValue({
			firstname: user.firstName,
			lastname: user.lastName,
			email: user.email
		});
	}


	onSubmit() {
		console.log(this.userForm.value);
	}
}
