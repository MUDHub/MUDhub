import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment as env } from 'src/environments/environment';
import { ICharacterCreateRequest, ICharacterCreateResponse } from '../model/character/CharacterDTO';
import { ICharacter } from '../model/character/ICharacter';

@Injectable({
	providedIn: 'root',
})
export class CharacterService {
	constructor(private http: HttpClient) {}

	public async getCharacter(characterid: string) {
		return await this.http.get<ICharacter>(`${env.api.url}/characters/${characterid}`).toPromise();
	}

	public async createCharacter(mudid: string, args: ICharacterCreateRequest) {
		return await this.http.post<ICharacterCreateResponse>(`${env.api.url}/characters`, args).toPromise();
	}

	public async getCharactersForPlayer(playerid: string) {
		return await this.http
			.get<ICharacter[]>(`${env.api.url}/characters`, {
				params: {
					userId: playerid,
				},
			})
			.toPromise();
	}

	public async getCharactersForPlayerForMud(playerid: string, mudid: string) {
		return await this.http
			.get<ICharacter[]>(`${env.api.url}/characters`, {
				params: {
					userId: playerid,
					mudId: mudid,
				},
			})
			.toPromise();
	}

	public async deleteCharacter(characterid: string) {
		return await this.http.delete(`${env.api.url}/characters/${characterid}`).toPromise();
	}
}
