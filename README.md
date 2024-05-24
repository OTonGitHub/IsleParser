# IsleParser

A quick and dirty scraper to grab and store articles from some maldivian news sites. Stores every article to a single table, by no means the best or performant scraper out there. Code quality and architecture updates set for v2.

# VSCode

- ctrl + b

### Project Structure

- Core Library.
- Console Interface.
- Playwright config for Proxy & Headless browser, deals with dynamic content loading.
- HtmlAgilityPack for parsing and querying HTML.
- Stored in portable SQLite file.
- Startup tests in Playwright.NUnit.

```
IsleParser/
│
├── IsleParser.Domain/
│   ├── IsleParser.Domain.csproj
│   ├── Entities/
│   │   └── Article.cs
│   └── Interfaces/
│       ├── IArticleRepository.cs
│       └── IIsleParserService.cs
│
├── IsleParser.Application/
│   ├── IsleParser.Application.csproj
│   ├── Services/
│   │   ├── ArticleService.cs
│   │   └── IIsleParserService.cs
│   ├── Interfaces/
│   │   └── IArticleService.cs
│   └── DependencyInjection.cs
│
├── IsleParser.Infrastructure/
│   ├── IsleParser.Infrastructure.csproj
│   ├── Services/
│   │   └── IsleParserService.cs
│   ├── Persistence/
│   │   └── ArticleRepository.cs
│   └── DependencyInjection.cs
│
├── IsleParserConsole/
│   ├── IsleParserConsole.csproj
│   └── Program.cs
│
├── IsleParserTests/
│   ├── IsleParserTests.csproj
│   ├── Application/
│   │   └── ArticleServiceTests.cs
│   └── Infrastructure/
│       └── IsleParserServiceTests.cs
│
└── IsleParserSolution.sln
```

### Project Creation Commands

- #### Project Init
  - `dotnet new sln -n IsleParser`
  - `mksdir src`
    > move Directory.Build.props into src.
  - `dotnet new gitignore`
  - `dotnet new console -lang "c#" -n "Console-UI" -f "net8.0" -o src/Console-UI -d -v diag`
    - Note here, `.\` was not used when sepcifying output directory, usually, just -n is enough,
      it will automatically create directory, use -o if you want folder name to be different,
      or when grouping projects under specific folders.
  - `dotnet sln IsleParser.sln add src/Console-UI`
    - Will add `.csproj` to sln automatically.
  - add Directory.Build.props and .editoconfig files for static code analysis.
  - `dotnet watch --no-hot-reload --project .NET-Server/src/Console-UI run --environment "Development"`
    - `run` command conatins `--launch-profile "https"` flag too, watch might have.
- #### Nuget
  - `dotnet add src/Console-UI package Microsoft.Playwright`
  - `dotnet add src/Console-UI package HtmlAgilityPack`
- #### Playwright
  - `pwsh .NET-Server/src/Console-UI/bin/Debug/net8.0/playwright.ps1  install`
  - `pwsh .NET-Server/src/Console-UI/bin/Debug/net8.0/playwright.ps1  install-deps`
