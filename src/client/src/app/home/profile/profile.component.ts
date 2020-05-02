import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { IUser } from 'src/app/model/IUser';

@Component({
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

	constructor(private auth: AuthService) { }

	user: IUser = this.auth.user;

	ngOnInit(): void {

	}

}
