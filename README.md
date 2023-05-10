# Parking Lot

1. This project is build using .NET 7.0 and Visual Studio 2022.
2. TablesAndFunctions.sql under Solution Items folder contains the necessary script for creating database and tables.
3. Run the sql script in Sql Server and Change the connection string in appsettings under Parking.UI project.
4. In Visual Studio, Mark Parking.UI project as startup project and Run it.
5. For UnitTests and PlaywrightTests, NUit is used here in this project.
6. In order to run PlaywrightTests, Parking.UI need to publish(in IIS/docker) and update the url in Setup method of PageTest classes.
7. PlaywrightTests can run from Test Explorer in visual studio or Windows Powershell
    

