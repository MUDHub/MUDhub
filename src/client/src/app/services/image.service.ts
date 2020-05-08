import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment as env } from 'src/environments/environment';

@Injectable({
	providedIn: 'root',
})
export class ImageService {
	constructor(private http: HttpClient) {}

	async uploadFile(file: File): Promise<string> {
		if (file != null) {
			const formData = new FormData();
			formData.append('file', file, file.name);

			return await this.http
				.post<string>(`${env.api.url}/images`, formData)
				.toPromise();
		}

		throw new Error('Argument "file" cannot be null');
	}
}
