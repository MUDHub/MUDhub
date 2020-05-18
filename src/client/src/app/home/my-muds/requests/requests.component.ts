import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MudJoinState } from 'src/app/model/muds/MudDTO';
import { MudService } from 'src/app/services/mud.service';
import { IMudRequest } from 'src/app/model/muds/IMudRequest';

@Component({
	templateUrl: './requests.component.html',
	styleUrls: ['./requests.component.scss'],
})
export class RequestsComponent implements OnInit {
	constructor(private route: ActivatedRoute, private mudService: MudService) {}

	mudId: string;
	requests: IMudRequest[];

	accepted: IMudRequest[];
	rejected: IMudRequest[];

	async ngOnInit() {
		this.mudId = this.route.snapshot.params.mudid;
		const requests = await await this.mudService.getJoinRequests(this.mudId);
		this.requests = requests.filter(r => r.state === MudJoinState.Requested);
		this.accepted = requests.filter(r => r.state === MudJoinState.Accepted);
		this.rejected = requests.filter(r => r.state === MudJoinState.Rejected);
	}


	async allow(userId: string) {
		try {
			await this.mudService.setRequestState(this.mudId, userId, MudJoinState.Accepted);
			const requests = this.requests.splice(this.requests.findIndex(r => r.userId === userId), 1);
			const rejected = this.rejected.splice(this.rejected.findIndex(r => r.userId === userId), 1);
			this.accepted.push(requests[0] || rejected[0]);
		} catch (err) {
			console.error('Error while approving request', err);
		}
	}

	async block(userId: string) {
		try {
			await this.mudService.setRequestState(this.mudId, userId, MudJoinState.Rejected);
			const requests = this.requests.splice(this.requests.findIndex(r => r.userId === userId), 1);
			const accepted = this.accepted.splice(this.accepted.findIndex(r => r.userId === userId), 1);
			this.rejected.push(requests[0] || accepted[0]);
		} catch (err) {
			console.error('Error while blocking request', err);
		}
	}
}
