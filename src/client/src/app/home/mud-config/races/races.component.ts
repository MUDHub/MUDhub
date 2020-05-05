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

	dialog: true;
	mudId: string;

	//Todo Interface muss implementiert werden
	races: Array<{ name: string; description: string; imagekey: string }> = [];

	ngOnInit(): void {
		this.mudId = this.route.snapshot.params.mudid;
	}

	onAbort() {
		this.router.navigate(['/my-muds/create']);
	}

	onLast() {
		this.router.navigate(['/my-muds/create']);
	}

	addRace() {
		this.races.push({
			name: this.form.get('name').value,
			description: this.form.get('description').value,
			imagekey: this.form.get('imagekey').value,
		});
	}

	async onSubmit() {
		/* Object erstellen */
		/* Request zur API schicken */

		//Redirect zur nächsten Konfigurationsseite
		this.router.navigate(['/my-muds/' + this.mudId + '/classes']);
	}
}
