import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IRoom } from 'src/app/model/areas/IRoom';
import { IArea } from 'src/app/model/areas/IArea';

@Component({
	selector: 'mh-rooms',
	templateUrl: './rooms.component.html',
	styleUrls: ['./rooms.component.scss'],
})
export class RoomsComponent implements OnInit {
	constructor(private route: ActivatedRoute, private router: Router) {}

	mudId: string;

	rooms: IRoom[][] = [[]];
	areas: IArea[] = [];

	ngOnInit(): void {
		this.mudId = this.route.snapshot.params.mudid;
		this.rooms = new Array(3).fill(new Array(3).fill(undefined));
		this.areas = [
			{
				mud: undefined,
				mudId: '',
				name: 'Etage 0',
				areaId: 'pw483zrf',
				description: 'Das Ergeschoss der DHBW Horb'
			}
		]
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

	addArea() {
		this.areas.push({
			areaId: '',
			mud: undefined,
			mudId: '',
			name: `Etage ${this.areas.length}`
		})
	}
}
