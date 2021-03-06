import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IRoom } from 'src/app/model/areas/IRoom';
import { VirtualTimeScheduler } from 'rxjs';
import { AreaService } from 'src/app/services/area.service';
import { ActivatedRoute, Router } from '@angular/router';
import swal from 'sweetalert2';
import { IConnectionCreateRequest, LockType } from 'src/app/model/areas/ConnectionsDTO';
import { IConnection } from 'src/app/model/areas/IConnection';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
	selector: 'mh-rooms-grid',
	templateUrl: './rooms-grid.component.html',
	styleUrls: ['./rooms-grid.component.scss'],
})
export class RoomsGridComponent implements OnInit {
	constructor(private route: ActivatedRoute, private areaService: AreaService) {}

	readonly maxWidth = 10;
	readonly maxHeight = 7;

	mudid: string;
	areaid: string;
	rooms: IRoom[][] = [[]];
	connections: IConnection[];

	async ngOnInit() {
		this.route.params.subscribe(async params => {
			this.mudid = params.mudid;
			this.areaid = params.areaid;
			const rooms = await this.areaService.getRoomsForArea(this.areaid);
			if (rooms.length > 0) {
				this.rooms = this.mapRooms(rooms);
			} else {
				this.rooms = [
					[undefined, undefined],
					[undefined, undefined],
				];
			}
		});

		this.areaService.roomCreated$.subscribe(room => {
			if (room.isDefaultRoom) {
				for (const row of this.rooms) {
					for (const r of row) {
						if (r && r.isDefaultRoom) {
							r.isDefaultRoom = false;
						}
					}
				}
			}
			this.rooms[room.y][room.x] = room;
		});

		this.areaService.roomUpdated$.subscribe(room => {
			if (room.isDefaultRoom) {
				for (const row of this.rooms) {
					for (const r of row) {
						if (r && r.isDefaultRoom) {
							r.isDefaultRoom = false;
						}
					}
				}
			}
			this.rooms[room.y][room.x] = room;
		});
	}

	get width() {
		return this.rooms[this.rooms.length - 1]?.length;
	}

	addColumn() {
		if (this.width < this.maxWidth) {
			for (const row of this.rooms) {
				row.push(undefined);
			}
		}
	}

