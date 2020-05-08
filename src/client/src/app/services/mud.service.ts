import { Injectable } from '@angular/core';
import {
	IMudCreateRequest,
	IMudCreateResponse,
	IRequestResponse,
	MudJoinState,
} from '../model/MudDTO';
import { HttpClient } from '@angular/common/http';
import { environment as env } from 'src/environments/environment';
import { IMud } from '../model/IMud';
import { IMudRequest } from '../model/IMudRequest';
import { MudRace, MudClass, MudItem } from '../model/MudSetupDTO';

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
	async getMudRace(mudId: string):Promise<MudRace[]>{
		return null;
	}

	async addMudRace(mudId: string, mudRace: MudRace) {
		//Function is not implemented on the api yet
	}

	async deleteMudRace(mudId: string, mudRace: MudRace) {
		//Function is not implemented on the api yet
	}

	//SETUP - Class
	async getMudClass(mudId: string):Promise<MudClass[]>{
		return null;
	}

	async addMudClass(mudId: string, mudClass: MudClass) {
		//Function is not implemented on the api yet
	}

	async deleteMudClass(mudId: string, mudClass: MudClass) {
		//Function is not implemented on the api yet
	}

	//SETUP - Item
	async getMudItem(mudId: string):Promise<MudItem[]>{
		return null;
	}

	async addMudItem(mudId: string, mudItem: MudItem) {
		//Function is not implemented on the api yet
	}

	async deleteMudItem(mudId: string, mudItem: MudItem) {
		//Function is not implemented on the api yet
	}
}
