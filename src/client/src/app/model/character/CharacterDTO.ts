import { ICharacter } from './ICharacter';
import { IBaseResponse } from '../BaseResponse';

export interface ICharacterCreateRequest {
	name: string;
	raceId: string;
	classId: string;
}

export interface ICharacterCreateResponse extends IBaseResponse {
	character: ICharacter;
}
