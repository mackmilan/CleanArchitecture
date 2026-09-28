namespace ClientApp.Models;

public sealed class TodosVmDto
{
    public IReadOnlyCollection<ColourDto> Colours { get; init; } = [];

    public IReadOnlyCollection<TodoListDto> Lists { get; init; } = [];
}

public sealed class ColourDto
{
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;
}

public sealed class TodoListDto
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public IReadOnlyCollection<TodoItemDto> Items { get; init; } = [];
}

public sealed class TodoItemDto
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }
}

public sealed class WeatherForecastDto
{
    public DateTime Date { get; init; }

    public int TemperatureC { get; init; }

    public int TemperatureF { get; init; }

    public string Summary { get; init; } = string.Empty;
}
