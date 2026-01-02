import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Patient, CreatePatient, UpdatePatient, SearchResult, SearchRequest } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private apiUrl = 'http://localhost:5000/api/patients';

  constructor(private http: HttpClient) { }

  search(request: SearchRequest): Observable<SearchResult<Patient>> {
    let params = new HttpParams()
      .set('page', request.page.toString())
      .set('pageSize', request.pageSize.toString())
      .set('isDescending', request.isDescending.toString());

    if (request.keyword) {
      params = params.set('keyword', request.keyword);
    }
    if (request.sortBy) {
      params = params.set('sortBy', request.sortBy);
    }

    return this.http.get<SearchResult<Patient>>(`${this.apiUrl}/search`, { params });
  }

  getById(id: number): Observable<Patient> {
    return this.http.get<Patient>(`${this.apiUrl}/${id}`);
  }

  create(patient: CreatePatient): Observable<Patient> {
    return this.http.post<Patient>(this.apiUrl, patient);
  }

  update(id: number, patient: UpdatePatient): Observable<Patient> {
    return this.http.put<Patient>(`${this.apiUrl}/${id}`, patient);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
