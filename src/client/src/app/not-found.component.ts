import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
	template: `
		<div class="container">
			<h1>Seite nicht gefunden</h1>
			<h4 class="route">{{ activeRoute }}</h4>
			<a href="/" class="link">Zurück zur Homepage</a>
		</div>
	`,
	styles: [
		`
			.container {
				display: flex;
				flex-direction: column;
				justify-content: center;
				align-items: center;
				height: 100vh;
			}
			.route {
				font-style: italic;
			}
			.link {
				cursor: pointer;
			}
		`,
	],
})
export class NotFoundComponent implements OnInit {
	constructor(private route: ActivatedRoute) {}

	activeRoute: string;

	ngOnInit() {
		this.activeRoute = this.route.snapshot.url.join('/');
	}
}
