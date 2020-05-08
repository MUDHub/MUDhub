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
		//Noch nicht in API implementiert
		this.races = [
			{
				name: "name",
				description: "desc",
				imagekey: "imagekey",
			},
			{
				name: "name2",
				description: "desc2",
				imagekey: "imagekey2",
			},
			{
				name: "name3",
				description: "desc3",
				imagekey: "imagekey3",
			}
		];
	}

	/*Übersicht Klassen*/
	classes: IMudClass[] = [];

	async getClasses() {
		//Noch nicht in API implementiert
		this.classes = [
			{
				name: "name",
				description: "desc",
				imagekey: "imagekey",
			},
			{
				name: "name2",
				description: "desc2",
				imagekey: "imagekey2",
			},
			{
				name: "name3",
				description: "desc3",
				imagekey: "imagekey3",
			}
		];
	}

	/*Übersicht Items*/
	items: IMudItem[] = [];

	async getItems() {
		//Noch nicht in API implementiert
		this.items = [
			{
				name: "name",
				description: "desc",
			},
			{
				name: "name2",
				description: "desc2",
			},
			{
				name: "name3",
				description: "desc3",
			}
		];
	}

	/*Übersicht Räume*/
	rooms: IRoom[][] = [[]];
	areas: IArea[] = [];

	async getAreas() {
		//this.areas = await this.areaService.getAreasForMUD(this.mudId);
	}

	async getRooms() {
		for (let i = 0; i < this.areas.length; i++) {
			//this.rooms = await this.areaService.getRooms(this.mudId, this.areas[i].areaId);
		}
	}

	/*General*/

	ngOnInit(): void {
		this.mudId = this.route.snapshot.params.mudid;

		//Fetch Ressources from Service
		this.getRaces();
		this.getClasses();
		this.getItems();
		this.getAreas();
		this.getRooms();
	}

	onAbort() {
		this.router.navigate(['/my-muds']);
	}

	onLast() {
		this.router.navigate(['/my-muds/' + this.mudId + '/rooms']);
	}

	jump(componentName: string){
		switch (componentName) {
			case "races":
				this.router.navigate(['/my-muds/' + this.mudId + '/races']);
			  break;
			case "classes":
				this.router.navigate(['/my-muds/' + this.mudId + '/classes']);
				break;
			case "items":
				this.router.navigate(['/my-muds/' + this.mudId + '/items']);
				break;
			case "rooms":
				this.router.navigate(['/my-muds/' + this.mudId + '/rooms']);
				break;
			case "finish":
				this.router.navigate(['/my-muds/' + this.mudId + '/finish']);
				break;
			default:
			  break;
		}
	}

	async onSubmit() {
		/* Object erstellen */
		/* Request zur API schicken */

		//Redirect zur MyMuds Seite - Configuration completed
		this.router.navigate(['/my-muds']);
	}
}
