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

  muds: IMud[];

  async ngOnInit() {
	  this.muds = await this.mudService.getById(this.authService.user.id);
  }

}
