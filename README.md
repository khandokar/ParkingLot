# Parking Lot

1. This project is build using .NET 7.0 and Visual Studio 2022.
2. TablesAndFunctions.sql under Solution Items folder contains the necessary script for creating database and tables.
3. Run the sql script in Sql Server and Change the connection string in appsettings under Parking.UI project.
4. In Visual Studio, Mark Parking.UI project as startup project and Run it.
5. For UnitTests and PlaywrightTests, NUit is used here in this project.
6. In order to run PlaywrightTests, Run Parking.UI, copy the url and update the url in Setup method of PageTest classes in PlaywrightTests project.
7. Open Powershell and go the PlaywrightTests project directory and first run $env:HEADED="1" and then run dotnet test.  
    

