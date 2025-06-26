# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with the .NET API codebase.

## API-Specific Development Commands

### Running the API
```bash
# Using Aspire (recommended - includes all dependencies)
dotnet run --project src/3-hosts/AppHost/AppHost.csproj

# Running API standalone
dotnet run --project src/3-hosts/Api/Api.csproj
# API runs on https://localhost:5223 (HTTPS) and http://localhost:5222 (HTTP)

# Running with hot reload
dotnet watch run --project src/3-hosts/Api/Api.csproj
```

### Testing Commands
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/Application.UnitTests/Application.UnitTests.csproj
dotnet test tests/Domain.UnitTests/Domain.UnitTests.csproj
dotnet test tests/Application.IntegrationTests/Application.IntegrationTests.csproj

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test
dotnet test --filter "FullyQualifiedName~CreateGroup"
```

### Database Commands
```bash
# Restore EF Core tools
dotnet tool restore

# Add migration
dotnet ef migrations add <Name> \
  --project src/2-infrastructure/Persistence/Persistence.csproj \
  --startup-project src/3-hosts/DatabaseMigrations/DatabaseMigrations.csproj

# Remove last migration
dotnet ef migrations remove \
  --project src/2-infrastructure/Persistence/Persistence.csproj \
  --startup-project src/3-hosts/DatabaseMigrations/DatabaseMigrations.csproj

# List migrations
dotnet ef migrations list \
  --project src/2-infrastructure/Persistence/Persistence.csproj \
  --startup-project src/3-hosts/DatabaseMigrations/DatabaseMigrations.csproj
```

### Code Quality
```bash
# Format code with CSharpier
dotnet csharpier .

# Check formatting
dotnet csharpier . --check

# Build with warnings as errors (default)
dotnet build
```

## Architecture & Patterns

### Clean Architecture Layers
1. **Domain**: Pure business logic, no dependencies
   - Entities implement `IAggregateRoot<TId>`
   - Rich domain models with business logic
   - Guard clauses in setters
   - No infrastructure concerns

2. **Application**: Use cases and application logic
   - CQRS pattern with Mediator
   - Vertical slice architecture
   - ErrorOr pattern for all operations
   - FluentValidation for input validation

3. **Infrastructure**: External concerns
   - Entity Framework Core implementation (separate Persistence project)
   - Authentication services
   - External API clients

4. **API**: HTTP concerns only
   - Minimal APIs with endpoint groups
   - Problem Details for error responses
   - OpenAPI documentation

### Vertical Slice Pattern
Each feature is a single file containing:
```csharp
public static class CreateGroup
{
    public sealed record Request(string Name, string? Description) : ICommand<ErrorOr<Response>>;
    public sealed record Response(Guid Id);
    
    internal sealed class Validator : BaseValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name).ValidString(required: true, maxLength: 512);
        }
    }
    
    internal sealed class Handler(IApplicationDbContext context) 
        : ICommandHandler<Request, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Request request, CancellationToken ct)
        {
            // Implementation
        }
    }
}
```

### Error Handling
Always use ErrorOr pattern:
```csharp
// Don't throw exceptions
throw new NotFoundException();  // ❌

// Return Error objects
return Error.NotFound();        // ✅

// Common error types
Error.Validation()
Error.NotFound()
Error.Conflict()
Error.Unauthorized()
```

### Validation Patterns
```csharp
// All validators inherit BaseValidator
internal sealed class Validator : BaseValidator<Request>
{
    public Validator()
    {
        // Common validation extensions
        RuleFor(x => x.Name).ValidString(required: true, maxLength: 50);
        RuleFor(x => x.Amount).PositiveDecimal();
        RuleFor(x => x.Email).NotNullOrEmptyWithErrorCode()
            .EmailAddress().WithErrorCode(ErrorCodes.Invalid);
    }
}
```

### Testing Patterns
```csharp
// Unit tests use TUnit
[Test]
public async Task CreateGroup_WithValidData_CreatesGroup()
{
    // Arrange
    var command = new CreateGroup.Request("Test Group", null);
    
    // Act
    var result = await App.SendAsync(command);
    
    // Assert (using Shouldly)
    result.IsError.ShouldBeFalse();
    result.Value.Id.ShouldNotBe(Guid.Empty);
}

// Test data builders
var group = new GroupBuilder()
    .WithName("Test")
    .WithDefaultMember()
    .Build();
```

## Code Conventions

### Naming & Style
- File-scoped namespaces
- Records for DTOs and requests
- `internal sealed` for implementations
- `public` only for contracts
- Static classes for feature grouping

### EF Core Patterns
```csharp
// Always include required navigation properties
var group = await context.Groups
    .Include(g => g.Members)
    .Include(g => g.Expenses)
    .FirstOrDefaultAsync(g => g.Id == groupId, ct);

// Save changes pattern
await context.SaveChangesAsync(CancellationToken.None); // Intentional None
```

### User Context
```csharp
// Get current user's member
var member = await context.Members
    .FirstOrDefaultAsync(m => m.UserId == request.UserId, ct);

if (member is null)
    return Error.Unauthorized();
```

## Common Pitfalls

1. **Validators must be `internal sealed`** - DI scanning includes internal types
2. **Include navigation properties** - EF Core doesn't lazy load
3. **Check user's Member record** - Not all users have member records
4. **Use ErrorOr consistently** - Never throw business exceptions
5. **CascadeMode.Stop** - Prevents multiple errors on same field
6. **String length defaults** - 512 chars unless specified

## Adding New Features

1. Create new file in appropriate module folder
2. Define Request/Response records
3. Implement Validator with BaseValidator
4. Implement Handler returning ErrorOr<Response>
5. Add endpoint mapping in Api
6. Write unit and integration tests
7. Update OpenAPI spec if needed

## Key Dependencies
- **Mediator**: CQRS command/query handling
- **ErrorOr**: Functional error handling
- **FluentValidation**: Input validation
- **TUnit**: Unit testing framework
- **Shouldly**: Assertion library
- **EF Core**: ORM with PostgreSQL