import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
	selector: 'mh-register',
	templateUrl: './register.component.html',
	styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
	constructor(private authService: AuthService) { }

	firstname = new FormControl();
	lastname = new FormControl();
	mail = new FormControl();
	password = new FormControl();

	register() {
		this.authService.register(this.firstname.value, this.lastname.value, this.mail.value, this.password.value);
	}

	ngOnInit(): void { }
}
