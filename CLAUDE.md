# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Split the Bill is a full-stack web application for tracking and splitting group expenses. It uses:
- **Backend**: .NET 9 API with Clean Architecture pattern
- **Frontend**: Nuxt 3 with Vue 3 and PrimeVue UI
- **Database**: PostgreSQL with Entity Framework Core
- **Orchestration**: .NET Aspire for local development
- **Authentication**: OpenID Connect (OIDC)

## Common Development Commands

### Quick Start
```bash
# Start the entire application stack (recommended)
dotnet run --project src/3-hosts/AppHost/AppHost.csproj
# This starts Aspire Dashboard, API, Database, Migrations, and Frontend
```

### Frontend Commands
```bash
cd frontend
npm install              # Install dependencies
npm run dev              # Development server (if not using Aspire)
npm run build            # Production build
npm run lint:check       # Check linting
npm run lint:fix         # Fix linting issues
npm run format:check     # Check formatting
npm run format:fix       # Fix formatting
npm run test:ts          # TypeScript type checking
```

### Backend Commands
```bash
# Building
dotnet build             # Build entire solution
dotnet build src/3-hosts/Api/Api.csproj  # Build specific project

# Testing
dotnet test              # Run all tests
dotnet test tests/Application.UnitTests/Application.UnitTests.csproj  # Unit tests only
dotnet test tests/Application.IntegrationTests/Application.IntegrationTests.csproj  # Integration tests (requires Docker)

# Code Formatting
dotnet csharpier .       # Format all C# files
dotnet csharpier . --check  # Check formatting without changes

# Database Migrations
dotnet tool restore      # Restore EF Core tools
dotnet ef migrations add <Name> --project src/2-infrastructure/Persistence/Persistence.csproj --startup-project src/3-hosts/DatabaseMigrations/DatabaseMigrations.csproj
```

## High-Level Architecture

### Backend Architecture (Clean Architecture)
```
api/
├── src/
│   ├── 1-core/          # Core business logic (no external dependencies)
│   │   ├── Domain/      # Entities, ValueObjects, Domain Events
│   │   └── Application/ # Use Cases, DTOs, Application Services
│   ├── 2-infrastructure/
│   │   ├── Infrastructure/  # External services, Auth
│   │   └── Persistence/     # EF Core, Repositories, Migrations
│   └── 3-hosts/
│       ├── Api/             # ASP.NET Core Web API
│       ├── AppHost/         # .NET Aspire orchestrator
│       └── DatabaseMigrations/  # Migration worker service
```

Key patterns:
- **CQRS-like**: Separate Commands and Queries with Mediator (not MediatR)
- **Domain-Driven Design**: Rich domain models
- **Dependency Injection**: Clean dependency management

### Frontend Architecture
```
frontend/
├── app/
│   ├── components/      # Vue components organized by feature
│   ├── pages/          # File-based routing
│   ├── composables/    # Shared composition functions
│   └── assets/         # Styles and static assets
├── server/             # Nitro server (BFF pattern)
│   ├── api/           # API proxy endpoints
│   └── utils/         # Server utilities
└── shared/            # Shared types and utilities
```

Key patterns:
- **BFF (Backend for Frontend)**: Server-side API proxy for security
- **Server-Side Authentication**: Tokens never exposed to browser
- **Component-Based UI**: PrimeVue components with Aura theme
- **Composables**: Reusable logic following Vue 3 Composition API

### Authentication Flow
1. Frontend redirects to OIDC IDP for login
2. IDP redirects back with authorization code
3. Nitro server exchanges code for tokens (stored server-side)
4. Frontend receives session cookie (no tokens in browser)
5. API calls proxied through Nitro with server-attached Bearer token

### Data Flow
1. **Frontend**: User interaction → Vue component → Composable → Nitro API
2. **BFF**: Nitro receives request → Attaches auth token → Forwards to .NET API
3. **API**: Controller → Mediator → Handler → Database
4. **Response**: Flows back through the same chain

## Development Guidelines

### When Making Changes
1. **Frontend**: Check existing components and composables for patterns
2. **Backend**: Follow Clean Architecture boundaries - Domain shouldn't reference Infrastructure
3. **API Changes**: Update OpenAPI spec and regenerate frontend types
4. **Database Changes**: Create EF Core migrations using the commands above
5. **Authentication**: Never expose tokens to the browser; use the BFF pattern

### Testing Requirements
- Backend: Unit tests for domain logic, integration tests for API endpoints
- Frontend: TypeScript type checking must pass
- Both: Code must be properly formatted before committing

### Environment Configuration
- Frontend: Uses `.env` files and Nuxt runtime config (set from Aspire for local development)
- Backend: Uses `appsettings.json` and environment variables
- Aspire: Automatically configures services and passes environment variables

### Key Technologies to Know
- **Backend**: C# 13, .NET 9, EF Core, Mediator, FluentValidation
- **Frontend**: TypeScript, Vue 3, Nuxt 3, PrimeVue, Valibot
- **Testing**: xUnit, Testcontainers, FluentAssertions
- **Infrastructure**: Docker, PostgreSQL, Redis (for sessions)