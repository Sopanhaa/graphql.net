# Validation — 30 October 2026

- SDK: .NET 10.0.100; both projects target net10.0.
- NuGet restore completed with pinned stable package versions.
- Build passed: 0 warnings, 0 errors.
- This hosted environment required a single MSBuild worker. Reproducible verification command:
  `dotnet build --no-restore -m:1 -p:UseSharedCompilation=false`
- Both projects started successfully with `dotnet run --no-build --project <project> --launch-profile https`.
- Development certificate trust was configured with `dotnet dev-certs https --trust`. The test HTTP client used the exported public certificate as its explicit CA; application certificate validation was not disabled.
- Live GraphQL HTTPS requests called the gRPC HTTPS service successfully:
  - Eight products and all six fields.
  - Exactly id/name/price when those fields were selected.
  - Existing ID 1 returned Wireless Mouse.
  - Missing ID 999 returned null without errors.
  - Case-insensitive Books filter returned three products.
  - Unknown category returned an empty list.
- Stopping the gRPC service produced `PRODUCT_SERVICE_UNAVAILABLE`, without a client-facing stack trace.
- The embedded development IDE endpoint served HTML successfully (following its redirect).
- GraphQL requests were verified with the included Python HTTP smoke test and curl. No manual clicks in the browser IDE were performed.
- Reflection is registered and mapped in Development; grpcurl was not installed, so a direct reflection inspection was not performed.

The README includes expected responses and steps to repeat the demo in Nitro/Banana Cake Pop.
