import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UnifiedSearchResult } from '../models/models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UnifiedSearchService {
  private apiUrl = `${environment.apiUrl}/search`;

  constructor(private http: HttpClient) { }

  unifiedSearch(keyword: string, page: number = 1, pageSize: number = 10): Observable<UnifiedSearchResult> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (keyword && keyword.trim()) {
      params = params.set('keyword', keyword.trim());
    }

    return this.http.get<UnifiedSearchResult>(`${this.apiUrl}/unified`, { params });
  }
}
