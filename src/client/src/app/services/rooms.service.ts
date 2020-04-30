import { Injectable } from '@angular/core';
import { IRoom } from '../model/IRoom';

@Injectable({
	providedIn: 'root',
})
export class RoomsService {
	constructor() {}


	public getRoomsForArea(areaId: string): IRoom[] {
		// TODO: remove placeholder items
		return [
			{
				name: '',
				position: { x: 0, y: 0 },
				connections: {
				},
			},
			{
				name: '',
				position: { x: 1, y: 0 },
				connections: {
					w: true,
					n: true
				},
			},
			{
				name: '',
				position: { x: 2, y: 0 },
				connections: {
					w: true,
				},
			},
			{
				name: '',
				position: { x: 3, y: 0 },
				connections: {
					w: true,
				},
			},
			{
				name: '',
				position: { x: 0, y: 1 },
				connections: {
				},
			},
			{
				name: '',
				position: { x: 1, y: 1 },
				connections: {
					w: true,
				},
			},
			{
				name: '',
				position: { x: 2, y: 1 },
				connections: {
					w: true,
				},
			},
			{
				name: '',
				position: { x: 3, y: 1 },
				connections: {
					w: true,
				},
			},
		];
	}
}
