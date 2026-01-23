# Shared Library

## Purpose
Shared is a cross-cutting library intended for reusable primitives only. It should not depend on domain-specific concepts from individual services.

## Boundaries
- Provide utilities, contracts, and helpers used across services.
- Avoid direct dependencies on service-specific domain models.
- Keep implementations framework-agnostic where possible.

## Versioning Strategy
- Use project references during active development.
- When stabilized, publish an internal NuGet package per major version.
- Follow semantic versioning: breaking changes require a major bump.

## CI Checks
- Build `Shared.csproj`.
- Run `Shared.Tests`.
- Run static analysis (nullable, analyzers, formatting) as part of the solution build.

## Docs
- `docs/webhooks.md`
- `docs/payments.md`
- `docs/cdn.md`
- `docs/migration.md`
