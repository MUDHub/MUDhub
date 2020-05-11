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
import { RoomCreateComponent } from './mud-config/rooms/rooms-grid/room-create/room-create.component';
import { componentFactoryName } from '@angular/compiler';
import { RoomsGridComponent } from './mud-config/rooms/rooms-grid/rooms-grid.component';
import { MudConfigShellComponent } from './mud-config/mud-config-shell.component';
import { RoomPortalComponent } from './mud-config/rooms/rooms-grid/room-portal/room-portal.component';

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
						component: MudJoinComponent,
					},
				],
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
				component: MudCreateComponent,
			},
			{
				path: 'my-muds/:mudid/requests',
				component: RequestsComponent,
			},
			{
				path: 'my-muds/:mudid',
				component: MudConfigShellComponent,
				children: [
					{
						path: '',
						pathMatch: 'full',
						redirectTo: 'races',
					},
					{
						path: 'races',
						component: RacesComponent,
					},
					{
						path: 'classes',
						component: ClassesComponent,
					},
					{
						path: 'items',
						component: ItemsComponent,
					},
					{
						path: 'areas',
						component: RoomsComponent,
						children: [
							{
								path: ':areaid/rooms',
								component: RoomsGridComponent,
								children: [
									{
										path: 'create',
										component: RoomCreateComponent,
									},
									{
										path: ':roomid/edit',
										component: RoomCreateComponent,
									},
									{
										path: ':roomid/portals',
										component: RoomPortalComponent,
									},
								],
							},
						],
					},
					{
						path: 'finish',
						component: FinishComponent,
					},
				],
			},
			{
				path: 'profile',
				component: ProfileComponent,
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
export class HomeRoutingModule {}
