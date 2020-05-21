import { Injectable } from '@angular/core';
import { GameService } from './game.service';
import { Direction } from '../model/game/Direction';
import { ItemTransferMethod } from '../model/game/signalR/ItemTransferMethod';

@Injectable({
	providedIn: 'root',
})
export class CommandService {
	constructor(private game: GameService) {}

	private registeredCommands: ICommand[] = [
		{
			keyword: 'gehe',
			shorthand: 'g',
			description: 'Bewegt den Charakter in die angegebene Richtung oder durch ein Portal',
			arguments: [
				{
					name: 'Richtung / Portal-Name',
				},
			],
			handler: args => this.handleMovement(args),
		},
		{
			keyword: 'untersuche',
			shorthand: 'u',
			description: 'Untersucht ein bestimmtes Objekt und gibt Informationen darüber aus',
			arguments: [
				{
					name: 'Objekt / Gegenstand',
				},
			],
			handler: args => this.handleExamine(args)
		},
		{
			keyword: 'zeige',
			shorthand: 'z',
			description: 'Zeigt Informationen zu einem bestimmten Thema an',
			arguments: [
				{
					name: `'Befehle' | 'Inventar'`,
				},
			],
			handler: args => this.handleShow(args),
		},
		{
			keyword: 'aufheben',
			description: 'Hebt einen Gegenstand auf',
			arguments: [
				{
					name: 'Name des Gegenstandes',
				},
			],
			handler: args => this.handlePickup(args),
		},
		{
			keyword: 'fallenlassen',
			description: 'Lässt einen Gegenstand in den Raum fallen',
			arguments: [
				{
					name: 'Name des Gegenstandes',
				},
			],
			handler: args => this.handleDrop(args),
		},
	];

	async handleInput(input: string) {
		const [name, ...args] = input.split(' ');

		const command = this.registeredCommands.find(c => c.keyword === name.toLowerCase() || c.shorthand === name.toLowerCase());
		if (command) {
			try {
				if (!command.handler) {
					throw new HandlerUndefinedException(command.keyword);
				}

				command.handler(args);
			} catch (err) {
				console.error('Error while invoking signalR handler on the server', err);
			}
		}
	}

	/**
	 * Handle movement commands
	 * @param args Command arguments
	 */
	private async handleMovement(args: string[]) {
		const dir = args[0];
		if (!dir) {
			throw new InvalidCommandException();
		}

		const direction = this.getDirectionFromString(dir);
		if (direction !== Direction.PORTAL) {
			await this.game.tryEnterRoom(direction);
		} else {
			this.game.tryEnterRoom(direction, dir);
		}
	}

	private getDirectionFromString(dir: string): Direction {
		switch (dir.toLowerCase()) {
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
				return Direction.PORTAL;
		}
	}

	private async handleExamine(args: string[]) {
		const [subject] = args;

		switch (subject.toLowerCase()) {
			case 'ausgänge':
				await this.game.showExits();
				break;
			case 'boden':
				await this.game.showRoomInventory();
				break;
		}
	}

	private async handleShow(args: string[]) {
		const [subject] = args;

		switch (subject.toLowerCase()) {
			case 'inventar':
				await this.game.showPlayerInventory();
				break;
			case 'befehle':
				await this.game.showCommands(this.registeredCommands);
		}
	}

	/**
	 * Handler method for picking up items
	 * @param args Command arguments
	 */
	private async handlePickup(args: string[]) {
		const [itemName] = args;

		if (itemName) {
			await this.game.transferItem(itemName, ItemTransferMethod.PICKUP);
		}
	}

	/**
	 * Handler method for dropping items
	 * @param args Command arguments
	 */
	private async handleDrop(args: string[]) {
		const [itemName] = args;

		if (itemName) {
			await this.game.transferItem(itemName, ItemTransferMethod.DROP);
		}
	}
}

export interface ICommand {
	keyword: string;
	shorthand?: string;
	description: string;
	arguments?: [
		{
			name: string;
		}
	];
	handler?: (args?: string[]) => Promise<void> | void;
}

export class PortalNameUndefinedException extends Error {
	constructor() {
		super('The Name of the command cannot be undefined');
	}
}
export class InvalidCommandException extends Error {}
export class HandlerUndefinedException extends Error {
	constructor(commandName: string) {
		super(`No handler for the command '${commandName}' specified`);
	}
}
