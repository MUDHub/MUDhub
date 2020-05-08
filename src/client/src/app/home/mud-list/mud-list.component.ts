import { Component, OnInit } from '@angular/core';
import { IMud } from 'src/app/model/muds/IMud';
import { MudService } from 'src/app/services/mud.service';
import { AuthService } from 'src/app/services/auth.service';
import { IUser } from 'src/app/model/auth/IUser';
import { MudJoinState } from 'src/app/model/muds/MudDTO';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
	templateUrl: './mud-list.component.html',
	styleUrls: ['./mud-list.component.scss'],
})
export class MudListComponent implements OnInit {
	constructor(
		private mudService: MudService,
		private auth: AuthService,
		private snackbar: MatSnackBar
	) {}

	RequestState = MudJoinState;
	user: IUser = this.auth.user;

	list: { mud: IMud; requestState?: MudJoinState }[];

	async ngOnInit() {
		this.list = [];
		try {
			const muds = await this.mudService.getAll();
			for (const mud of muds) {
				if (!mud.isPublic) {
					const mudRequests = await this.mudService.getJoinRequests(
						mud.mudId
					);
					const request = mudRequests.find(
						m => m.userId === this.user.id
					);
					this.list.push({ mud, requestState: request?.state });
				} else {
					this.list.push({ mud });
				}
			}
		} catch (err) {
			console.error('Error while fetching muds', err);
			this.snackbar.open('Fehler beim Abrufen der MUDs', 'OK', {
				duration: 10000,
			});
		}
	}

	async requestAccess(mud: IMud) {
		try {
			await this.mudService.requestAccess(mud.mudId);
			const found = this.list.find(m => m.mud === mud)
			if (found) {
				found.requestState = MudJoinState.Requested;
			}
		} catch (err) {
			console.error('error in sending mud join request', err);
		}
	}
}