	async removeColumn() {
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
				const dialogResult = await swal.fire({
					icon: 'warning',
					title: 'Warnung',
					text: 'Durch das Löschen einer Spalte würden Räume gelöscht werden! Fortfahren?',
					showCancelButton: true,
					cancelButtonText: 'Nein',
					confirmButtonText: 'Ja',
				});
				if (dialogResult.value) {
					for (const row of this.rooms) {
						const room = row.pop();
						if (room) {
							this.areaService.deleteRoom(room.roomId);
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
		if (this.height < this.maxHeight) {
			this.rooms.push(new Array(this.width).fill(undefined));
		}
	}

	async removeRow() {
		if (this.rooms.length > 1) {
			const isSafe = !this.rooms[this.rooms.length - 1].some(r => r !== undefined);
			if (isSafe) {
				this.rooms.pop();
			} else {
				const dialogResult = await swal.fire({
					icon: 'warning',
					title: 'Warnung',
					text: 'Durch das Löschen einer Reihe würden Räume gelöscht werden! Fortfahren?',
					showCancelButton: true,
					cancelButtonText: 'Nein',
					confirmButtonText: 'Ja',
				});
				if (dialogResult.value) {
					const toDelete = this.rooms[this.rooms.length - 1].filter(r => r !== undefined);

					for (const room of toDelete) {
						this.areaService.deleteRoom(room.roomId);
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

	public async deleteRoom(room: IRoom) {
		try {
			await this.areaService.deleteRoom(room.roomId);

			for (let y = 0; y < this.rooms.length; y++) {
				for (let x = 0; x < this.rooms[y].length; x++) {
					if (this.rooms[y][x]?.roomId === room.roomId) {
						this.rooms[y][x] = undefined;
						if (this.rooms[y][x + 1]) {
							this.rooms[y][x + 1].connections.west = false;
						}
						if (x - 1 > 0 && this.rooms[y][x - 1]) {
							this.rooms[y][x - 1].connections.east = false;
						}
						if (this.rooms[y + 1] && this.rooms[y + 1][x]) {
							this.rooms[y + 1][x].connections.north = false;
						}
						if (this.rooms[y - 1] && this.rooms[y - 1][x]) {
							this.rooms[y - 1][x].connections.south = false;
						}
						return;
					}
				}
			}
		} catch (err) {
			console.error('Error while deleting room', err);
			swal.fire({
				icon: 'error',
				title: 'Fehler',
				text: err.error?.displayMessage || err.error?.errormessage || 'Fehler beim Löschen des Raumes',
			});
		}
	}

	public async addConnection(room1: IRoom, room2: IRoom) {
		const connection: IConnectionCreateRequest = {
			roomId1: room1.roomId,
			roomId2: room2.roomId,
			lockType: LockType.NoLock,
		};
		try {
			const response = await this.areaService.createConnection(connection);

			for (let y = 0; y < this.rooms.length; y++) {
				for (let x = 0; x < this.rooms[y].length; x++) {
					const room = this.rooms[y][x];
					if (response.connection.room1Id === room?.roomId) {
						if (this.rooms[y][x + 1]?.roomId === response.connection.room2Id) {
							room.connections.east = true;
							this.rooms[y][x + 1].connections.west = true;
							continue;
						}
						if (this.rooms[y][x - 1]?.roomId === response.connection.room2Id) {
							room.connections.west = true;
							this.rooms[y][x - 1].connections.east = true;
							continue;
						}
						if (this.rooms[y + 1] && this.rooms[y + 1][x]?.roomId === response.connection.room2Id) {
							room.connections.south = true;
							this.rooms[y + 1][x].connections.north = true;
							continue;
						}
						if (this.rooms[y - 1] && this.rooms[y - 1][x]?.roomId === response.connection.room2Id) {
							room.connections.north = true;
							this.rooms[y - 1][x].connections.south = true;
							continue;
						}
					}
				}
			}
		} catch (err) {
			console.error('Error while creating connection', err);
		}
	}

	public async deleteConnection(room1: IRoom, room2: IRoom) {
		const connectionsForRoom = await this.areaService.getConnectionsForRoom(room1.roomId);
		const connectionToDelete = connectionsForRoom.filter(
			c => c.room1Id === room2.roomId || c.room2Id === room2.roomId
		)[0];
		if (connectionToDelete) {
			try {
				await this.areaService.deleteConnection(connectionToDelete.id);
				for (let y = 0; y < this.rooms.length; y++) {
					for (let x = 0; x < this.rooms[y].length; x++) {
						const room = this.rooms[y][x];
						if (room?.roomId === connectionToDelete.room1Id) {
							if (this.rooms[y][x + 1]?.roomId === connectionToDelete.room2Id) {
								room.connections.east = false;
								this.rooms[y][x + 1].connections.west = false;
								continue;
							}
							if (this.rooms[y][x - 1]?.roomId === connectionToDelete.room2Id) {
								room.connections.west = false;
								this.rooms[y][x - 1].connections.east = false;
								continue;
							}
							if (this.rooms[y + 1] && this.rooms[y + 1][x]?.roomId === connectionToDelete.room2Id) {
								room.connections.south = false;
								this.rooms[y + 1][x].connections.north = false;
								continue;
							}
							if (this.rooms[y - 1] && this.rooms[y - 1][x]?.roomId === connectionToDelete.room2Id) {
								room.connections.north = false;
								this.rooms[y - 1][x].connections.south = false;
								continue;
							}
						}
					}
				}
			} catch (err) {
				console.error('Error while deleting connection', err);
				swal.fire({
					icon: 'error',
					title: 'Fehler',
					text: err.error?.displayMessage || err.error?.errormessage || 'Fehler beim Löschen der Verbindung',
				});
			}
		}
	}
}
