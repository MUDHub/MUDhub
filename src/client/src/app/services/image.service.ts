import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment as env } from 'src/environments/environment';

@Injectable({
	providedIn: 'root',
})
export class ImageService {
	constructor(private http: HttpClient) {}

	async getImageKey(file: File): Promise<string> {
		
		const formData = new FormData();
		formData.append('file', file, file.name);

		if (file != null) {
			return this.http
				.post<string>(`${env.api.url}/images`, formData)
				.toPromise();
		} else {
			return new Promise<string>((res, rej) => {
				res(null);
			});
		}
	}
}
