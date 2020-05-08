import { Component, OnInit, Input } from '@angular/core';
import { IRoom } from 'src/app/model/areas/IRoom';

@Component({
	selector: 'mh-rooms-grid',
	templateUrl: './rooms-grid.component.html',
	styleUrls: ['./rooms-grid.component.scss'],
})
export class RoomsGridComponent {
	constructor() {}

	@Input() rooms: IRoom[][];

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
}
