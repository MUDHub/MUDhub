import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'mh-finish',
  templateUrl: './finish.component.html',
  styleUrls: ['./finish.component.scss']
})
export class FinishComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router) { }

  mudId: string;

  ngOnInit(): void {
    this.mudId = this.route.snapshot.params.mudid;
  }

  onAbort(){
		this.router.navigate(['/my-muds/create']);
	}

	onLast() {
		this.router.navigate(['/my-muds/' + this.mudId + '/rooms']);
	}

	async onSubmit(){
		/* Object erstellen */
		/* Request zur API schicken */
		
		//Redirect zur MyMuds Seite - Configuration completed
		this.router.navigate(['/my-muds']);
	}
}
