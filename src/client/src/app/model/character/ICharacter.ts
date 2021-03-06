import { IMud } from '../muds/IMud';

export interface ICharacter {
	id: string;
	name: string;
	ownerId: string;
	ownerFullname: string;
	raceName: string;
	className: string;
	roomName: string;
	mud: IMud;
}
