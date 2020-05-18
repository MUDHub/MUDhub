import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { ImageService } from 'src/app/services/image.service';
import { IMudClass } from 'src/app/model/muds/MudSetupDTO';
import { MudService } from 'src/app/services/mud.service';
import { IMudClassResponse } from 'src/app/model/muds/MudDTO';
import { IImageUploadResponse } from 'src/app/model/FileUploadDTO';
import Swal from 'sweetalert2';

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

	error;
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
		this.form.valueChanges.subscribe(() => {
			this.error = undefined;
		});
	}

	changeDialog() {
		this.form.reset();
		this.dialog = !this.dialog;
		this.edit = false;
	}

	async addClass() {
		this.error = undefined;
		// Get Imagekey from API if an Image was uploaded
		let imageKey: IImageUploadResponse = null;

		try {
			if (this.selectedFile != null) {
				imageKey = await this.imageService.uploadFile(this.selectedFile);
			}
		} catch (e) {
			this.selectedFile = null;
		}
		// Make API post request if its a new value
		if (!this.edit) {
			try {
				const response: IMudClassResponse = await this.mudService.addClass(
					this.mudId,
					{
						name: this.form.get('name').value,
						description: this.form.get('description').value,
						imageKey: imageKey?.imageUrl,
						mudId: this.mudId,
					}
				);

				this.classes.push(response.class);
			} catch (err) {
				console.error('Error while adding new class', err);
				this.error = err;
			}
		} else {
			try {
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
					this.classes[this.index] = response.class;
				}
			} catch (err) {
				console.error('Error while editing class', err);
				this.error = err;
			}
		}

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
