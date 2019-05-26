import { Component, OnInit, AfterViewInit, ViewChild } from "@angular/core";
import { IIdentityNumberListModel } from "../identity-number-list";
import { FileUploadService } from "./identity-number-upload.service";
import { IdentityNumbersValidComponent } from "../identity-numbers-valid/identity-numbers-valid.component";
import { IdentityNumbersInvalidComponent } from "../identity-numbers-invalid/identity-numbers-invalid.component";

@Component({
  selector: "app-identity-number-upload",
  templateUrl: "./identity-number-upload.component.html"
})
export class IdentityNumberUploadComponent implements OnInit, AfterViewInit {
  fileUploads = [null] ;

  @ViewChild(IdentityNumbersValidComponent)
  private identityNumbersValidComponent: IdentityNumbersValidComponent;
  @ViewChild(IdentityNumbersInvalidComponent)
  private identityNumbersInvalidComponent: IdentityNumbersInvalidComponent;

  constructor(private fileUploadService: FileUploadService
  ) {
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
  }

  handleFileInput(files: FileList, index: number) {
    this.fileUploads[index] = files.item(0);
  }

  addFileUpload() {
    this.fileUploads.push(null);
  }

  removeFileUpload(index: number) {
    this.fileUploads.splice(index, 1);
  }

  uploadAndValidateFiles() {
    this.fileUploadService.postFiles<IIdentityNumberListModel>(this.fileUploads).subscribe(result => {
      this.identityNumbersValidComponent.validIdentityNumbers =
        this.identityNumbersValidComponent.validIdentityNumbers.concat(result.validIdentityNumbers);
      this.identityNumbersInvalidComponent.invalidIdentityNumbers =
        this.identityNumbersInvalidComponent.invalidIdentityNumbers.concat(result.invalidIdentityNumbers);
      },
      error => {
        console.log(error);
      });
  }

}
