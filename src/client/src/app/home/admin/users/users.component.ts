import { Component, OnInit, ViewChild } from '@angular/core';
import { IUser } from 'src/app/model/IUser';
import { UsersService } from 'src/app/services/users.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { UserRole } from 'src/app/model/UserRole';

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
		const users = await this.userService.getAll();
		this.dataSource.data = users;
	}

	async addRole(id: string, role: UserRole) {
		console.log({ id, role });
		const response = await this.userService.addRoleToUser(id, role);
		console.log(response);
	}
}
