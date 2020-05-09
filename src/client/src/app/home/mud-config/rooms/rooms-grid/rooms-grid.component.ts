import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IRoom } from 'src/app/model/areas/IRoom';
import { VirtualTimeScheduler } from 'rxjs';

@Component({
	selector: 'mh-rooms-grid',
	templateUrl: './rooms-grid.component.html',
	styleUrls: ['./rooms-grid.component.scss'],
})
export class RoomsGridComponent {
	constructor() {}

	@Input() rooms: IRoom[][];

	@Output() delete = new EventEmitter<IRoom>();
	@Output() create = new EventEmitter<IRoom>();

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
							this.delete.emit(room);
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
						this.delete.emit(room);
					}

					this.rooms.pop();
				}
			}
		}
	}
}
