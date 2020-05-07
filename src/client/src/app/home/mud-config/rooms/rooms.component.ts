import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
	selector: 'mh-rooms',
	templateUrl: './rooms.component.html',
	styleUrls: ['./rooms.component.scss'],
})
export class RoomsComponent implements OnInit {
	constructor(private route: ActivatedRoute, private router: Router) {}

	mudId: string;

	ngOnInit(): void {
		this.mudId = this.route.snapshot.params.mudid;
	}

	onAbort() {
		this.router.navigate(['/my-muds']);
	}

	onLast() {
		this.router.navigate(['/my-muds/' + this.mudId + '/items']);
	}

	async onSubmit() {
		/* Object erstellen */
		/* Request zur API schicken */

		// Redirect zur nächsten Konfigurationsseite
		this.router.navigate(['/my-muds/' + this.mudId + '/finish']);
	}
}
