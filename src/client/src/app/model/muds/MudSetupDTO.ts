export interface IMudRace {
	raceId: string;
	name: string;
	description: string;
	imageKey: string;
}

export interface IMudClass {
	classId: string;
	name: string;
	description: string;
	imageKey: string;
}

export interface IMudItem {
	id: string;
	name: string;
	description: string;
	imageKey: string;
	weight: number;
}

export interface IValidationResult {
	validationErrors: [
		{
			region: IRegion;
			message: string;
		}
	];
	isMudValid: boolean;
	displayMessage: string;
	errormessage: string;
	succeeded: boolean;
}

export enum IRegion {
	General = 0,
	Races = 1,
	Classes = 2,
	Items = 3,
	Areas = 4,
}
