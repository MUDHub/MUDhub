import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'mh-classes',
  templateUrl: './classes.component.html',
  styleUrls: ['./classes.component.scss']
})
export class ClassesComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router) { }

  mudId: string;

  ngOnInit(): void {
    this.mudId = this.route.snapshot.params.mudid;
  }

  onAbort(){
		this.router.navigate(['/my-muds/create']);
	}

	onLast() {
		this.router.navigate(['/my-muds/' + this.mudId + '/races']);
	}

	async onSubmit(){
		/* Object erstellen */
		/* Request zur API schicken */
		
		//Redirect zur nächsten Konfigurationsseite
		this.router.navigate(['/my-muds/'+this.mudId+'/items']);
	}

}
