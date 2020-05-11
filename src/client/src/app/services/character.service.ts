import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment as env } from 'src/environments/environment';
import { ICharacterCreateRequest } from '../model/character/CharacterDTO';
import { IBaseResponse } from '../model/BaseResponse';
import { ICharacter } from '../model/character/ICharacter';

@Injectable({
	providedIn: 'root',
})
export class CharacterService {
	constructor(private http: HttpClient) {}

	public async getCharacters() {}

	public async createCharacter(mudid: string, args: ICharacterCreateRequest) {
		return await this.http
			.post<IBaseResponse>(`${env.api.url}/muds/${mudid}/characters`, args)
			.toPromise();
	}

	public async getCharactersForPlayer(mudid: string, playerid: string) {
		return await this.http.get<ICharacter[]>(`${env.api.url}/muds/${mudid}/characters`, {
			params: {
				userId: playerid
			}
		}).toPromise();
	}
}
