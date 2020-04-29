import { Component, OnInit } from '@angular/core';
import {FormGroup, FormControl} from '@angular/forms';
import {AuthService} from '../../services/auth.service';

@Component({
	selector: 'mh-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

	constructor(private authService: AuthService) {}

	mail= new FormControl();
	password= new FormControl();

	/**
	 * 
	 * @param mail 
	 * @param password 
	 */
	login(){
		this.authService.login(this.mail.value,this.password.value);
	}

	ngOnInit(): void {}
}
