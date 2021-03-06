import { Component } from '@angular/core';
import { FormBuilder, Validators, FormControl } from '@angular/forms';
import { MudService } from 'src/app/services/mud.service';
import { IMudCreateRequest } from 'src/app/model/muds/MudDTO';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ImageService } from 'src/app/services/image.service';

@Component({
	templateUrl: './mud-create.component.html',
	styleUrls: ['./mud-create.component.scss'],
})
export class MudCreateComponent {
	constructor(
		private fb: FormBuilder,
		private mud: MudService,
		private router: Router,
		private snackbar: MatSnackBar,
		private imageService: ImageService
	) {}

	isLoading = false;

	selectedFile: File = undefined;
	createForm = this.fb.group({
		name: ['', [Validators.required, Validators.minLength(4)]],
		description: ['', Validators.required],
		public: true,
		autoRestart: false,
	});

	async onSubmit() {
		const mud: IMudCreateRequest = {
			name: this.createForm.get('name').value,
			description: this.createForm.get('description').value,
			isPublic: this.createForm.get('public').value,
			autoRestart: this.createForm.get('autoRestart').value,
		};

		this.isLoading = true;

		try {
			if (this.selectedFile) {
				const response = await this.imageService.uploadFile(this.selectedFile);
				mud.imagekey = response.imageUrl;
			}
		} catch (err) {
			console.error('Error while uploading image', err);
		}

		try {
			const res = await this.mud.create(mud);
			this.router.navigate(['/my-muds', res.mudId, 'races']);
		} catch (err) {
			console.error('Error while creating MUD', err);
			this.snackbar.open('Fehler beim Erstellen des MUDS', 'OK', {
				duration: 10000,
			});
		} finally {
			this.isLoading = false;
		}
	}

	onSelectedFileChange(event) {
		this.selectedFile = event.target.files[0] as File;
	}
}
