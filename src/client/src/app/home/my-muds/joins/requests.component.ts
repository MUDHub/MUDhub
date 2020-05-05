import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IRequestResponse } from 'src/app/model/MudDTO';
import { MudService } from 'src/app/services/mud.service';

@Component({
	templateUrl: './requests.component.html',
	styleUrls: ['./requests.component.scss'],
})
export class RequestsComponent implements OnInit {
	constructor(private route: ActivatedRoute, private mudService: MudService) {}

	mudId: string;
	requests: IRequestResponse[];

	async ngOnInit() {
		this.mudId = this.route.snapshot.params.mudid;
		this.requests = await this.mudService.getJoinRequests(this.mudId);

		console.log(this.requests);
	}
}
