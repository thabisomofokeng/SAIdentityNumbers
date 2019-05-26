import { IValidIdentityNumber } from "./identity-numbers-valid/identity-number-valid";
import { IInvalidIdentityNumber } from "./identity-numbers-invalid/identity-number-invalid";

export interface IIdentityNumberListModel {
  validIdentityNumbers: IValidIdentityNumber[];
  invalidIdentityNumbers: IInvalidIdentityNumber[];
}
