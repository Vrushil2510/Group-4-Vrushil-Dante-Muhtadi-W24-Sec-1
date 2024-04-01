namespace What_ToDo.Entities;

public class Task
{
    public string Name { get; set; }
    public bool IsCompleted { get; set; }

    public Task(string name)
    {
        Name = name;
        IsCompleted = false;
    }
}