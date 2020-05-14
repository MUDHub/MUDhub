import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IValidationResult, IRegion } from 'src/app/model/muds/MudSetupDTO';
import { MudService } from 'src/app/services/mud.service';
import {MatCardModule} from '@angular/material/card';

@Component({
	selector: 'mh-finish',
	templateUrl: './finish.component.html',
	styleUrls: ['./finish.component.scss'],
})
export class FinishComponent implements OnInit {
	constructor(
		private route: ActivatedRoute,
		private router: Router,
		private mudService: MudService
	) {}

	panelOpenState: boolean = true;
	mudId: string;

	validationResult: IValidationResult;

	async ngOnInit() {
		this.mudId = this.route.snapshot.params.mudid;
		this.validationResult = await this.mudService.validateMudGame(this.mudId);
	}

	async onSubmit() {
		/* Object erstellen */
		/* Request zur API schicken */

		// Redirect zur MyMuds Seite - Configuration completed
		this.router.navigate(['/my-muds']);
	}
}
