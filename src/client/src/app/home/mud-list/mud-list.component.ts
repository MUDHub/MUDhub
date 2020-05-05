import { Component, OnInit } from '@angular/core';
import { IMud } from 'src/app/model/IMud';
import { MudService } from 'src/app/services/mud.service';
import { AuthService } from 'src/app/services/auth.service';
import { IUser } from 'src/app/model/IUser';
import { MudJoinState } from 'src/app/model/MudDTO';

@Component({
	templateUrl: './mud-list.component.html',
	styleUrls: ['./mud-list.component.scss'],
})
export class MudListComponent implements OnInit {
	constructor(private mudService: MudService, private auth: AuthService) {}

	RequestState = MudJoinState;
	user: IUser = this.auth.user;

	list: { mud: IMud; requestState?: MudJoinState }[];

	async ngOnInit() {
		this.list = [];
		const muds = await this.mudService.getAll();
		for (const mud of muds) {
			if (!mud.isPublic) {
				const mudRequests = await this.mudService.getJoinRequests(mud.mudId);
				const request = mudRequests.find(m => m.userId === this.user.id);
				this.list.push({ mud, requestState: request.state });
			} else {
				this.list.push({ mud });
			}
		}
	}

	async requestAccess(mud: IMud) {
		try {
			await this.mudService.requestAccess(mud.mudId);
		} catch (err) {
			console.error('error in sending mud join request', err);
		}
	}
}
