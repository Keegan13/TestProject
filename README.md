 
## Requirements

1. [.Net core 2.2 SDK](https://www.microsoft.com/net/core#windows)
2. [Visual studio 2017](https://www.visualstudio.com/) OR [VSCode](https://code.visualstudio.com/) with [C#](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp) extension
3. [NodeJs](https://nodejs.org/en/) (Latest LTS)
4. [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2017) OR [MS SQL Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express)

## Installation
```
1. Clone/Download repo:
    git clone https://github.com/Keegan13/TestProject
2. Restore packages:
    cd %PROJECT_PATH%
    dotnet restore TestProject.sln
    cd WebHost/ClientApp
    npm install
3. Run project
    3.1 From console
      cd ./..
      dotnet run
    3.2 From VisualStudio
      F5 project "Host" 
4. Navigate to https://localhost:5001/ 
