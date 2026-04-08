```
███████╗███╗   ██╗████████╗██╗     ██╗██████╗     ██████╗  ██████╗ ███████╗███████╗
██╔════╝████╗  ██║╚══██╔══╝██║     ██║██╔══██╗    ██╔══██╗██╔═══██╗██╔════╝██╔════╝
█████╗  ██╔██╗ ██║   ██║   ██║     ██║██████╔╝    ██████╔╝██║   ██║███████╗███████╗
██╔══╝  ██║╚██╗██║   ██║   ██║     ██║██╔══██╗    ██╔══██╗██║   ██║╚════██║╚════██║
███████╗██║ ╚████║   ██║   ███████╗██║██████╔╝    ██████╔╝╚██████╔╝███████║███████║
╚══════╝╚═╝  ╚═══╝   ╚═╝   ╚══════╝╚═╝╚═════╝     ╚═════╝  ╚═════╝ ╚══════╝╚══════╝
                                                                                     
                    ██████╗ ███████╗███████╗███████╗ █████╗ ████████╗███████╗██████╗ 
                    ██╔══██╗██╔════╝██╔════╝██╔════╝██╔══██╗╚══██╔══╝██╔════╝██╔══██╗
                    ██║  ██║█████╗  █████╗  █████╗  ███████║   ██║   █████╗  ██║  ██║
                    ██║  ██║██╔══╝  ██╔══╝  ██╔══╝  ██╔══██║   ██║   ██╔══╝  ██║  ██║
                    ██████╔╝███████╗██║     ███████╗██║  ██║   ██║   ███████╗██████╔╝
                    ╚═════╝ ╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═════╝ 
```

<div align="center">

# 🕹️ LEGACY SLAYER: ENTERPRISE LIBRARY EDITION 🕹️

### *Modernize .NET Framework Enterprise Library to .NET 9 Core DI*

```
┌─────────────────────────────────────────────────────────────┐
│                                                             │
│  ▓▓▓▓▓▓▓▓▓░░░░░░░░░  BOSS HEALTH: ENTERPRISE LIBRARY      │
│                                                             │
│  [💀 Logging Block]  [💀 Exception Block]  [💀 DAAB]       │
│  [💀 Unity]  [💀 Validation Block]  [💀 WCF]               │
│                                                             │
│           🎯 PLAYER: .NET 9 Core + Modern DI               │
│           ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  HP: MAXIMUM         │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

**🎮 DIFFICULTY:** ⭐⭐⭐⭐⭐ (5/5 - Final Boss)  
**⏱️ ESTIMATED PLAYTIME:** 5-7 hours  
**🏆 ACHIEVEMENT UNLOCKED:** "EntLib Destroyer"

</div>

---

## 🌟 OVERVIEW

Welcome to the **ultimate boss battle** of .NET modernization! 💥 In this arcade-style adventure, you'll face the legendary **ENTERPRISE LIBRARY** — a relic from the 2008-2015 era that once ruled the .NET Framework kingdom with its XML-laden configuration files and abstraction layers upon abstraction layers.

**YOUR MISSION:** Transform Alpine Financial Services' expense management system from a .NET Framework 4.8 behemoth loaded with EntLib blocks into a sleek, modern .NET 9 masterpiece using native Core patterns! 🚀

### 💼 The Business Domain
You're working on an internal **expense reporting system** where:
- 💰 Employees submit expenses
- ✅ Managers approve them
- 💸 Finance processes reimbursements
- 🔗 Payroll system integration handles payments

But there's a problem... **EVERYTHING** uses Enterprise Library! 😱

---

## 🎯 WHAT YOU'LL LEARN

Prepare to level up your modernization skills! 🎓

- 🔍 **Identify and catalog** Enterprise Library dependencies lurking in legacy codebases
- 📝 **Replace EntLib Logging Application Block** with `Microsoft.Extensions.Logging` + Serilog
- 💥 **Replace EntLib Exception Handling policies** with ASP.NET Core exception middleware
- 🗄️ **Migrate from Data Access Application Block (DAAB)** to Entity Framework Core
- 🔌 **Replace Unity container extensions** with built-in dependency injection
- ✅ **Modernize validation** from EntLib Validation Block to FluentValidation
- 🌐 **Convert WCF service clients** to HttpClientFactory with Polly resilience
- 🔐 **Upgrade authentication** from Windows Auth to Azure AD/Entra ID
- ☁️ **Deploy to Azure** with App Service, Redis Cache, and Application Insights

---

## 🛠️ PREREQUISITES

**Power Up Your Arsenal Before Battle!** ⚔️

### Required Skills
- 💪 Strong C# and .NET Framework experience
- 🤔 Familiarity with Enterprise Library concepts (helpful but not required)

### Required Tools
- 🔧 .NET Framework 4.8 Developer Pack
- 🆕 .NET 9 SDK
- 🎨 Visual Studio 2022
- 💾 SQL Server LocalDB or Azure SQL Database
- ☁️ Azure subscription (for deployment and Redis Cache)

---

## 🚀 QUICK START

**INSERT COIN TO BEGIN** 🪙

```bash
# Clone the repository
git clone https://github.com/EmeaAppGbb/appmodlab-dotnet-framework-entlib-to-dotnet9-core-di.git
cd appmodlab-dotnet-framework-entlib-to-dotnet9-core-di

