import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HelpComponent } from './help.component';
import { HelpLoginComponent } from './help-login/help-login.component';
import { FaqComponent } from './faq/faq.component';
import { IntroductionComponent } from './introduction/introduction.component';

const routes: Routes = [
	{
		path: '',
		component: HelpComponent,
		children: [
			{
				path: '',
				redirectTo: 'introduction',
				pathMatch: 'full',
			},
			{
				path: 'introduction',
				component: IntroductionComponent,
			},
			{
				path: 'help-login',
				component: HelpLoginComponent,
			},
			{
				path: 'faq',
				component: FaqComponent,
            },
            
		],
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class HelpRoutingModule {}
