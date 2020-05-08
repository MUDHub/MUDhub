import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IRoom } from 'src/app/model/areas/IRoom';
import { IArea } from 'src/app/model/areas/IArea';
import { AreaService } from 'src/app/services/area.service';

@Component({
	selector: 'mh-rooms',
	templateUrl: './rooms.component.html',
	styleUrls: ['./rooms.component.scss'],
})
export class RoomsComponent implements OnInit {
	constructor(private route: ActivatedRoute, private router: Router, private areaService: AreaService) {}

	mudId: string;

	rooms: IRoom[][] = [[]];
	areas: IArea[] = [];
	selectedArea: IArea;

	ngOnInit(): void {
		this.mudId = this.route.snapshot.params.mudid;
		this.rooms = new Array(3).fill(new Array(3).fill(undefined));
		this.areas = [];
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
		// TODO: show popup/dialog to get infos for room and then create it via API call
	}

	addArea() {
		// TODO: show popup/dialog to get infos for area and then create it via API call
		// TODO: handle return object
		this.areaService.createArea(this.mudId, {
			name: `Etage ${Math.random() * 100}`,
			description: 'Lorem ipsum dolor sit amet'
		});
	}
}
