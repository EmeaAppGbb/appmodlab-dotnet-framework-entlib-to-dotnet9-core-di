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
agenticTools: ["GitHub Copilot CLI"]
tags: ["enterprise-library", "dependency-injection", "logging", "ef-core", "dotnet-9"]
extensions: ["github.copilot"]
thumbnail: ""
video: ""
version: "1.0.0"
---

## Overview

This lab walks you through replacing Microsoft Enterprise Library patterns in a .NET Framework 4.8 healthcare claims processing application with native .NET 9 Core dependency injection, logging, exception handling, and data access patterns. You will modernize each layer of the application—swapping EntLib logging for Serilog, EntLib Data Access for Entity Framework Core, and XML-based configuration for the built-in .NET Core configuration and DI system.

![Application Architecture](assets/screenshots/03-architecture-diagram.png)
*High-level architecture of the claims processing application showing the key components targeted for modernization.*

## The Legacy Application

The existing application is a multi-project .NET Framework 4.8 solution that processes healthcare insurance claims. It relies heavily on Microsoft Enterprise Library for cross-cutting concerns including logging, exception handling, data access, and caching.

![Solution Structure](assets/screenshots/02-solution-structure.png)
*The solution is organized into multiple projects separating web, business logic, data access, and shared infrastructure layers.*

Enterprise Library is deeply embedded across the codebase, with dependencies on `Microsoft.Practices.EnterpriseLibrary.Logging`, `ExceptionHandling`, and `Data` packages wired through XML configuration files.

![Enterprise Library Dependencies](assets/screenshots/02-entlib-dependencies.png)
*NuGet package references showing the Enterprise Library dependencies that will be replaced with native .NET Core equivalents.*

The claims workflow spans multiple layers—from the ASP.NET web front-end through business rules validation and into SQL Server persistence—all coordinated through EntLib's block-based patterns.

![Claims Workflow](assets/screenshots/04-claims-workflow.png)
*End-to-end claims processing workflow illustrating how data flows through the application layers.*

## Getting Started

Before beginning the migration, verify that the legacy application compiles successfully. The solution builds with VS 2022 Build Tools (MSBuild 17.14) targeting .NET Framework 4.8.

![MSBuild Release Build](assets/screenshots/01-msbuild-release-build.png)
*A successful release build confirms the starting point is stable and ready for migration.*

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

## Solution

The completed migration is available on the [`solution-final`](../../tree/solution-final) branch with step-by-step tagged commits:

| Tag | Description |
|-----|-------------|
| `step-01-explore-legacy` | Analyze legacy EntLib usage across the codebase |
| `step-02-map-entlib` | Map EntLib patterns to .NET 9 equivalents |
| `step-03-migration-plan` | Create phased migration plan |
| `step-04-migrate-logging` | Replace EntLib Logging with Serilog + ILogger<T> |
| `step-05-migrate-data` | Replace EntLib DAAB with Entity Framework Core 9 |
| `step-06-migrate-exceptions` | Replace EntLib Exception Handling with try/catch + ILogger |
| `step-07-setup-di` | Replace Unity with ASP.NET Core native DI |
| `step-08-build-validate` | Build succeeds with zero errors, all EntLib removed |

Each step output is documented in `assets/outputs/step-NN.txt` on the solution branch.
