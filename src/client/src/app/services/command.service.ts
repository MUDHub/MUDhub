import { Injectable } from '@angular/core';
import { GameService } from './game.service';
import { Direction } from '../model/game/Direction';

@Injectable({
	providedIn: 'root',
})
export class CommandService {
	constructor(private game: GameService) {}

	private registeredCommands: ICommand[] = [
		{
			keyword: 'gehe',
			shorthand: 'g',
			arguments: [
				{
					name: 'Richtung / Portal'
				}
			]
		}
	];

	handleInput(input: string) {
		const [command, ...args] = input.split(' ');

		switch (command) {
			case 'g':
			case 'gehe':
				this.handleMovement(args);
		}
	}


	private handleMovement(args: string[]) {
		const dir = args[0];
		if (!dir) {
			throw new Error('invalid command structure');
		}

		const direction = this.getDirectionFromString(dir);
		if (direction) {
			this.game.tryEnterRoom(direction);
		} else {
			// Room is portal
		}

	}



	private getDirectionFromString(dir: string): Direction {
		switch (dir) {
			case 'n':
			case 'norden':
				return Direction.NORTH;
			case 's':
			case 'süden':
				return Direction.SOUTH;
			case 'o':
			case 'osten':
				return Direction.EAST;
			case 'w':
			case 'westen':
				return Direction.WEST;
			default:
				return undefined;
		}
	}
}

export interface ICommand {
	keyword: string;
	shorthand?: string;
	arguments?: [
		{
			name: string;
		}
	];
}
