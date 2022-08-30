# Shepherd

Shepherd is an issue tracking application used to organize tasks and projects.

## Installation

This application uses C# and ASP.NET Core version 3.1.

To check your current version and a list of SDKs that are downloaded:
```bash
dotnet --version
```
```bash
dotnet --list-sdks
```

To Install version 3.1 go to this link [https://dotnet.microsoft.com/en-us/download/dotnet/3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)

If you have a different version of .NET installed, be sure to have a global.json file on the project level and include:
```bash
{
    "sdk": {
        "version": "3.1.422"
    }
}
```

Clone the repository:

```bash
git clone https://github.com/Springer114/Shepherd.git
```

### MySQL

To check if it is installed:
```bash
mysql -u root -p
```

To install, visit [https://dev.mysql.com/downloads/installer/](https://dev.mysql.com/downloads/installer/)

After installation
- Create an appsettings.json file at the project level
- In appsettings.json, add the following code and replace server, userid, password, port, and database name for yours.

```bash
{
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "AllowedHosts": "*",
      "DBInfo":
      {
          "Name": "MySQLconnect",
          "ConnectionString": "server=localhost;userid=root;password=YOUR_PW;port=YOUR_PORT;database=DB_NAME;SslMode=None"
      }
  }
```

### Entity Framework

A modern object relational mapper that helps support LINQ queries, change tracking, updates and schema migrations.

To check if it is installed:
```bash
dotnet ef
```
To install:
```bash
dotnet tool install --global dotnet-ef
```
Update to most recent version:
```bash
dotnet tool update --global dotnet-ef
```

#### Dependencies

Required to run certain commands on a project:

```bash
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 3.1.1
dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.1.5
```

Run the lines below to make migrations:
```bash
dotnet ef migrations add FirstMigration
dotnet ef database update
```

Run at the project level:
```bash
dotnet run
```

Open localhost:5000 in your browser and begin organizing your tasks and projects.