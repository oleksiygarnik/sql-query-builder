import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Filter } from '../../entities/filter';
import { Car } from '../../entities/car';
import { VirtualTable } from '../../entities/virtual.table';
import { TableService } from '../../services/table.service';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent implements OnInit {
  tableName: string = "Cars";
  cars: Car[];
  virtualTables: VirtualTable[];
  creatingNewTable: boolean = false;
  filters: Filter[] = new Array()
  availableFields: string[] = new Array();
  addBranch: boolean = false;

  @ViewChild("tableNameInput") tableNameInput: ElementRef;

  constructor(private tableService: TableService) {
    tableService.getCars().subscribe(cars => { this.cars = cars; });
    this.getVirtualTables();
    for (let field in new Car()) {
      this.availableFields.push(field);
    }
  }

  getVirtualTables()
  {
    this.tableService.getVirtualTables().subscribe(tables => this.virtualTables = tables);
  }

  ngOnInit(): void {
  }

  deleteVirtualTable(id: number) {
    this.tableService.deleteVirtualTable(id).subscribe(_ => this.getVirtualTables());
  }
  
  selectVirtualTable(table: VirtualTable) {
    this.tableService.getVirtualTable(table.id).subscribe(cars => this.cars = cars);
    this.tableName = table.name;
  }

  addNewFilter() {
    this.filters.push(new Filter('', ''));
  }

  saveVirtualTable() {
    let tableName = this.tableNameInput?.nativeElement.value;
    if (!tableName) {
      alert("Table name can't be empty");
      this.tableNameInput.nativeElement.focus();
      return;
    }

    let selectedFilials = ["LESHALAPTOP"];
    if (this.addBranch) {
      selectedFilials.push("LESHALAPTOP\\MSSQLSERVER_TEMP")
    }

    this.tableService.saveNewTable(tableName, this.filters, selectedFilials)
      .subscribe(_ => this.getVirtualTables());
  }

}
