import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MudService } from 'src/app/services/mud.service';
import { IMudCreateRequest } from 'src/app/model/MudDTO';

@Component({
	templateUrl: './general.component.html',
	styleUrls: ['./general.component.scss'],
})
export class GeneralComponent {
	constructor(private fb: FormBuilder, private mud: MudService) {}

	createForm = this.fb.group({
		name: ['', Validators.required],
		description: ['', Validators.required],
		public: [true],
		autoRestart: [false],
	});

	async onSubmit() {
		const mud: IMudCreateRequest = {
			name: this.createForm.get('name').value,
			description: this.createForm.get('description').value,
			isPublic: this.createForm.get('public').value,
			autoRestart: this.createForm.get('autoRestart').value,
		};

		console.log('sending:', mud);

		const response = await this.mud.create(mud);
		console.log('response:', response);
	}
}
