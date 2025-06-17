# ServiceDefaults Project

This project provides common .NET Aspire services and cross-cutting concerns for the Split-the-Bill application. It should be referenced by each service project in the solution.

## Current Features

- **OpenTelemetry**: Logging, metrics, and distributed tracing
- **Health Checks**: Live and ready probes with proper endpoint mapping
- **Service Discovery**: Automatic service registration and discovery
- **Resilience**: HTTP client resilience patterns with retry policies

## Usage

```csharp
// Basic usage
builder.AddServiceDefaults();

// Map health check endpoints (development only)
app.MapDefaultEndpoints();
```

## Health Check Strategy

- **Live** (`/health/live`): Minimal check indicating service is responding
- **Ready** (`/health/ready`): Complete health including external dependencies (DB, MassTransit, etc.)

Register external dependency health checks with the `ready` tag:
```csharp
builder.Services.AddHealthChecks()
    .AddDbContext<AppDbContext>(["ready"])
    .AddRabbitMQ("connection-string", ["ready"]);
```

## Planned Enhancements

### Essential Cross-Cutting Concerns

#### **Structured Logging**
- [ ] Serilog configuration with structured logging
- [ ] Log enrichment with correlation IDs, user context
- [ ] Environment-specific log levels and sinks
- [ ] Request/response logging middleware

#### **Security & Authentication**
- [ ] JWT authentication setup
- [ ] Authorization policy configuration
- [ ] Security headers middleware
- [ ] CORS policies configuration
- [ ] API key authentication for internal services

#### **API Standards**
- [ ] API versioning setup
- [ ] Swagger/OpenAPI configuration
- [ ] Content negotiation setup
- [ ] Global exception handling with problem details
- [ ] Model validation with FluentValidation

### Aspire-Specific Features

#### **Data Access**
- [ ] Database connection resilience patterns
- [ ] Entity Framework Core configuration
- [ ] Connection string management
- [ ] Database health checks

#### **Messaging**
- [ ] MassTransit/RabbitMQ defaults
- [ ] Message broker resilience
- [ ] Dead letter queue configuration
- [ ] Message audit logging

#### **Caching**
- [ ] Redis configuration and connection
- [ ] Distributed cache setup
- [ ] Cache invalidation patterns
- [ ] Cache health checks

#### **Configuration Management**
- [ ] Secrets management helpers
- [ ] Environment-specific configuration
- [ ] Configuration validation
- [ ] Feature flags integration

### Operational Concerns

#### **Performance & Monitoring**
- [ ] Custom application metrics
- [ ] Performance counters
- [ ] Request timeout configuration
- [ ] Rate limiting defaults

#### **Reliability**
- [ ] Circuit breaker patterns
- [ ] Bulkhead isolation
- [ ] Timeout policies
- [ ] Fallback mechanisms

## Future Architecture

Consider organizing into focused extension methods for better modularity:

```csharp
builder.AddServiceDefaults()
       .AddStructuredLogging()
       .AddSecurity()
       .AddApiDefaults()
       .AddDataAccess()
       .AddMessaging()
       .AddCaching();
```

## Implementation Priority

1. **High Priority**: Structured logging, security, global exception handling
2. **Medium Priority**: API versioning, Swagger, database resilience
3. **Low Priority**: Advanced monitoring, rate limiting, feature flags

## Notes

- Health check endpoints are currently development-only for security
- OpenTelemetry excludes health check endpoints from tracing to reduce noise
- Service discovery and resilience are enabled by default for HTTP clients