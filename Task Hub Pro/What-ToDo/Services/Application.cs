using Spectre.Console;

namespace What_ToDo.Services;

public interface IApplication
{
    void Run();
}

public class Application : IApplication
{
    private readonly ITaskManager _taskManager;

    public Application(ITaskManager taskManager)
    {
        _taskManager = taskManager;
    }

    public void Run()
    {
        while (true)
        {
            AnsiConsole.Clear();

            _taskManager.LoadTasks();
            _taskManager.DisplayTasks();
            HandleSelection();
        }
    }

    public string DisplayMenu()
    {
        var rule = new Rule();
        rule.LeftJustified();
        AnsiConsole.Write(rule);

        var options = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("Add", "Edit", "Mark as completed", "Delete", "Exit")
        );

        return options;
    }

    public void HandleSelection()
    {
        var selection = DisplayMenu();
        switch (selection)
        {
            case "Add":
                _taskManager.AddTask();
                break;

            case "Edit":
                _taskManager.EditTask();
                break;

            case "Mark as completed":
                _taskManager.MarkTaskAsCompleted();
                break;

            case "Delete":
                _taskManager.DeleteTask();
                break;

            case "Exit":
                Environment.Exit(0);
                break;
        }
    }
}