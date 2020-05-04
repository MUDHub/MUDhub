import { Component, OnInit } from '@angular/core';
import { MudService } from 'src/app/services/mud.service';
import { IMud } from 'src/app/model/IMud';
import { AuthService } from 'src/app/services/auth.service';

@Component({
	templateUrl: './my-muds.component.html',
	styleUrls: ['./my-muds.component.scss'],
})
export class MyMudsComponent implements OnInit {
	constructor(
		private mudService: MudService,
		private authService: AuthService
	) {}

	muds: IMud[] = [
		{
			mudId: 'test',
			name: 'testname',
			description: 'desc',
			isPublic: 'false',
			autoRestart: true,
			owner: this.authService.user,
		},
		{
			mudId: 'test',
			name: 'testname',
			description: 'desc',
			isPublic: 'true',
			autoRestart: false,
			owner: this.authService.user,
		},
		{
			mudId: 'test',
			name: 'testname',
			description: 'desc',
			isPublic: 'true',
			autoRestart: true,
			owner: this.authService.user,
		},
	];

	async ngOnInit() {
		await this.loadMuds();
	}

	async loadMuds() {
		this.muds = await this.mudService.getForUserId(
			this.authService.user.id
		);
	}

	editMud(mudId: string) {
		//Todo redirect to edit page
		console.log('Edit Mud' + mudId);
	}

	async deleteMud(mudId: string) {
		try {
			await this.mudService.deleteMud(mudId);
			await this.loadMuds();
		} catch (err) {
			// TODO: Error handling
		}
	}
}
