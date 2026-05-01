namespace TaskManagementApplication;

public class DataManager {

    FileSaver filesaver;


    public List<TaskItem> TaskList {get; set;}
    public User User { get; set; }
  
    
    public DataManager(){       
        filesaver = new FileSaver("tasks-list.txt");
        TaskList = getFileContents().Length > 0 ? getListOfTasks() : new List<TaskItem>(); 
        User = LoadUser() ?? new User ("Default User");
         
        TaskList = getFileContents().Length > 0 ? getListOfTasks() : new List<TaskItem>(); 
      
    }

    public void addtasksToList(TaskItem task){
        int getLastId = filesaver.getLastTaskID();
        
        task.TaskId = getLastId == 0 ? 1 : getLastId+1;
        

        this.filesaver.AppendData(task);
        TaskList.Add(task);
    }


    public string[] getFileContents(){
         return File.ReadAllLines(this.filesaver.fileName);
         
    }

    public List<TaskItem> getListOfTasks(){
        var TaskListItems =  new List<TaskItem>();
        var fileContents = getFileContents();
        foreach(var listItem in fileContents) {
            var eachItem = listItem.Split("|");           
            int taskId = int.Parse(eachItem[0]);
            string taskName = eachItem[1];
            string description = eachItem[2];
            DateTime deadline = DateTime.Parse(eachItem[3]);
            string priority = eachItem.Length > 4 ? eachItem[4]: "";
            string category = eachItem.Length > 5 ? eachItem[5]: "";
            DateTime? startTime = DateTime.TryParse(eachItem.ElementAtOrDefault(6), out var parsedST) ? parsedST  : null;
            DateTime? endTime = DateTime.TryParse(eachItem.ElementAtOrDefault(7), out var parsedET) ? parsedET  : null;
            string status = eachItem.Length > 8 ? eachItem[8]: "";
          
            TaskListItems.Add(new TaskItem(taskId,taskName,description,deadline,priority,category,startTime,endTime,status));
        }

        return TaskListItems;
    }

  
    public void SaveAllData(){
        filesaver.SaveAllData(TaskList);
    }


    public User LoadUser()
    {
        if (!File.Exists("users.txt"))
            return new User("Default User");

        var name = File.ReadAllText("users.txt").Trim();

        return new User(name);
        
    }

    public void SaveUser(User user)
{
    File.WriteAllText("users.txt", user.username);
}
    
 

   


}