# Checkout the legacy branch to start
git checkout legacy

# Open in Visual Studio
start AlpineExpenseManager.sln

# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build

# Run database migrations (LocalDB)
# SQL scripts are in the Database folder

# Start the API
dotnet run --project AlpineExpenseManager.WebApi
```

🎮 **PRO TIP:** Use the `main` branch to review the completed lab, or jump to step branches to see incremental progress!

---

## 📁 PROJECT STRUCTURE

```
AlpineExpenseManager/
├── 📄 AlpineExpenseManager.sln
├── 🌐 AlpineExpenseManager.WebApi/
│   ├── ⚙️ Web.config                       # 💀 300+ lines of EntLib XML hell
│   ├── 🚀 Global.asax.cs                   # EntLib bootstrapper
│   ├── 📁 App_Start/
│   │   ├── WebApiConfig.cs              # Web API routing
│   │   └── UnityConfig.cs               # 💀 Unity + EntLib registration
│   ├── 🎮 Controllers/
│   │   ├── ExpenseController.cs         # Expense CRUD (uses DAAB)
│   │   ├── ApprovalController.cs        # Approval workflow
│   │   ├── ReimbursementController.cs   # Finance processing
│   │   └── ReportController.cs          # Expense reports
│   └── 🛡️ Filters/
│       └── EntLibExceptionFilter.cs     # 💀 Exception policies
├── 💼 AlpineExpenseManager.Core/
│   ├── 🔧 Services/
│   │   ├── ExpenseService.cs            # Business logic + EntLib logging
│   │   ├── ApprovalService.cs           # Approval rules
│   │   └── NotificationService.cs       # WCF client for payroll
│   ├── 💾 DataAccess/
│   │   ├── ExpenseRepository.cs         # 💀 DAAB-based data access
│   │   ├── ApprovalRepository.cs        # 💀 Stored procedures via DAAB
│   │   └── DatabaseFactory.cs           # EntLib Database factory
│   ├── ✅ Validation/
│   │   └── ExpenseValidators.cs         # 💀 EntLib Validation attributes
│   └── 📝 Logging/
│       └── LoggingHelper.cs             # 💀 Static EntLib LogWriter wrapper
├── 🔌 AlpineExpenseManager.WcfContracts/
│   ├── IPayrollService.cs               # 💀 WCF service contract
│   └── PayrollServiceClient.cs          # 💀 Generated WCF proxy
└── 🧪 AlpineExpenseManager.Tests/
    └── Services/                        # Tests with EntLib test doubles
