# Clean Architecture Vue Client

This frontend uses Vue 3, TypeScript, and Vite. The generated OpenAPI client is written to `src/web-api-client.ts` by NSwag.

## Available scripts

- `npm start` starts the Vite development server with hot module replacement and the ASP.NET Core API proxy.
- `npm run typecheck` checks TypeScript and Vue single-file components.
- `npm run build` creates an optimized production build in `build/`.
- `npm run preview` serves the production build locally.

Run the solution through the Aspire AppHost for API document generation and the configured backend proxy:

```bash
dotnet run --project ../AppHost
```

Vite environment variables exposed to client code must use the `VITE_` prefix.
