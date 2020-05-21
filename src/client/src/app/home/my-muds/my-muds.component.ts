import { Component, OnInit } from '@angular/core';
import { MudService } from 'src/app/services/mud.service';
import { IMud } from 'src/app/model/muds/IMud';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import Swal from 'sweetalert2';

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

	async editMud(mudId: string) {
		await this.mudService.edit(mudId, true);
		await this.router.navigate(['/my-muds', mudId, 'races']);
	}

	async startMud(mud: IMud) {
		try {
			await this.mudService.start(mud.mudId);
			mud.isRunning = true;
		} catch (err) {
			console.error('Error while starting mud', err);
			if (err.status === 400) {
				await Swal.fire({
					icon: 'error',
					title: 'Fehler',
					text: 'MUD kann nicht gestartet, checken Sie die Konfiguration oder schließen Sie diese ab'
				});
			}
		}
	}

	async stopMud(mud: IMud) {
		try {
			await this.mudService.stop(mud.mudId);
			mud.isRunning = false;
		} catch (err) {
			console.error('Error while stopping mud', err);
		}
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
