# Clean Architecture Blazor Client

This project is a Blazor WebAssembly frontend hosted by the ASP.NET Core Web project. The Web host serves the client and API from the same origin, so browser requests use the existing cookie authentication flow.

Run the solution through the Aspire AppHost:

```bash
dotnet run --project ../AppHost
```

The client includes Home, Counter, Weather, Tasks, Login, and Register pages. Weather and task data are loaded from the Web API.
