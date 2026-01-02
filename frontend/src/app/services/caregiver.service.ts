import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Caregiver, SearchResult, SearchRequest } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class CaregiverService {
  private apiUrl = 'http://localhost:5000/api/caregivers';

  constructor(private http: HttpClient) { }

  search(request: SearchRequest): Observable<SearchResult<Caregiver>> {
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

    return this.http.get<SearchResult<Caregiver>>(`${this.apiUrl}/search`, { params });
  }

  getById(id: number): Observable<Caregiver> {
    return this.http.get<Caregiver>(`${this.apiUrl}/${id}`);
  }
}
