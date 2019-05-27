# SA Identity Numbers Validator

ASP.NET Core 2.1 Web Api with AngularJS 5 front-end project for processing South African social security ID Numbers.

The project processes a list of one or more South African ID Numbers, each result is appended to either the valid or invalid result file on the server, and the results are displayed in a grid on the UI.

The web UI provides a facility to input a single ID number and also a mechanism to add more numbers to be submitted for validation at a single time. There is also a facility to upload one or more files containing a list of ID Numbers separated by a new line.

## Solution design (N-tier)

- Infrastracture Layer: IdentityNumber.Infrastructure includes data layer (Repository Pattern using CSVHelper) and Web Api project.
- Domain/Business Layer: IdentityNumber.Domain (Facade Pattern implementation using services)
- Presentation Layer: IdentityNumber.Presentation (AngularJS 5 using ASP.NET Core 2.1 Web Api back-end)

## Testing

- Solution includes unit tests for the Web Api
- IdentityNumber.Infrastructure.Api has two test files for identity numbers upload validation. The valid and invalid identity numbers are saved and read from the ValidIdentityNumbers.csv and InvalidIdentityNumbers.csv files respectively.

## Logging

- Log files are by default stored at C:\TEMP\SAIdentityNumbers. To change file location, go the appsettings.json and modify the Serilog WriteTo pathFormat to desired location.

## Dependencies

- CsvHelper: https://github.com/JoshClose/CsvHelper
- Serilog: https://github.com/serilog/serilog

## Setup

- Clone repository
- Open solution using Visual Studio 2017, Set Startup Projects to IdentityNumber.Infrastructure.Api and IdentityNumber.Presentation
- Press F5 to debug solution
