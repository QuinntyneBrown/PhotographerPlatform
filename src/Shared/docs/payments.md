# Payment Provider Guide

## Onboarding a New Provider
1. Implement `Shared.Payments.IPaymentProvider`.
2. Bind provider configuration via `PaymentProviderConfiguration`.
3. Use `IdempotencyKeyHelper` for safe retries.
4. Verify provider webhooks with `PaymentWebhookSignatureVerifier`.

## Secret Loading Patterns
Use `SecretReference` + `SecretLoader` to resolve API keys or webhook secrets from:
- Direct values (development only)
- Environment variables
- File-based secrets (e.g., mounted secrets)

## Error Mapping
Map provider-specific errors into `PaymentError` and populate `PaymentResult`.
