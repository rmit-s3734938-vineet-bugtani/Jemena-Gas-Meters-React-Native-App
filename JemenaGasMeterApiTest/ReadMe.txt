RMIT University Project Semester 1 2020
Project Name: Jemena Gas Meter Life Cycle Tracking System
Developer Name: Rajarajeswari Rudhrakumar(Raji)
Project: JemenaGasMeter.ApiIntegrationTest

This test case is build using ASP.Net XUnit. 
Dependencies:
xunit.runner.visualstudio(2.4.1)
xunit(2.4.1)
Microsoft.AspNetCore.Hosting(2.2.7)
Microsoft.AspNetCore.Mvc.Core(2.2.5)
Microsoft.AspNetCore.TestHost(3.1.3)
Microsoft.EntityFrameworkCore.InMemory(3.1.3)

How to run the test case:
1) Run the attached "JemenaDbTest.sql". 
	This script creates database named, JemenaDbTest" and creates all the tables with test data. 
	
2) In the "appsettings.json" 
	DefaultConnection, set the database path as configured. 
	Example:
	 "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JemenaDbTest;Trusted_Connection=True;MultipleActiveResultSets=true"

3) Open Test Explorer => View->Test Explorer then click Run All Tests (Ctrl+R,A)

