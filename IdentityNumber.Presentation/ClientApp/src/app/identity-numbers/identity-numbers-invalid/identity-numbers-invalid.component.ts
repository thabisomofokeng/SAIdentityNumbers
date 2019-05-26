import { Component, OnInit } from "@angular/core";
import { IInvalidIdentityNumber } from "./identity-number-invalid";
import { IdentityNumberService } from "../identity-number.service";

@Component({
  selector: "app-identity-numbers-invalid",
  templateUrl: "./identity-numbers-invalid.component.html"
})
export class IdentityNumbersInvalidComponent implements OnInit {
  invalidIdentityNumbers: IInvalidIdentityNumber[];

  constructor(private identityNumberService: IdentityNumberService) {
  }

  ngOnInit() {
    this.getInvalidIdentityNumbers();
  }

  getInvalidIdentityNumbers() {
    this.identityNumberService.getInvalidIdentityNumbers<IInvalidIdentityNumber[]>().subscribe(result => {
      this.invalidIdentityNumbers = result;
    }, error => console.error(error));
  }
}
