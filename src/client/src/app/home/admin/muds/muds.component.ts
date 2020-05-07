import { Component, OnInit, ViewChild } from '@angular/core';
import { MudService } from 'src/app/services/mud.service';
import { IMud } from 'src/app/model/IMud';
import { MatTableDataSource, MatTable } from '@angular/material/table';

@Component({
	templateUrl: './muds.component.html',
	styleUrls: ['./muds.component.scss'],
})
export class MudsComponent implements OnInit {
	constructor(private mudService: MudService) {}

	@ViewChild('table') table: MatTable<IMud>;

	dataSource = new MatTableDataSource<IMud>([]);
	displayedColumns = ['id', 'name', 'owner', 'actions'];

	get muds() {
		return this.dataSource.data;
	}

	async ngOnInit() {
		await this.loadMuds();
	}

	async loadMuds() {
		const muds = await this.mudService.getAll();
		this.dataSource.data = muds;
	}


	async delete(mud: IMud) {
		try {
			await this.mudService.deleteMud(mud.mudId);
			const index = this.muds.indexOf(mud);
			this.muds.splice(index, 1);
			this.table.renderRows();
		} catch (err) {
			console.error('Error while deleting mud', err);
		}
	}
}
