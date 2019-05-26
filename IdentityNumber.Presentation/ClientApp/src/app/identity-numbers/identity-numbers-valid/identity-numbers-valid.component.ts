import { Component, OnInit } from "@angular/core";
import { IValidIdentityNumber } from "./identity-number-valid";
import { IdentityNumberService } from "../identity-number.service";

@Component({
  selector: "app-identity-numbers-valid",
  templateUrl: "./identity-numbers-valid.component.html"
})
export class IdentityNumbersValidComponent implements OnInit {
  validIdentityNumbers: IValidIdentityNumber[];

  constructor(private identityNumberService: IdentityNumberService) {
  }

  ngOnInit() {
    this.getValidIdentityNumbers();
  }

  getValidIdentityNumbers() {
    this.identityNumberService.getValidIdentityNumbers<IValidIdentityNumber[]>().subscribe(result => {
      this.validIdentityNumbers = result;
    }, error => console.error(error));
  }
}
