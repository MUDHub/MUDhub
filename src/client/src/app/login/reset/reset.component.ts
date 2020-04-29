import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
	selector: 'mh-reset',
	templateUrl: './reset.component.html',
	styleUrls: ['./reset.component.scss'],
})
export class ResetComponent implements OnInit {
	constructor(private authService: AuthService) {}

	mail = new FormControl();

	reset(){
		this.authService.reset(this.mail.value);
	}

	ngOnInit(): void {}
}
