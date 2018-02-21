# CuMaster
MSIS capstone project for CIS9590 - Information Systems Development Project at Baruch Collage

Note that this is not a full working system, and should not be taken as such.

Completed features are:
1) Currency conversion calculator
2) Saving conversions
3) User signup and administration
4) Rate listing report with chart
5) Batch job (console application) to call for and save currency rates
6) User set defaults
7) Rate change email alert sign up 

Written in C#, .NET Framework 4.6.1, MVC 5 with Razor.
Front-End: JQuery 3.2.1, Bootstrap 3.3.7, DataTables 1.10.15, C3 0.4.8, other small helper libraries (Moment.js, JQuery.Validation, etc.).
Data layer via Entity Framework 6.
Utalizes DI container Ninject.

Built against custom Microsoft SQL Server database (not included in this repository).
