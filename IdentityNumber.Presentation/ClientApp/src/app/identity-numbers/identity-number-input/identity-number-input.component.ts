import { Component, OnInit, AfterViewInit, ViewChild } from "@angular/core";
import { IIdentityNumberListModel } from "../identity-number-list";
import { IdentityNumberService } from "../identity-number.service";
import { IdentityNumbersValidComponent } from "../identity-numbers-valid/identity-numbers-valid.component";
import { IdentityNumbersInvalidComponent } from "../identity-numbers-invalid/identity-numbers-invalid.component";

@Component({
  selector: "app-identity-number-input",
  templateUrl: "./identity-number-input.component.html"
})
export class IdentityNumberInputComponent implements OnInit, AfterViewInit {
  identityNumbers: string[] = [];
  error = null;
  success = null;

  @ViewChild(IdentityNumbersValidComponent)
  private identityNumbersValidComponent: IdentityNumbersValidComponent;
  @ViewChild(IdentityNumbersInvalidComponent)
  private identityNumbersInvalidComponent: IdentityNumbersInvalidComponent;

  constructor(private identityNumberService: IdentityNumberService) {
  }

  ngOnInit() {
    this.identityNumbers.push(null);
  }

  ngAfterViewInit(): void {
  }

  addIdentityNumber() {
    this.identityNumbers.push(null);
  }

  removeIdentityNumber(index: number) {
    this.identityNumbers.splice(index, 1);
  }

  validateIdentityNumbers() {
    this.identityNumberService.validateIdentityNumbers<IIdentityNumberListModel>(this.identityNumbers).subscribe(
      result => {
        this.success = result;
        this.identityNumbersValidComponent.validIdentityNumbers =
          this.identityNumbersValidComponent.validIdentityNumbers.concat(result.validIdentityNumbers);
        this.identityNumbersInvalidComponent.invalidIdentityNumbers =
          this.identityNumbersInvalidComponent.invalidIdentityNumbers.concat(result.invalidIdentityNumbers);
      },
      error => {
        this.error = error;
        console.log(error);
      });
  }

}
