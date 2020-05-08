import { Component, OnInit } from '@angular/core';
import { MudService } from 'src/app/services/mud.service';
import { IMud } from 'src/app/model/muds/IMud';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
	templateUrl: './my-muds.component.html',
	styleUrls: ['./my-muds.component.scss'],
})
export class MyMudsComponent implements OnInit {
	constructor(
		private mudService: MudService,
		private authService: AuthService,
		private router: Router,
		private snackbar: MatSnackBar
	) {}

	muds: IMud[] = [];

	async ngOnInit() {
		await this.loadMuds();
	}

	async loadMuds() {
		try {
			this.muds = await this.mudService.getForUserId(
				this.authService.user.id
			);
		} catch (err) {
			console.error('Error while fetching MUDs', err);
			this.snackbar.open('Fehler beim Laden der MUDs', 'OK', {
				duration: 10000,
			});
		}
	}

	editMud(mudId: string) {
		this.router.navigate(['/my-muds', mudId, 'races']);
	}

	async deleteMud(mudId: string) {
		try {
			await this.mudService.deleteMud(mudId);
			await this.loadMuds();
		} catch (err) {
			console.error('Error while deleting MUD', err);
			this.snackbar.open('MUD konnte nicht gelöscht werden', 'OK', {
				duration: 10000,
			});
		}
	}
}
