import { Component } from '@angular/core';
import { FormBuilder, Validators, FormControl } from '@angular/forms';
import { MudService } from 'src/app/services/mud.service';
import { IMudCreateRequest } from 'src/app/model/MudDTO';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
	templateUrl: './mud-create.component.html',
	styleUrls: ['./mud-create.component.scss'],
})
export class MudCreateComponent {
	constructor(
		private fb: FormBuilder,
		private mud: MudService,
		private router: Router,
		private snackbar: MatSnackBar
	) {}

	isLoading = false;

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
			const res = await this.mud.create(mud);
			this.router.navigate(['/my-muds', res.mudId, 'races']);
		} catch (err) {
			this.snackbar.open('Fehler beim Erstellen des MUDS', 'OK', {
				duration: 10000,
			});
		} finally {
			this.isLoading = false;
		}
	}
}
