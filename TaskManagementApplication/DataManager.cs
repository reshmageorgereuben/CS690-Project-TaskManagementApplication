namespace TaskManagementApplication;

public class DataManager {

    FileSaver filesaver;


    public List<Task> TaskList {get; set;}
    public List<TimeBlock> Scheduler {get; set;}
    
    public DataManager(){       
        filesaver = new FileSaver("tasks-list.txt");
        TaskList = getFileContents().Length > 0 ? getListOfTasks() : new List<Task>(); 
        Scheduler = TaskList.Count > 0 ? fillScheduler() : new List<TimeBlock>();   
    }

    public void addtasksToList(Task task){
        int getLastId = filesaver.getLastTaskID();
        
        task.TaskId = getLastId == 0 ? 1 : getLastId+1;
        

        this.filesaver.AppendData(task);
        TaskList.Add(task);
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
            DateTime? startTime = DateTime.TryParse(eachItem.ElementAtOrDefault(6), out var parsedST) ? parsedST  : null;
            DateTime? endTime = DateTime.TryParse(eachItem.ElementAtOrDefault(7), out var parsedET) ? parsedET  : null;
            string status = eachItem.Length > 8 ? eachItem[8]: "";
            String setReminder = eachItem.Length > 9? eachItem[9] : "";
            TaskListItems.Add(new Task(taskId,taskName,description,deadline,priority,category,startTime,endTime,status,setReminder));
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
                    string priority = eachItem.Length > 4 ? eachItem[4]: "";
                    string category = eachItem.Length > 5 ? eachItem[5]: "";
                    DateTime? startTime = eachItem.Length > 6 ? DateTime.Parse(eachItem[6]): null;
                    DateTime? endTime = eachItem.Length > 7 ? DateTime.Parse(eachItem[7]): null;
                    string status = eachItem.Length > 8 ? eachItem[8]: "";
                    string setReminder = eachItem.Length > 9? eachItem[9] :"";
            TaskList.Add(new Task(int.Parse(eachItem[0]),taskName,description,deadline,priority,category,startTime,endTime,status,setReminder));

        }
        }
        return TaskList;
        
    }
   
    public void SaveAllData(){
        filesaver.SaveAllData(TaskList);
    }

     public List<TimeBlock> fillScheduler(){    
        var SchedulerItems =  new List<TimeBlock>();
         foreach(var listItem in TaskList) {            
            SchedulerItems.Add(new TimeBlock(listItem.TaskId,listItem.TaskName,listItem.Deadline,listItem.StartTime,listItem.EndTime));
        }

        return SchedulerItems;
        
    }

   


}