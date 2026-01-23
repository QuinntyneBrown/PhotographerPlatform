# CDN Helper Guide

## URL Building
Use `CdnUrlBuilder` to build transformed image URLs and responsive variants.

## Cache Headers
Use `CacheControlPolicy.ToHeaderValue()` to standardize cache-control headers.

## Signed URLs
Use `SignedUrlGenerator` to append expiration and signature parameters.

## Image Processing
Implement `IImageProcessor` to plug in resizing/format conversion logic.
