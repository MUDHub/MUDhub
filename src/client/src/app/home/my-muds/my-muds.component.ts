import { Component, OnInit } from '@angular/core';
import { MudService } from 'src/app/services/mud.service';
import { IMud } from 'src/app/model/IMud';
import { AuthService } from 'src/app/services/auth.service';

@Component({
	templateUrl: './my-muds.component.html',
	styleUrls: ['./my-muds.component.scss']
})
export class MyMudsComponent implements OnInit {

	constructor(private mudService: MudService, private authService: AuthService) { }

	muds: IMud[] = [
		{
			mudId: "ajsdlksajdl",
			name: "Mud1",
			description: "description",
			isPublic: "true",
			autoRestart: true,
			owner: this.authService.user
		},
		{
			mudId: "ajsdlksajdl",
			name: "Mud2",
			description: "description",
			isPublic: "true",
			autoRestart: true,
			owner: this.authService.user
		},
		{
			mudId: "ajsdlksajdl",
			name: "Mud3",
			description: "description",
			isPublic: "true",
			autoRestart: true,
			owner: this.authService.user
		}
	];

	async ngOnInit() {
		//this.muds = await this.mudService.getById(this.authService.user.id);

	}

	editMud(mudId: string) {
		//Todo redirect to edit page
		console.log("Edit Mud" + mudId);
	}

	deleteMud(mudId: string) {
		this.mudService.deleteMud(mudId);
	}

}
