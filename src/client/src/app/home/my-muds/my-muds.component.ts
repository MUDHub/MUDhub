import { Component, OnInit } from '@angular/core';
import { MudService } from 'src/app/services/mud.service';
import { IMud } from 'src/app/model/IMud';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
	templateUrl: './my-muds.component.html',
	styleUrls: ['./my-muds.component.scss'],
})
export class MyMudsComponent implements OnInit {
	constructor(
		private mudService: MudService,
		private authService: AuthService,
		private router: Router,
	) {}

	muds: IMud[] = [];

	async ngOnInit() {
		await this.loadMuds();
	}

	async loadMuds() {
		this.muds = await this.mudService.getForUserId(
			this.authService.user.id
		);
	}

	editMud(mudId: string) {
		this.router.navigate(['/my-muds/' + mudId + '/races']);
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
