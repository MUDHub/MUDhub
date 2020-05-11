import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IArea } from 'src/app/model/areas/IArea';
import { AreaService } from 'src/app/services/area.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IAreaCreateRequest } from 'src/app/model/areas/AreaDTO';
import swal from 'sweetalert2';

@Component({
	selector: 'mh-area-list',
	templateUrl: './area-list.component.html',
	styleUrls: ['./area-list.component.scss'],
})
export class AreaListComponent implements OnInit {
	constructor(
		private route: ActivatedRoute,
		private router: Router,
		private areaService: AreaService
	) {}

	mudid: string;
	areas: IArea[];

	isFormActive = false;

	async ngOnInit() {
		this.mudid = this.route.snapshot.params.mudid;
		this.areas = await this.areaService.getAreasForMud(this.mudid);
	}

	async deleteArea(area: IArea) {
		try {
			await this.areaService.deleteArea(area.areaId);
			this.areas.splice(this.areas.indexOf(area), 1);
			this.router.navigate(['../areas'], { relativeTo: this.route });
		} catch (err) {
			console.error('Error while deleting area', err);
			swal.fire({
				icon: 'error',
				title: 'Fehler',
				text: err.error.displayMessage
			});
		}
	}

	async onAdd(input: HTMLInputElement) {
		const name = input.value;
		if (name) {
			const area: IAreaCreateRequest = { name, mudId: this.mudid };
			try {
				const response = await this.areaService.createArea(area);
				this.areas.push(response.area);
				input.value = '';
				this.isFormActive = false;
			} catch (err) {
				console.error('Error while creating area', err);
				swal.fire({
					icon: 'error',
					title: 'Fehler',
					text: err.error?.displayMessage || err.error?.errormessage || 'Fehler beim Erstellen der Area'
				});
			}
		}
	}

	async updateName(area: IArea, newName: string) {
		area.name = newName;
		await this.areaService.updateArea(area.areaId, { name: newName });
	}
}
