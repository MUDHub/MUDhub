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
import {
	IMudRace,
	IMudClass,
	IMudItem,
	IValidationResult,
} from '../model/muds/MudSetupDTO';

@Injectable({
	providedIn: 'root',
})
export class MudService {
	constructor(private http: HttpClient) {}

	async getAll() {
		return await this.http.get<IMud[]>(`${env.api.url}/muds`).toPromise();
	}

	async getById(mudid: string) {
		return await this.http
			.get<IMud>(`${env.api.url}/muds/${mudid}`)
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
	//SETUP - Rac
	async getRaceForMud(mudId: string) {
		return await this.http
			.get<IMudRace[]>(`${env.api.url}/races`, {
				params: {
					mudId,
				},
			})
			.toPromise();
	}

	async addRace(mudId: string, mudRace: IMudRaceRequest) {
		return await this.http
			.post<IMudRaceResponse>(`${env.api.url}/races`, mudRace)
			.toPromise();
	}

	async deleteRace(mudId: string, raceid: string) {
		return await this.http
			.delete(`${env.api.url}/races/${raceid}`)
			.toPromise();
	}

	async editRace(raceid: string, mudRace: IMudRaceRequest) {
		return await this.http
			.put<IMudRaceResponse>(`${env.api.url}/races/${raceid}`, mudRace)
			.toPromise();
	}

	// SETUP - Class
	async getClassForMud(mudId: string) {
		return await this.http
			.get<IMudClass[]>(`${env.api.url}/classes`, {
				params: {
					mudId,
				},
			})
			.toPromise();
	}

	async addClass(mudId: string, mudClass: IMudClassRequest) {
		return await this.http
			.post<IMudClassResponse>(`${env.api.url}/classes`, mudClass)
			.toPromise();
	}

	async deleteClass(mudId: string, classid: string) {
		return await this.http
			.delete(`${env.api.url}/classes/${classid}`)
			.toPromise();
	}

	async editClass(classid: string, mudClass: IMudClassRequest) {
		return await this.http
			.put<IMudClassResponse>(
				`${env.api.url}/classes/${classid}`,
				mudClass
			)
			.toPromise();
	}

	// SETUP - Item
	async getItemsForMud(mudId: string) {
		return await this.http
			.get<IMudItem[]>(`${env.api.url}/items`, {
				params: {
					mudId,
				},
			})
			.toPromise();
	}

	async addItem(mudId: string, mudItem: IMudItemRequest) {
		return await this.http
			.post<IMudItemResponse>(`${env.api.url}/items`, mudItem)
			.toPromise();
	}

	async deleteItem(mudId: string, itemid: string) {
		return await this.http
			.delete(`${env.api.url}/items/${itemid}`)
			.toPromise();
	}


	async editItem(itemid: string, mudItem: IMudItemRequest) {
		return await this.http
			.put<IMudItemResponse>(`${env.api.url}/items/${itemid}`, mudItem)
      .toPromise();
  }

	async validateMudGame(mudId: string): Promise<IValidationResult> {
		return await this.http
			.get<IValidationResult>(`${env.api.url}/muds/${mudId}/validate`)
			.toPromise();
	}
}
