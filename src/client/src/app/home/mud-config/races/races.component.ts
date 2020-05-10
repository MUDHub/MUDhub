import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { ImageService } from 'src/app/services/image.service';
import { IMudRace } from 'src/app/model/muds/MudSetupDTO';
import { MudService } from 'src/app/services/mud.service';

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
		imagekey: [''],
	});

	dialog = false;
	mudId: string;
	selectedFile: File = null;

	//Todo Interface muss implementiert werden
	races: Array<IMudRace> = [];

	ngOnInit(): void {
		/* Daten fetchen und in Array laden */
		this.mudId = this.route.snapshot.params.mudid;
	}

	changeDialog() {
		this.form.reset();
		this.dialog = !this.dialog;
	}

	async addRace() {
		// Get Imagekey from API if an Image was uploaded
		let imageKey = null;

		try {
			if (this.selectedFile != null) {
				imageKey = await this.imageService.uploadFile(
					this.selectedFile
				);
			}
		} catch (e) {
			this.selectedFile = null;
		}

		// Push races Object to the array
		this.races.push({
			raceId: (this.races.length - 1).toString(),
			name: this.form.get('name').value,
			description: this.form.get('description').value,
			imagekey: imageKey,
		});

		this.mudService.addMudRace(
			this.mudId,
			this.races[this.races.length - 1]
		);

		// Reset File Buffer
		this.selectedFile = null;
		this.changeDialog();
	}

	onFileSelected(event) {
		this.selectedFile = event.target.files[0] as File;
	}

	deleteRow(index: number) {
		this.mudService.deleteMudRace(this.mudId, this.races[index]);
		this.races.splice(index, 1);
	}
}
