import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
	templateUrl: './general.component.html',
	styleUrls: ['./general.component.scss'],
})
export class GeneralComponent implements OnInit {
	constructor(private fb: FormBuilder) {}

	createForm = this.fb.group({
		name: ['', Validators.required],
		description: ['', Validators.required],
		public: [true],
	});

	get name() {
		return this.createForm.get('name');
	}
	get description() {
		return this.createForm.get('description');
	}
	get public() {
		return this.createForm.get('public');
	}

	ngOnInit(): void {}

	onSubmit() {
		console.log(this.createForm.value);
	}
}
