# Shared Adoption & Migration

## Identify Duplicated Logic
- Search services for custom webhook signing, retry/backoff, or cache-control helpers.
- Prefer `Shared.Webhooks`, `Shared.Http`, and `Shared.Cdn` utilities for consistency.

## Replace with Shared Utilities
- Swap per-service HMAC helpers for `Shared.Security.HmacSignature`.
- Replace custom retry logic with `Shared.Http.ResilientHttpClient` and `RetryBackoff`.
- Use `Shared.Cdn.CacheControlPolicy` and `CdnUrlBuilder` for CDN headers/URLs.

## Deprecation Notes
- When removing local helpers, add changelog notes and mark old types as obsolete.
- Provide migration guides in service README files.
