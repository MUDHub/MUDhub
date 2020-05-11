import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IMudRace, IMudClass, IMudItem } from 'src/app/model/muds/MudSetupDTO';
import { IRoom } from 'src/app/model/areas/IRoom';
import { IArea } from 'src/app/model/areas/IArea';
import { MudService } from 'src/app/services/mud.service';
import { AreaService } from 'src/app/services/area.service';

@Component({
	selector: 'mh-finish',
	templateUrl: './finish.component.html',
	styleUrls: ['./finish.component.scss'],
})
export class FinishComponent implements OnInit {
	constructor(
		private route: ActivatedRoute,
		private router: Router,
		private mudService: MudService,
		private areaService: AreaService
	) {}

	panelOpenState: boolean = true;
	mudId: string;

	/*Übersicht Rassen*/
	races: IMudRace[] = [];

	async getRaces() {
		this.races = await this.mudService.getRaceForMud(this.mudId);
	}
	/*Übersicht Klassen*/
	classes: IMudClass[] = [];

	async getClasses() {
		this.classes = await this.mudService.getClassForMud(this.mudId);
		console.log(this.classes);
	}

	/*Übersicht Items*/
	items: IMudItem[] = [];

	async getItems() {
		this.items = await this.mudService.getItemsForMud(this.mudId);
	}

	/*Übersicht Räume*/
	rooms: IRoom[] = [];
	areas: IArea[] = [];

	async getAreas() {
		this.areas = await this.areaService.getAreasForMud(this.mudId);
	}

	async getRooms() {
		for (let i = 0; i < this.areas.length; i++) {
			this.rooms = await this.areaService.getRoomsForMud(this.mudId);
		}
	}

	/*General*/

	async ngOnInit() {
		this.mudId = this.route.snapshot.params.mudid;

		// Fetch Ressources from Service
		await this.getRaces();
		await this.getClasses();
		await this.getItems();
		await this.getAreas();
		await this.getRooms();
	}

	async onSubmit() {
		/* Object erstellen */
		/* Request zur API schicken */

		// Redirect zur MyMuds Seite - Configuration completed
		this.router.navigate(['/my-muds']);
	}
}
