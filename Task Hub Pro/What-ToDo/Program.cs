using Microsoft.Extensions.DependencyInjection;
using What_ToDo.Services;

var services = new ServiceCollection();
services.AddSingleton<IApplication, Application>();
services.AddSingleton<ITaskManager, TaskManager>();

var serviceProvider = services.BuildServiceProvider();

var application = serviceProvider.GetService<IApplication>();
application.Run();