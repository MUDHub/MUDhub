import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { ImageService } from 'src/app/services/image.service';
import { IMudClass } from 'src/app/model/muds/MudSetupDTO';
import { MudService } from 'src/app/services/mud.service';
import { IMudClassResponse } from 'src/app/model/muds/MudDTO';
import { IImageUploadResponse } from 'src/app/model/FileUploadDTO';

@Component({
	selector: 'mh-classes',
	templateUrl: './classes.component.html',
	styleUrls: ['./classes.component.scss'],
})
export class ClassesComponent implements OnInit {
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
	edit = false;
	mudId: string;
	index: number;
	selectedFile: File = null;

	// Todo Interface muss implementiert werden
	classes: Array<IMudClass> = [];

	async ngOnInit() {
		/* Daten fetchen und in Array laden */
		this.mudId = this.route.snapshot.params.mudid;
		this.classes = await this.mudService.getClassForMud(this.mudId);
	}

	changeDialog() {
		this.form.reset();
		this.dialog = !this.dialog;
		this.edit = false;
	}

	async addClass() {
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

		// Make API post request if its a new value
		if (!this.edit) {
			const response: IMudClassResponse = await this.mudService.addClass(
				this.mudId,
				{
					name: this.form.get('name').value,
					description: this.form.get('description').value,
					imageKey: imageKey?.imageUrl,
					mudId: this.mudId,
				}
			);

			// Push races Object to the array
			if (response.succeeded) {
				this.classes.push({
					description: response.class.description,
					name: response.class.name,
					classId: response.class.classId,
					imageKey: response.class.imageKey,
				});
			}
		} else {
			// Make API put request if its an edit
			const response: IMudClassResponse = await this.mudService.editClass(
				this.classes[this.index].classId,
				{
					name: this.form.get('name').value,
					description: this.form.get('description').value,
					imageKey: imageKey?.imageUrl,
					mudId: this.mudId,
				}
			);

			if (response.succeeded) {
				this.classes[this.index] = {
					description: response.class.description,
					name: response.class.name,
					classId: response.class.classId,
					imageKey: response.class.imageKey,
				};
			}
		}

		// Reset File Buffer
		this.selectedFile = null;

		// Close Dialog
		this.changeDialog();
	}

	onFileSelected(event) {
		this.selectedFile = event.target.files[0] as File;
	}

	async deleteRow(index: number) {
		await this.mudService.deleteClass(
			this.mudId,
			this.classes[index].classId
		);
		this.classes.splice(index, 1);
	}

	editRow(index: number) {
		this.dialog = true;
		this.edit = true;
		this.form.get('name').setValue(this.classes[index].name);
		this.form.get('description').setValue(this.classes[index].description);
		this.form.get('imagekey').setValue(this.classes[index].imageKey);

		this.index = index;
	}
}
