import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IRoom } from 'src/app/model/areas/IRoom';
import { VirtualTimeScheduler } from 'rxjs';
import { AreaService } from 'src/app/services/area.service';
import { ActivatedRoute } from '@angular/router';

@Component({
	selector: 'mh-rooms-grid',
	templateUrl: './rooms-grid.component.html',
	styleUrls: ['./rooms-grid.component.scss'],
})
export class RoomsGridComponent implements OnInit {
	constructor(
		private route: ActivatedRoute,
		private areaService: AreaService
	) {}

	mudid: string;
	areaid: string;
	rooms: IRoom[][] = [[]];

	async ngOnInit() {
		this.route.params.subscribe(async (params) => {
			this.mudid = params.mudid;
			this.areaid = params.areaid;
			const rooms = await this.areaService.getRooms(this.mudid, this.areaid);
			this.rooms = this.mapRooms(rooms);
		});
	}

	get width() {
		return this.rooms[this.rooms.length - 1]?.length;
	}

	addColumn() {
		if (this.width < 6) {
			for (const row of this.rooms) {
				row.push(undefined);
			}
		}
	}

	removeColumn() {
		if (this.rooms[this.rooms.length - 1].length > 1) {
			let isSafe = true;
			for (const row of this.rooms) {
				if (row[row.length - 1] !== undefined) {
					isSafe = false;
				}
			}

			if (isSafe) {
				for (const row of this.rooms) {
					row.pop();
				}
			} else {
				if (
					confirm(
						'Durch diese Aktion werden Räume gelöscht! Fortfahren?'
					)
				) {
					for (const row of this.rooms) {
						const room = row.pop();
						if (room) {
							// TODO: delete // this.delete.emit(room);
						}
					}
				}
			}
		}
	}

	get height() {
		return this.rooms.length;
	}

	addRow() {
		if (this.height < 6) {
			this.rooms.push(new Array(this.width).fill(undefined));
		}
	}

	removeRow() {
		if (this.rooms.length > 1) {
			const isSafe = !this.rooms[this.rooms.length - 1].some(
				r => r !== undefined
			);
			if (isSafe) {
				this.rooms.pop();
			} else {
				if (
					confirm(
						'Durch diese Aktion werden Räume gelöscht! Fortfahren?'
					)
				) {
					const toDelete = this.rooms[this.rooms.length - 1].filter(
						r => r !== undefined
					);

					for (const room of toDelete) {
						// TODO: delete // this.delete.emit(room);
					}

					this.rooms.pop();
				}
			}
		}
	}

	private mapRooms(roomList: IRoom[]): IRoom[][] {
		const matrix: IRoom[][] = [[]];
		for (const room of roomList) {
			while (matrix.length <= room.y) {
				matrix.push(new Array(matrix[matrix.length - 1].length));
			}

			for (const row of matrix) {
				while (row.length <= room.x) {
					row.push(undefined);
				}
			}

			matrix[room.y][room.x] = room;
		}

		return matrix;
	}
}
