import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { ImageService } from 'src/app/services/image.service';
import { IMudRace } from 'src/app/model/muds/MudSetupDTO';
import { MudService } from 'src/app/services/mud.service';
import { IMudRaceResponse, IMudRaceRequest } from 'src/app/model/muds/MudDTO';
import { IImageUploadResponse } from 'src/app/model/FileUploadDTO';

@Component({
	templateUrl: './races.component.html',
	styleUrls: ['./races.component.scss'],
})
export class RacesComponent implements OnInit {
	constructor(
		private fb: FormBuilder,
		private route: ActivatedRoute,
		private router: Router,
		private imageService: ImageService,
		private mudService: MudService
	) {}

	form = this.fb.group({
		name: ['', Validators.required],
		description: ['', Validators.required],
		imageKey: [''],
	});

	dialog = false;
	mudId: string;
	selectedFile: File = null;

	races: Array<IMudRace> = [];

	async ngOnInit() {
		/* Daten fetchen und in Array laden */
		this.mudId = this.route.snapshot.params.mudid;
		this.races = await this.mudService.getRaceForMud(this.mudId);
	}

	changeDialog() {
		this.form.reset();
		this.dialog = !this.dialog;
	}

	async addRace() {
		// Get Imagekey from API if an Image was uploaded
		let imageKey: IImageUploadResponse = null;

		try {
			if (this.selectedFile != null) {
				imageKey = await this.imageService.uploadFile(
					this.selectedFile
				);
			}
		} catch (e) {
			this.selectedFile = null;
		}

		// Make API request
		const response: IMudRaceResponse = await this.mudService.addRace(
			this.mudId,
			{
				name: this.form.get('name').value,
				description: this.form.get('description').value,
				imageKey: imageKey?.imageUrl,
				mudId: this.mudId
			}
		);

		// Push races Object to the array
		this.races.push({
			description: response.race.description,
			name: response.race.name,
			raceId: response.race.raceId,
			imageKey: response.race.imageKey,
		});

		// Reset File Buffer
		this.selectedFile = null;
		this.changeDialog();
	}

	onFileSelected(event) {
		this.selectedFile = event.target.files[0] as File;
	}

	async deleteRow(index: number) {
		await this.mudService.deleteRace(this.mudId, this.races[index].raceId);
		this.races.splice(index, 1);
	}
}
