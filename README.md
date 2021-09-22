# Dashboards API

Dashboard is a web application that helps the user manage their dashboards. From the dashboard page, the user can choose a layout and add desired widgets that are supported by the system

### Setup and run program

* Prerequisite
  1. .Net 5.0
     - `Download`: https://dotnet.microsoft.com/download
  2. Sql Server (at least 2017)
     - `Download`: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
  3. Entity Framework Core tools reference - .NET Core CLI
     - `bash` : `dotnet tool install --global dotnet-ef`

* Setup and run
  1. Configure Connection String
     - Open `appsettings.json` in `Dashboard.API` directory
     - Change follow format: "SqlServer": "Server = **[Server name]**; Database = Dashboard; User Id = **[User]**; Password = **[Password]**;"
     - Save

  2. Update database: `bash` : `dotnet ef database update`

  3. Build project:
     1. Change directory to `~/Dashboard.API` and open terminal | shell | command Prompt
     2. `bash` : `dotnet build`

  4. Run project: `bash` : `dotnet run`
     - Server:
       - https://localhost:5001
       - http://localhost:5000
     - Swagger:
       - https://localhost:5001/swagger/index.html