```

---

## ⚡ THE LEGACY STACK (BOSS STATS)

**Know Your Enemy!** 👾

| 💀 LEGACY BOSS | 📊 POWER LEVEL | 🎯 WEAKNESS |
|---------------|---------------|-------------|
| **Logging Application Block** | Flat file + database logging | ILogger<T> + Serilog |
| **Exception Handling Block** | Named policies, swallows exceptions | Middleware + ProblemDetails |
| **Data Access Block (DAAB)** | ADO.NET + stored procedures | Entity Framework Core |
| **Validation Block** | Attribute-based validation | FluentValidation |
| **Caching Block** | In-memory, no invalidation | IMemoryCache + Redis |
| **Unity Container** | EntLib's preferred DI | Built-in DI |
| **WCF Services** | SOAP, generated proxies | HttpClientFactory + Polly |

### 🔥 Anti-Patterns You'll Encounter

```xml
<!-- 300+ lines of XML configuration in web.config 😱 -->
<enterpriseLibrary.ConfigurationSource>
  <sources>
    <add name="System Configuration Source" 
         type="Microsoft.Practices.EnterpriseLibrary..." />
  </sources>
</enterpriseLibrary.ConfigurationSource>
```

- 📜 **XML Config Hell:** 300+ lines of EntLib configuration
- 🧱 **Static Wrappers:** Untestable LogWriter static classes
- 🕳️ **Silent Failures:** Exception policies that swallow errors
- 🎭 **Triple Abstraction:** DAAB wrapping ADO.NET wrapping stored procedures
- 🔌 **WCF Spaghetti:** Generated proxy classes everywhere

---

## 🎯 TARGET ARCHITECTURE (YOUR WEAPONS)

**Modern .NET 9 Arsenal!** ⚔️✨

| 🎯 MODERN WEAPON | 💪 CAPABILITY | 🌟 BENEFIT |
|-----------------|--------------|-----------|
| **Microsoft.Extensions.Logging** | Built-in logging abstraction | Clean, testable logging |
| **Serilog** | Structured logging | JSON logs, multiple sinks |
| **ASP.NET Core Middleware** | Global exception handling | RFC 7807 ProblemDetails |
| **Entity Framework Core 9** | Modern ORM with LINQ | Type-safe queries, migrations |
| **FluentValidation** | Fluent validation rules | Readable, composable validation |
| **Built-in DI** | Microsoft.Extensions.DependencyInjection | No third-party containers needed |
| **HttpClientFactory** | Typed HTTP clients | Resilient, testable HTTP calls |
| **Polly** | Resilience policies | Retry, circuit breaker, timeout |
| **Options Pattern** | Strongly-typed config | appsettings.json → POCOs |
| **Azure Redis Cache** | Distributed caching | Scalable, invalidation strategies |
| **Entra ID** | Modern auth with JWT | Claims-based identity |
| **Application Insights** | Observability | Distributed tracing, metrics |

### 🏗️ Architecture Overview

Every Enterprise Library block gets **DESTROYED** and replaced with its modern equivalent:

```
┌─────────────────────┐        ┌─────────────────────┐
│   LEGACY (💀)       │   🔥   │   MODERN (✨)       │
├─────────────────────┤  ───>  ├─────────────────────┤
│ Logging Block       │        │ ILogger<T> + Serilog│
│ Exception Block     │        │ Middleware          │
│ DAAB                │        │ EF Core             │
│ Unity               │        │ Built-in DI         │
│ Validation Block    │        │ FluentValidation    │
│ WCF Client          │        │ HttpClientFactory   │
│ Windows Auth        │        │ Entra ID + JWT      │
│ XML Config          │        │ Options Pattern     │
└─────────────────────┘        └─────────────────────┘
```

---

## 🕹️ LAB WALKTHROUGH

**SELECT YOUR CHARACTER: COPILOT CLI** 🤖

This lab is designed to be completed using **GitHub Copilot CLI** as your co-pilot! 🚁

### 📖 Branch Strategy

Follow the progressive difficulty curve:

```
legacy  →  step-1  →  step-2  →  step-3  →  step-4  →  step-5  →  step-6  →  solution
  💀        ⚡        💥        🗄️        🔌        🌐        ☁️         ✅
