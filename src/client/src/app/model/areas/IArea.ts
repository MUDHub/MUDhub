import { IMud } from '../muds/IMud';

export interface IArea {
	areaId: string;
	description: string;
	name: string;
	mudId: string;
	mud: IMud;
}
