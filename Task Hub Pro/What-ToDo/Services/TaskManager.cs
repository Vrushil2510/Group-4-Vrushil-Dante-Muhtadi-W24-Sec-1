using Spectre.Console;

namespace What_ToDo.Services;

public interface ITaskManager
{
    void AddTask();
    void EditTask();
    void MarkTaskAsCompleted();
    void DeleteTask();
    void LoadTasks();
    void DisplayTasks();
}

public class TaskManager : ITaskManager
{
    private List<Entities.Task> tasks = new();

    public void AddTask()
    {
        while (true)
        {
            var taskName = AnsiConsole.Ask<string>("Enter task name (Enter 0 to return): ");
            if (taskName == "0")
            {
                return;
            }
            tasks.Add(new Entities.Task(taskName));
            UpdateTasks();
            break;
        }
    }

    public void EditTask()
    {
        if (tasks.Count == 0)
        {
            return;
        }

        var prompt = new SelectionPrompt<string>()
            .Title("Select a task to edit:");

        foreach (var task in tasks)
        {
            prompt.AddChoice(task.Name);
        }

        var selectedTask = AnsiConsole.Prompt(prompt);

        var newTaskName = AnsiConsole.Ask<string>($"Enter a new name for the task '{selectedTask}' (Enter 0 to return): ");

        if (newTaskName == "0")
        {
            return;
        }
        int index = tasks.FindIndex(task => task.Name == selectedTask);
        tasks[index].Name = newTaskName;
        UpdateTasks();
    }

    public void MarkTaskAsCompleted()
    {
        if (tasks.Count == 0)
        {
            return;
        }

        var selectedTask = AnsiConsole.Prompt(
            new SelectionPrompt<Entities.Task>()
                .Title("Select a task to mark as completed:")
                .AddChoices(tasks)
                .UseConverter(task => task.Name)
        );

        selectedTask.IsCompleted = true;
        UpdateTasks();
    }

    public void DeleteTask()
    {
        if (tasks.Count == 0)
        {
            return;
        }

        var selectedTask = AnsiConsole.Prompt(
            new SelectionPrompt<Entities.Task>()
                .Title("Select a task to delete:")
                .AddChoices(tasks)
                .UseConverter(task => task.Name)
        );

        tasks.Remove(selectedTask);
        UpdateTasks();
    }

    public void UpdateTasks()
    {
        File.WriteAllLines("tasks.txt", tasks.Select(task => $"{task.Name}|{task.IsCompleted}"));
    }

    public void LoadTasks()
    {
        if (File.Exists("tasks.txt"))
        {
            tasks = File.ReadAllLines("tasks.txt")
                .Select(line => line.Split('|'))
                .Select(parts => new Entities.Task(parts[0]) { IsCompleted = bool.Parse(parts[1]) })
                .ToList();
        }
    }

    public void DisplayTasks()
    {
        AnsiConsole.WriteLine("Task Hub Pro:");

        var table = new Table();
        table.AddColumn("Task");
        table.AddColumn("Status");

        foreach (var task in tasks)
        {
            table.AddRow(task.Name, task.IsCompleted ? "Completed" : "Not completed");
        }
        AnsiConsole.Write(table);
    }
}