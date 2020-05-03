import { Component } from '@angular/core';
import { FormBuilder, Validators, FormControl } from '@angular/forms';
import { MudService } from 'src/app/services/mud.service';
import { IMudCreateRequest } from 'src/app/model/MudDTO';
import { Router } from '@angular/router';

@Component({
	templateUrl: './general.component.html',
	styleUrls: ['./general.component.scss'],
})
export class GeneralComponent {
	constructor(private fb: FormBuilder, private mud: MudService, private router: Router) {}

	createForm = this.fb.group({
		name: ['', [Validators.required, Validators.minLength(4)]],
		description: ['', Validators.required],
		public: { value: true, disabled: true },
		autoRestart: { value: false, disabled: true },
	});

	async onSubmit() {
		const mud: IMudCreateRequest = {
			name: this.createForm.get('name').value,
			description: this.createForm.get('description').value,
			isPublic: this.createForm.get('public').value,
			autoRestart: this.createForm.get('autoRestart').value,
		};

		try {
			await this.mud.create(mud);
			this.router.navigate(['/my-muds']);
		} catch (err) {
			// TODO: Error handling
		}

	}
}
