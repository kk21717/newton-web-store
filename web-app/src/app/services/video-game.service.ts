import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { VideoGame, CreateVideoGameRequest, UpdateVideoGameRequest, PagedResult } from '../models/video-game.model';

@Injectable({
  providedIn: 'root'
})
export class VideoGameService {
  private readonly apiUrl = 'http://localhost:5000/api/videogames';

  constructor(private http: HttpClient) {}

  getAll(): Observable<VideoGame[]> {
    return this.http.get<VideoGame[]>(this.apiUrl);
  }

  getAllPaged(pageNumber: number, pageSize: number): Observable<PagedResult<VideoGame>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<PagedResult<VideoGame>>(this.apiUrl, { params });
  }

  getById(id: number): Observable<VideoGame> {
    return this.http.get<VideoGame>(`${this.apiUrl}/${id}`);
  }

  search(term: string): Observable<VideoGame[]> {
    return this.http.get<VideoGame[]>(`${this.apiUrl}/search`, {
      params: { term }
    });
  }

  searchPaged(term: string, pageNumber: number, pageSize: number): Observable<PagedResult<VideoGame>> {
    const params = new HttpParams()
      .set('term', term)
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<PagedResult<VideoGame>>(`${this.apiUrl}/search`, { params });
  }

  getByGenre(genre: string): Observable<VideoGame[]> {
    return this.http.get<VideoGame[]>(`${this.apiUrl}/genre/${encodeURIComponent(genre)}`);
  }

  getByGenrePaged(genre: string, pageNumber: number, pageSize: number): Observable<PagedResult<VideoGame>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<PagedResult<VideoGame>>(`${this.apiUrl}/genre/${encodeURIComponent(genre)}`, { params });
  }

  getByPlatform(platform: string): Observable<VideoGame[]> {
    return this.http.get<VideoGame[]>(`${this.apiUrl}/platform/${encodeURIComponent(platform)}`);
  }

  getByPlatformPaged(platform: string, pageNumber: number, pageSize: number): Observable<PagedResult<VideoGame>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<PagedResult<VideoGame>>(`${this.apiUrl}/platform/${encodeURIComponent(platform)}`, { params });
  }

  create(request: CreateVideoGameRequest): Observable<VideoGame> {
    return this.http.post<VideoGame>(this.apiUrl, request);
  }

  update(id: number, request: UpdateVideoGameRequest): Observable<VideoGame> {
    return this.http.put<VideoGame>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
