namespace CleanArchitecture.Web.AcceptanceTests.Pages;

public abstract class BasePage(IPage page)
{
    protected static string BaseUrl =>
#if (UseBlazor)
        AspireSetup.App.GetEndpoint(Services.WebApi).ToString().TrimEnd('/');
#else
        AspireSetup.App.GetEndpoint(Services.WebFrontend).ToString().TrimEnd('/');
#endif

    public abstract string PagePath { get; }

    protected IPage Page { get; } = page;

    public Task GotoAsync() => Page.GotoAsync(PagePath);
}
