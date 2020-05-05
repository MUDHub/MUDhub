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
import { MudJoinComponent } from './mud-list/mud-join/mud-join.component';
import { MudCreateComponent } from './mud-create/mud-create.component';
import { RacesComponent } from './mud-config/races/races.component';
import { RequestsComponent } from './my-muds/requests/requests.component';
import { MudsComponent } from './admin/muds/muds.component';
import { ClassesComponent } from './mud-config/classes/classes.component';
import { ItemsComponent } from './mud-config/items/items.component';
import { RoomsComponent } from './mud-config/rooms/rooms.component';
import { FinishComponent } from './mud-config/finish/finish.component';

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
				children: [
					{
						path: 'join',
						component: MudJoinComponent
					}
				]
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
				path: 'my-muds/create',
				canActivate: [MasterGuard],
				component: MudCreateComponent
			},
			{
				path: 'my-muds/:mudid',
				children: [
					{
						path: 'races',
						component: RacesComponent
					},
					{
						path: 'classes',
						component: ClassesComponent
					},
					{
						path: 'items',
						component: ItemsComponent
					},
					{
						path: 'rooms',
						component: RoomsComponent
					},
					{
						path: 'finish',
						component: FinishComponent
					},
					{
						path: 'requests',
						component: RequestsComponent
					}
				]
			},
			{
				path: 'profile',
				component: ProfileComponent,
			},
			{
				path: 'admin',
				canActivate: [AdminGuard],
				children: [
					{
						path: 'users',
						component: UsersComponent,
					},
					{
						path: 'muds',
						component: MudsComponent,
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
export class HomeRoutingModule { }
