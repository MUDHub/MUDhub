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

	rooms: IRoom[][];

	ngOnInit(): void {
		this.mudId = this.route.snapshot.params.mudid;
		this.rooms = [
			[undefined, undefined, undefined],
			[undefined, undefined, undefined],
			[undefined, undefined, undefined],
		];
	}

	get width() {
		return this.rooms[0]?.length;
	}

	set width(value: number) {
		if (value > 0) {
			if (value > this.width) {
				const width = this.width;
				for (const row of this.rooms) {
					while (value > row.length) {
						row.push(undefined);
					}
				}
			} else if (value < this.width) {
				for (const row of this.rooms) {
					row.splice(value - 1, 1);
				}
			}
		}
	}

	get height() {
		return this.rooms.length;
	}

	set height(value: number) {
		if (value > 0) {
			if (value > this.height) {
				while (value > this.rooms.length) {
					this.rooms.push(new Array(this.width).fill(undefined));
				}
			} else if (value < this.height) {
				this.rooms.splice(value - 1, 1);
			}
		}
	}

	onAbort() {
		this.router.navigate(['/my-muds']);
	}

	onLast() {
		this.router.navigate(['/my-muds/' + this.mudId + '/items']);
	}

	async onNext() {
		/* Object erstellen */
		/* Request zur API schicken */

		// Redirect zur nächsten Konfigurationsseite
		this.router.navigate(['/my-muds/' + this.mudId + '/finish']);
	}

	createRoom(x: number, y: number) {
		this.rooms[y][x] = {
			name: 'Test Raum',
		};
	}
}


interface IRoom {
	name: string;
	description?: string;
}
