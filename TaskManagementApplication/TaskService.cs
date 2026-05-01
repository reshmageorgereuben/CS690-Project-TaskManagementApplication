namespace TaskManagementApplication;

public class TaskService
{
    DataManager dataManager;

      public TaskService(DataManager dm)
    {
        dataManager = dm;
    }

    public List<TaskItem> GetAllTasks()
    {
        return dataManager.TaskList;
    }

    public void AddTask(TaskItem task)
    {
        dataManager.addtasksToList(task);
        dataManager.SaveAllData();
    }

    public void UpdateTask(TaskItem updated)
    {
        var task = dataManager.TaskList.FirstOrDefault(t => t.TaskId == updated.TaskId);

        if (task == null) return;

        task.TaskName = updated.TaskName;
        task.Description = updated.Description;
        task.Deadline = updated.Deadline;
        task.Priority = updated.Priority;
        task.Category = updated.Category;
        task.Status = updated.Status;
        task.StartTime = updated.StartTime;
        task.EndTime = updated.EndTime;

        dataManager.SaveAllData();
    }

    public bool DeleteTask(int id)
    {
        var removed = dataManager.TaskList.RemoveAll(t => t.TaskId == id) > 0;

        if (removed)
            dataManager.SaveAllData();

        return removed;
    }

    public TaskItem? GetTaskById(int id)
    {
        return dataManager.TaskList.FirstOrDefault(t => t.TaskId == id);
    }

     public void UpdateStatus(int id, string status)
    {
        var task = GetTaskById(id);
        if (task == null) return;

        task.Status = status;
        dataManager.SaveAllData();
    }

    public void UpdatePriority(int id, string priority)
    {
        var task = GetTaskById(id);
        if (task == null) return;

        task.Priority = priority;
        dataManager.SaveAllData();
    }

    public void UpdateCategory(int id, string category)
    {
        var task = GetTaskById(id);
        if (task == null) return;

        task.Category = category;
        dataManager.SaveAllData();
    }
}