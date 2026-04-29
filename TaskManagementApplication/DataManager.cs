namespace TaskManagementApplication;

public class DataManager {

    FileSaver filesaver;


    public List<Task> TaskList {get; set;}
    
    public DataManager(){
       
        // TaskList = new List<Task>();
        filesaver = new FileSaver("tasks-list.txt");
        TaskList = getFileContents().Length > 0 ? getListOfTasks() : new List<Task>();

        


    }

    public void addtasksToList(Task task){
        int getLastId = filesaver.getLastTaskID();
        
        task.TaskId = getLastId == 0 ? 1 : getLastId+1;
        
        this.TaskList.Add(task);
        this.filesaver.AppendData(task);
    }


    public string[] getFileContents(){
         return File.ReadAllLines("tasks-list.txt");
         
    }

    public List<Task> getListOfTasks(){
        var TaskListItems =  new List<Task>();
        var fileContents = getFileContents();
        foreach(var listItem in fileContents) {
            var eachItem = listItem.Split("|");           
            int taskId = int.Parse(eachItem[0]);
            string taskName = eachItem[1];
            string description = eachItem[2];
            DateTime deadline = DateTime.Parse(eachItem[3]);
            string priority = eachItem.Length > 4 ? eachItem[4]: "";
            string category = eachItem.Length > 5 ? eachItem[5]: "";
            TaskListItems.Add(new Task(taskId,taskName,description,deadline,priority,category));

        }

        return TaskListItems;
    }

     public List<Task> updateTasks(Task task){
        TaskList =  new List<Task>();
        var fileContents = getFileContents();

        foreach(var listItem in fileContents) {
            var eachItem = listItem.Split("|");
            if(int.Parse(eachItem[0]) == task.TaskId ) {
                    string taskName = eachItem[1];
                    string description = eachItem[2];
                    DateTime deadline = DateTime.Parse(eachItem[3]);
            TaskList.Add(new Task(task.TaskId,taskName,description,deadline));

        }
        }
        return TaskList;
        
    }
   
    public void SaveAllData(){
        filesaver.SaveAllData(TaskList);
    }



}