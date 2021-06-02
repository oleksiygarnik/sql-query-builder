import { Injectable } from '@angular/core';
import { Filter } from '../entities/filter';
import { Car } from '../entities/car';
import { VirtualTable } from '../entities/virtual.table';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TableService {
  hostUrl: string = 'http://localhost:5000/api'

  constructor(
    private http: HttpClient
  ) { }

  getCars(): Observable<Car[]> {
    return this.http.get<Car[]>(`${this.hostUrl}`);
  }

  getVirtualTables(): Observable<VirtualTable[]> {
    return this.http.get<VirtualTable[]>(`${this.hostUrl}/virtual-tables`);
  }

  getVirtualTable(id: number): Observable<Car[]> {
    return this.http.get<Car[]>(`${this.hostUrl}/virtual-tables/${id}/cars`);
  }

  saveNewTable(name: string, filters: Filter[], selectedFilials: string[]): Observable<VirtualTable> {
    var json = this.generateJson(name, filters, selectedFilials);
    const httpOptions = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }

    console.log(json);
    return this.http.post<VirtualTable>(`${this.hostUrl}/virtual-tables/add`, json, httpOptions);
  }

  deleteVirtualTable(id: number): Observable<any> {
    return this.http.delete(`${this.hostUrl}/virtual-tables/${id}`);
  }

  private generateJson(name: string, filters: Filter[], selectedFilials: string[]): string {
    var table = {
      tableName: name,
      filters: filters,
      filials: selectedFilials
    }
    return JSON.stringify(table);
  }
}
