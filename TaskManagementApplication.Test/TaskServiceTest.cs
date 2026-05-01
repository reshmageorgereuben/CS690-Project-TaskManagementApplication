using TaskManagementApplication;

public class TaskServiceTests
{
    DataManager dm;
    TaskService taskService;

    public TaskServiceTests()
    {
        dm = new DataManager();        
        dm.TaskList = new List<TaskItem>();  
        taskService = new TaskService(dm);
    }

    
    [Fact]
    public void AddTask_Should_Add_TaskToList()
    {
        TaskItem task = new TaskItem(1,"Study","Read C#",DateTime.Parse("2026-05-01"),"High","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress");
        taskService.AddTask(task);
        Assert.Single(dm.TaskList);
        Assert.Equal("Study", dm.TaskList[0].TaskName);
    }

 
    [Fact]
    public void GetAllTasks_Should_Return_AllTasks()
    {
        dm.TaskList.Add(new TaskItem(1,"Study","Read C#",DateTime.Parse("2026-05-01"),"High","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress"));
        dm.TaskList.Add(new TaskItem(2,"Review Emails","Read C#",DateTime.Parse("2026-05-01"),"High","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress"));
        var result = taskService.GetAllTasks();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetTaskById_Should_Return_CorrectTask()
    {
        var task = new TaskItem(10,"Study","Read C#",DateTime.Parse("2026-05-01"),"High","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress");
        dm.TaskList.Add(task);
        var result = taskService.GetTaskById(10);
        Assert.NotNull(result);
        Assert.Equal(10, result.TaskId);
    }

    [Fact]
    public void GetTaskById_Should_Return_Null_WhenNotFound()
    {
        var result = taskService.GetTaskById(999);
        Assert.Null(result);
    }

   
    [Fact]
    public void DeleteTask_Should_Remove_Task()
    {
        dm.TaskList.Add(new TaskItem(1,"Study","Read C#",DateTime.Parse("2026-05-01"),"High","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress"));
        taskService.DeleteTask(1);
        Assert.Empty(dm.TaskList);
    }

   
    [Fact]
    public void UpdateTask_Should_Update_AllFields() {     
        var task = new TaskItem(1,"old","Read C#",DateTime.Parse("2026-05-01"),"High","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"Done");
        dm.TaskList.Add(task);
        var updated = new TaskItem(1,"New","Read C#",DateTime.Parse("2026-05-01"),"High","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress");
        taskService.UpdateTask(updated);
        task  = taskService.GetTaskById(1);
        Assert.Equal("New", task.TaskName);
        Assert.Equal("High", task.Priority);
        Assert.Equal("In Progress", task.Status);        
    }

   
    [Fact]
    public void UpdateStatus_Should_Change_Status()
    {        
          var task = new TaskItem(1,"Study","Read C#",DateTime.Parse("2026-05-01"),"High","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress");
        dm.TaskList.Add(task);
        taskService.UpdateStatus(1, "Completed");
        Assert.Equal("Completed", task.Status);
    }

    [Fact]
    public void UpdatePriority_Should_Change_Priority()
    {
        var task = new TaskItem(1,"Study","Read C#",DateTime.Parse("2026-05-01"),"Medium","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress");
        dm.TaskList.Add(task);
        taskService.UpdatePriority(1, "High");
        Assert.Equal("High", task.Priority);
    }

   
    [Fact]
    public void UpdateCategory_Should_Change_Category()
    {
        var task = new TaskItem(1,"Study","Read C#",DateTime.Parse("2026-05-01"),"High","Work",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress");
        dm.TaskList.Add(task);
        taskService.UpdateCategory(1, "Personal");
        Assert.Equal("Personal", task.Category);
    }

   
}