import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
	templateUrl: './races.component.html',
	styleUrls: ['./races.component.scss'],
})
export class RacesComponent implements OnInit {
	constructor(
		private fb: FormBuilder,
		private route: ActivatedRoute,
		private router: Router
	) {}

	form = this.fb.group({
		name: ['', Validators.required],
		description: ['', Validators.required],
		imagekey: [''],
	});

	dialog =  false;
	mudId: string;

	//Todo Interface muss implementiert werden
	races: Array<{name: string; description: string; imagekey: string }> = [];

	ngOnInit(): void {
		/* Daten fetchen und in Array laden */

		this.mudId = this.route.snapshot.params.mudid;
	}

	changeDialog() {
		this.form.reset();
		this.dialog = !this.dialog;
	}

	onAbort() {
		this.router.navigate(['/my-muds']);
	}

	addRace() {
		this.races.push({
			name: this.form.get('name').value,
			description: this.form.get('description').value,
			imagekey: this.form.get('imagekey').value,
		});

		this.changeDialog();
	}

	deleteRow(index: number){
		this.races.splice(index, 1);
	}

	async onSubmit() {
		/* Object erstellen */
		/* Request zur API schicken */

		//Redirect zur nächsten Konfigurationsseite
		this.router.navigate(['/my-muds/' + this.mudId + '/classes']);
	}
}
