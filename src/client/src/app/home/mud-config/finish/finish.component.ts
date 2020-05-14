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

	/* Handle Validation Result */
	raceStatus: boolean = true;
	raceStatusMsg: string = "";

	classStatus: boolean = true;
	classStatusMsg: string = "";

	itemStatus: boolean = true;
	itemStatusMsg: string = "";

	areaStatus: boolean = true;
	areaStatusMsg: string = "";
	
	generalStatus: boolean = true;
	generalStatusMsg: string = "";

	async handleValidation(){
		this.validationResult = await this.mudService.validateMudGame(this.mudId);
		//Validate Error Status
		this.validationResult.validationErrors.forEach(error => {
			(error.region == IRegion.General) ? this.generalStatus = true : this.generalStatus = false;
			(error.region == IRegion.Races) ? this.raceStatus = true : this.raceStatus = false;
			(error.region == IRegion.Classes) ? this.classStatus = true : this.classStatus = false;
			(error.region == IRegion.Items) ? this.itemStatus = true : this.itemStatus = false;
			(error.region == IRegion.Areas) ? this.areaStatus = true : this.areaStatus = false;
		});
		//Validate Error Status Message
		this.validationResult.validationErrors.forEach(error => {
			(error.region == IRegion.General) ? this.generalStatusMsg = error.message : this.generalStatusMsg = "";
			(error.region == IRegion.Races) ? this.raceStatusMsg = error.message : this.raceStatusMsg = "";
			(error.region == IRegion.Classes) ? this.classStatusMsg = error.message : this.classStatusMsg = "";
			(error.region == IRegion.Items) ? this.itemStatusMsg = error.message : this.itemStatusMsg = "";
			(error.region == IRegion.Areas) ? this.areaStatusMsg = error.message : this.areaStatusMsg = "";
		});
		//Get General Information
		this.generalStatus = !this.validationResult.isMudValid;
		this.generalStatusMsg = this.validationResult.errormessage;
		console.log(this.validationResult);
	}
	/* Handle Validation Result End */

	async ngOnInit() {
		this.mudId = this.route.snapshot.params.mudid;
		await this.handleValidation();
	}

	async onSubmit() {
		/* Object erstellen */
		/* Request zur API schicken */

		// Redirect zur MyMuds Seite - Configuration completed
		this.router.navigate(['/my-muds']);
	}
}
