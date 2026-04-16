---
title: "EntLib to .NET 9 DI Migration"
description: "Replace Microsoft Enterprise Library with native .NET Core patterns—logging, DI, exception handling & data access"
authors: ["marconsilva"]
category: "Code Modernization"
industry: "Healthcare & Life Sciences"
services: ["Azure App Service", "Azure SQL Database", "Azure Redis Cache", "Application Insights"]
languages: ["C#", ".NET"]
frameworks: ["ASP.NET Core", "Entity Framework Core", "Serilog"]
modernizationTools: []
agenticTools: []
tags: ["enterprise-library", "dependency-injection", "logging", "ef-core", "dotnet-9"]
extensions: ["github.copilot"]
thumbnail: ""
video: ""
version: "1.0.0"
---

## Initial Application Screenshots

> **Note:** This is a .NET Framework 4.8 application that requires IIS/IIS Express to run. It was successfully built using VS 2022 Build Tools (MSBuild 17.14) with the .NET Framework 4.8 targeting pack. The app cannot run standalone — it requires IIS hosting, which is not available in this environment.

### MSBuild Release Build (VS 2022 Build Tools)
![MSBuild Release Build](assets/screenshots/01-msbuild-release-build.png)

### Solution Structure
![Solution Structure](assets/screenshots/02-solution-structure.png)

### Project Structure
![Project Structure](assets/screenshots/01-project-structure.png)

### Enterprise Library Dependencies
![Enterprise Library Dependencies](assets/screenshots/02-entlib-dependencies.png)

### Application Architecture
![Application Architecture](assets/screenshots/03-architecture-diagram.png)

### Claims Processing Workflow & Build Requirements
![Claims Workflow](assets/screenshots/04-claims-workflow.png)
