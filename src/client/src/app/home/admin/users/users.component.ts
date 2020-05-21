import { Component, OnInit, ViewChild } from '@angular/core';
import { IUser } from 'src/app/model/auth/IUser';
import { UsersService } from 'src/app/services/users.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { UserRole } from 'src/app/model/auth/UserRole';

@Component({
	templateUrl: './users.component.html',
	styleUrls: ['./users.component.scss'],
})
export class UsersComponent implements OnInit {
	constructor(private userService: UsersService) {}

	UserRole = UserRole;

	@ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

	dataSource = new MatTableDataSource<IUser>([]);
	displayedColumns = ['firstname', 'lastname', 'email', 'roles', 'actions'];

	async ngOnInit() {
		this.dataSource.paginator = this.paginator;
		await this.loadUsers();
	}

	async loadUsers() {
		const users = await this.userService.getAll();
		this.dataSource.data = users;
	}

	async addRole(id: string, role: UserRole) {
		try {
			await this.userService.addRoleToUser(id, role);
			await this.loadUsers();
		} catch (err) {
			console.error('Error while adding role to user', err);
		}
	}

	async removeRole(id: string, role: UserRole) {
		try {
			await this.userService.removeRoleFromUser(id, role);
			await this.loadUsers();
		} catch (err) {
			console.error('Error while adding role to user', err);
		}
	}


	async delete(user: IUser) {
		try {
			await this.userService.delete(user.id);
			await this.loadUsers();
		} catch (err) {
			console.error(`Error while deleting user(${user.id})`, err);
		}
	}
}
