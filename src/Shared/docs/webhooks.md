# Webhooks Guide

## Adding a New Webhook Event Type
1. Add a new value to `Shared.Webhooks.WebhookEventType`.
2. Ensure any producers publish a `WebhookEvent` with the new type.
3. Consumers should use `WebhookEventEnvelope` for external delivery.
4. If the event requires extra metadata, include it in `WebhookEvent.Metadata`.

## Dispatching Webhooks
Use `DefaultWebhookDispatcher` to deliver events with retries and backoff. Configure
`WebhookDispatchPolicy` for retry limits.

## Signature Verification
Use `WebhookSignature.Create` to sign payloads and `WebhookSignature.Verify` to verify.