```

| 🎮 LEVEL | 🌿 BRANCH | 🎯 OBJECTIVE |
|---------|----------|-------------|
| **START** | `legacy` | The EntLib-heavy .NET Framework 4.8 app |
| **LEVEL 1** | `step-1-remove-logging-block` | Replace EntLib Logging with ILogger<T> + Serilog |
| **LEVEL 2** | `step-2-remove-exception-block` | Replace Exception Handling Block with middleware |
| **LEVEL 3** | `step-3-remove-daab` | Replace DAAB with EF Core |
| **LEVEL 4** | `step-4-remove-unity` | Replace Unity with built-in DI |
| **LEVEL 5** | `step-5-remove-wcf` | Replace WCF with HttpClientFactory |
| **LEVEL 6** | `step-6-modern-auth-and-deploy` | Entra ID auth + Azure deployment |
| **WIN!** | `solution` | Fully modernized .NET 9 application ✨ |

### 🎯 Step-by-Step Gameplay

#### 🔍 **LEVEL 0: Audit EntLib Usage**
```bash
# Survey the battlefield
gh copilot suggest "Find all Enterprise Library NuGet packages"
gh copilot suggest "Search for all EntLib logging usage in the codebase"
gh copilot suggest "List all Unity container registrations"
```

**🎯 Goal:** Catalog all EntLib dependencies, config blocks, and usage patterns

---

#### ⚡ **LEVEL 1: Destroy the Logging Block**
```bash
git checkout step-1-remove-logging-block

# Replace static LogWriter with ILogger<T>
gh copilot suggest "Replace EntLib LogWriter with ILogger<T>"
gh copilot suggest "Configure Serilog with file and Application Insights sinks"
gh copilot suggest "Remove all EntLib logging configuration from web.config"
```

**🎯 Goal:** Zero LogWriter references, ILogger<T> everywhere, structured JSON logs

---

#### 💥 **LEVEL 2: Crush the Exception Handling Block**
```bash
git checkout step-2-remove-exception-block

# Remove exception policies
gh copilot suggest "Remove EntLib exception policies"
gh copilot suggest "Create global exception middleware with ProblemDetails"
gh copilot suggest "Return RFC 7807 error responses"
```

**🎯 Goal:** No exception swallowing, proper HTTP error responses

---

#### 🗄️ **LEVEL 3: Obliterate the DAAB**
```bash
git checkout step-3-remove-daab

# Replace stored procedures with EF Core
gh copilot suggest "Create EF Core DbContext for expense entities"
gh copilot suggest "Map stored procedure calls to LINQ queries"
gh copilot suggest "Generate EF Core migrations from existing schema"
```

**🎯 Goal:** Zero DAAB references, type-safe LINQ queries, EF Core migrations

---

#### 🔌 **LEVEL 4: Eliminate Unity**
```bash
git checkout step-4-remove-unity

# Use built-in DI
gh copilot suggest "Register all services in Program.cs using built-in DI"
gh copilot suggest "Remove Unity container and UnityConfig"
gh copilot suggest "Convert Unity lifetime managers to DI service lifetimes"
```

**🎯 Goal:** No Unity references, all services in native DI container

---

#### 🌐 **LEVEL 5: Annihilate WCF**
```bash
git checkout step-5-remove-wcf

# Modern HTTP clients
gh copilot suggest "Replace WCF service client with typed HttpClient"
gh copilot suggest "Add Polly retry policies for resilience"
gh copilot suggest "Configure HttpClientFactory in DI"
```

**🎯 Goal:** No WCF proxies, resilient HTTP calls with Polly

---

#### ☁️ **LEVEL 6: Modern Auth & Azure Deploy**
```bash
git checkout step-6-modern-auth-and-deploy

