namespace TaskManagementApplication;

public class Task{

   public int TaskId;
   public string TaskName;
   public string Description;
   public DateTime Deadline;
   public string Priority;
   public string Category;
   

//    public Task( string taskName, string description, DateTime deadline, string priority = ""){
                    
//             //this.TaskId = taskID;        
//             this.TaskName = taskName;
//             this.Description = description;
//             this.Deadline = deadline;
//             this.Priority = priority;
//     }  

    public Task(int taskID, string taskName, string description, DateTime deadline, string priority = "", string category = ""){
                    
            this.TaskId = taskID;        
            this.TaskName = taskName;
            this.Description = description;
            this.Deadline = deadline;
            this.Priority = priority;
            this.Category = category;

    }  

     public override string ToString()
    {
        return $"{TaskName} - {Description} - {Priority} - {Category}";
    }

   
}