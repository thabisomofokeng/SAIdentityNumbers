import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http"
import { Observable } from "rxjs";

@Injectable()
export class FileUploadService {

  constructor(private http: HttpClient, @Inject("BASE_API_URL") private baseUrl: string){}

  postFiles<T>(files: File[]): Observable<T> {
    const endpoint = this.baseUrl + "api/identityNumbers/validateUploadFiles";
    const formData = new FormData();
    for (let file of files) {
      if (file == null)
        continue;
      formData.append("files", file, file.name);
    }
    return this.http.post<T>(endpoint, formData);
  }
}
