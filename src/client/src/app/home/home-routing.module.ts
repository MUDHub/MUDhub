import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeShellComponent } from './home-shell/home-shell.component';
import { MudListComponent } from './mud-list/mud-list.component';
import { MyGamesComponent } from './my-games/my-games.component';
import { MyMudsComponent } from './my-muds/my-muds.component';
import { ProfileComponent } from './profile/profile.component';
import { UsersComponent } from './admin/users/users.component';
import { MasterGuard } from '../guards/master.guard';
import { AdminGuard } from '../guards/admin.guard';

const routes: Routes = [
	{
		path: '',
		component: HomeShellComponent,
		children: [
			{
				path: '',
				redirectTo: 'muds',
				pathMatch: 'full',
			},
			{
				path: 'muds',
				component: MudListComponent,
			},
			{
				path: 'my-games',
				component: MyGamesComponent,
			},
			{
				path: 'my-muds',
				canActivate: [MasterGuard],
				component: MyMudsComponent,
			},
			{
				path: 'profile',
				component: ProfileComponent,
			},
			{
				path: 'mud-create',
				canActivate: [MasterGuard],
				loadChildren: () =>
					import('./mud-create/mud-create.module').then(
						m => m.MudCreateModule
					),
			},
			{
				path: 'admin',
				canActivate: [AdminGuard],
				children: [
					{
						path: 'users',
						component: UsersComponent,
					},
				],
			},
		],
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class HomeRoutingModule {}
