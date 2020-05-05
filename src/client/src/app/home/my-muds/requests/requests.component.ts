import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MudJoinState } from 'src/app/model/MudDTO';
import { MudService } from 'src/app/services/mud.service';
import { IMudRequest } from 'src/app/model/IMudRequest';

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

		console.log({ requests: this.requests, accepted: this.accepted, rejected: this.rejected });
	}


	async allow(userId: string) {

	}

	async block(userId: string) {

	}
}
