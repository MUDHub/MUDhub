import { Injectable } from '@angular/core';
import {
	IMudCreateRequest,
	IMudCreateResponse,
	IRequestResponse,
	MudJoinState,
	IMudClassResponse,
	IMudRaceResponse,
	IMudItemResponse,
	IMudItemRequest,
	IMudRaceRequest,
	IMudClassRequest,
} from '../model/muds/MudDTO';
import { HttpClient } from '@angular/common/http';
import { environment as env } from 'src/environments/environment';
import { IMud } from '../model/muds/IMud';
import { IMudRequest } from '../model/muds/IMudRequest';
import { IMudRace, IMudClass, IMudItem } from '../model/muds/MudSetupDTO';

@Injectable({
	providedIn: 'root',
})
export class MudService {
	constructor(private http: HttpClient) {}

	async getAll() {
		return await this.http.get<IMud[]>(`${env.api.url}/muds`).toPromise();
	}

	async getById(id: string) {
		return await this.http
			.get<IMud>(`${env.api.url}/muds/${id}`)
			.toPromise();
	}

	async getForUserId(userid: string) {
		return await this.http
			.get<IMud[]>(`${env.api.url}/muds`, { params: { userid } })
			.toPromise();
	}

	async create(args: IMudCreateRequest) {
		const response = await this.http
			.post<IMudCreateResponse>(`${env.api.url}/muds`, args)
			.toPromise();
		return response.mud;
	}

	async deleteMud(mudId: string) {
		return await this.http
			.delete(`${env.api.url}/muds/${mudId}`)
			.toPromise();
	}

	async getJoinRequests(mudId: string): Promise<IMudRequest[]> {
		const requests = await this.http
			.get<IRequestResponse[]>(`${env.api.url}/muds/${mudId}/request`)
			.toPromise();

		return requests.map<IMudRequest>(r => {
			return {
				userId: r.userid,
				email: r.userEmail,
				mudId: r.mudGameId,
				state: r.state,
			};
		});
	}

	async requestAccess(mudId: string) {
		return await this.http
			.post(`${env.api.url}/muds/${mudId}/requestjoin`, null)
			.toPromise();
	}

	async setRequestState(mudId: string, userId: string, state: MudJoinState) {
		return await this.http
			.put(`${env.api.url}/muds/${mudId}/request/${userId}`, { state })
			.toPromise();
	}

	//SETUP Process
	//SETUP - Race
	async getMudRace(mudId: string): Promise<IMudRace[]> {
		return await this.http
			.get<IMudRace[]>(`${env.api.url}/mudgame/${mudId}/Races`)
			.toPromise();
	}

	async addMudRace(
		mudId: string,
		mudRace: IMudRaceRequest
	): Promise<IMudRaceResponse> {
		return await this.http
			.post<IMudRaceResponse>(
				`${env.api.url}/mudgame/${mudId}/Races`,
				mudRace
			)
			.toPromise();
	}

	async deleteMudRace(mudId: string, mudRace: IMudRace) {
		return await this.http
			.delete(`${env.api.url}/mudgame/${mudId}/Races/${mudRace.raceId}`)
			.toPromise();
	}

	// SETUP - Class
	async getMudClass(mudId: string): Promise<IMudClass[]> {
		return await this.http
			.get<IMudClass[]>(`${env.api.url}/mudgame/${mudId}/classes`)
			.toPromise();
	}

	async addMudClass(
		mudId: string,
		mudClass: IMudClassRequest
	): Promise<IMudClassResponse> {
		return await this.http
			.post<IMudClassResponse>(
				`${env.api.url}/mudgame/${mudId}/classes`,
				mudClass
			)
			.toPromise();
	}

	async deleteMudClass(mudId: string, mudClass: IMudClass) {
		return await this.http
			.delete(
				`${env.api.url}/mudgame/${mudId}/classes/${mudClass.classId}`
			)
			.toPromise();
	}

	// SETUP - Item
	async getMudItem(mudId: string): Promise<IMudItem[]> {
		return await this.http
			.get<IMudItem[]>(`${env.api.url}/muds/${mudId}/items`)
			.toPromise();
	}

	async addMudItem(
		mudId: string,
		mudItem: IMudItemRequest
	): Promise<IMudItemResponse> {
		console.log(mudItem);
		return await this.http
			.post<IMudItemResponse>(
				`${env.api.url}/muds/${mudId}/items`,
				mudItem
			)
			.toPromise();
	}

	async deleteMudItem(mudId: string, mudItem: IMudItem) {
		return await this.http
			.delete(`${env.api.url}/muds/${mudId}/items/${mudItem.itemId}`)
			.toPromise();
	}
}
