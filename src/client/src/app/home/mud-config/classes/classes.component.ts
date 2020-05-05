import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'mh-classes',
  templateUrl: './classes.component.html',
  styleUrls: ['./classes.component.scss']
})
export class ClassesComponent implements OnInit {

	constructor(
		private fb: FormBuilder,
		private route: ActivatedRoute,
		private router: Router
	) {}

	form = this.fb.group({
		name: ['', Validators.required],
		description: ['', Validators.required],
		imagekey: [''],
	});

	dialog =  false;
	mudId: string;

	//Todo Interface muss implementiert werden
	classes: Array<{name: string; description: string; imagekey: string }> = [];

	ngOnInit(): void {
		this.mudId = this.route.snapshot.params.mudid;
	}

	changeDialog() {
		this.form.reset();
		this.dialog = !this.dialog;
	}

	onAbort() {
		this.router.navigate(['/my-muds']);
	}

	addClass() {
		this.classes.push({
			name: this.form.get('name').value,
			description: this.form.get('description').value,
			imagekey: this.form.get('imagekey').value,
		});

		this.changeDialog();
	}

	deleteRow(index: number){
		this.classes.splice(index, 1);
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
