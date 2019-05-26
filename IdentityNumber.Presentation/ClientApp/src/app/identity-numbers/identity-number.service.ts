import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http"
import { Observable } from "rxjs";

@Injectable()
export class IdentityNumberService {

  constructor(private http: HttpClient, @Inject("BASE_API_URL") private baseUrl: string){}

  getValidIdentityNumbers<T>() {
    return this.http.get<T>(this.baseUrl + "api/identityNumbers/valid");
  }

  getInvalidIdentityNumbers<T>() {
    return this.http.get<T>(this.baseUrl + "api/identityNumbers/invalid");
  }

  validateIdentityNumbers<T>(identityNumbers: string[]): Observable<T> {
    return this.http.post<T>(this.baseUrl + "api/identityNumbers", identityNumbers);
  }
}