# Final modernization
gh copilot suggest "Add Entra ID JWT authentication"
gh copilot suggest "Create Bicep templates for Azure resources"
gh copilot suggest "Deploy to Azure App Service with Redis Cache"
```

**🎯 Goal:** JWT auth, deployed to Azure, Application Insights enabled

---

### 🏆 VICTORY CONDITION

When you reach the `solution` branch, you should have:

✅ **ZERO** Enterprise Library NuGet packages  
✅ Serilog producing structured JSON logs  
✅ EF Core migrations matching database schema  
✅ FluentValidation rules equivalent to EntLib validation  
✅ Middleware returning proper ProblemDetails  
✅ HttpClientFactory with Polly policies  
✅ Entra ID authentication working  
✅ Deployed to Azure App Service  
✅ Application Insights collecting telemetry  

```
╔════════════════════════════════════════════════╗
║                                                ║
║   🎉🎉🎉  BOSS DEFEATED! 🎉🎉🎉                 ║
║                                                ║
║   ENTERPRISE LIBRARY HAS BEEN SLAIN!           ║
║                                                ║
║   Achievement Unlocked:                        ║
║   🏆 "LEGACY SLAYER"                           ║
║                                                ║
║   You have mastered the art of .NET            ║
║   modernization! EntLib is no more! 💀         ║
║                                                ║
╚════════════════════════════════════════════════╝
```

---

## ⏱️ DURATION

**⏰ ESTIMATED PLAYTIME:** 5-7 hours

### Time Breakdown by Level
- 🔍 Level 0 (Audit): 30-45 min
- ⚡ Level 1 (Logging): 45-60 min
- 💥 Level 2 (Exceptions): 45 min
- 🗄️ Level 3 (DAAB → EF Core): 90-120 min (boss fight!)
- 🔌 Level 4 (Unity): 30-45 min
- 🌐 Level 5 (WCF): 45-60 min
- ☁️ Level 6 (Auth + Deploy): 60-90 min

💡 **Pro tip:** Take breaks between levels! This is a marathon, not a sprint! 🏃‍♂️

---

## 📚 RESOURCES & POWER-UPS

### 🎮 Official Documentation
- 📘 [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging)
- 📘 [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- 📘 [ASP.NET Core Dependency Injection](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- 📘 [Serilog Documentation](https://serilog.net/)
- 📘 [FluentValidation](https://docs.fluentvalidation.net/)
- 📘 [Polly Resilience](https://www.pollydocs.org/)
- 📘 [Azure App Service](https://learn.microsoft.com/en-us/azure/app-service/)

### 💎 Migration Guides
- 🔗 [Migrating from Enterprise Library](https://learn.microsoft.com/en-us/dotnet/framework/migration-guide/)
- 🔗 [.NET Framework to .NET Core Migration](https://learn.microsoft.com/en-us/dotnet/core/porting/)
- 🔗 [WCF to gRPC/HTTP Migration](https://learn.microsoft.com/en-us/dotnet/architecture/grpc-for-wcf-developers/)

### 🎯 EntLib Specific Resources
- 📖 [Enterprise Library Documentation (Archive)](https://learn.microsoft.com/en-us/previous-versions/msp-n-p/ff632023(v=pandp.10))
- 📖 [EntLib to Modern .NET Mapping Table](APPMODLAB.md) *(See full lab doc)*

### ⚡ Community
- 💬 [.NET Community Discord](https://discord.gg/dotnet)
- 💬 [Stack Overflow - enterprise-library tag](https://stackoverflow.com/questions/tagged/enterprise-library)
- 💬 [GitHub Discussions](https://github.com/EmeaAppGbb/appmodlab-dotnet-framework-entlib-to-dotnet9-core-di/discussions)

---

## 🎨 CREDITS & ACKNOWLEDGMENTS

### 👾 Game Design
- **Lab Architect:** GBB App Innovation Team
- **Demo App:** Alpine Financial Services Expense Manager
- **Final Boss:** Microsoft Enterprise Library (2008-2015, RIP 💀)

### 🌟 Special Thanks
- Enterprise Library team for the lessons learned (and the pain endured)
- .NET Core team for saving us all from XML configuration hell
- GitHub Copilot for being the ultimate co-op player 🤖

---

<div align="center">

## 🎮 READY PLAYER ONE?

**Press START to begin your journey!** 🕹️

```
┌────────────────────────────────────────────────┐
│                                                │
│  > CONTINUE                                    │
│    NEW GAME                                    │
│    OPTIONS                                     │
│    EXIT                                        │
│                                                │
└────────────────────────────────────────────────┘
```

### 📫 Need Help?

Open an issue or discussion in this repo!  
**Remember:** Even the best players consult the strategy guide! 📖

---

**Made with 💜 by the GBB Team**  
*Destroying legacy code, one boss at a time* ⚔️

```
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓
▓                                              ▓
▓  🎯 MISSION: MODERNIZE ALL THE THINGS! 🚀   ▓
▓                                              ▓
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓
```

</div>
