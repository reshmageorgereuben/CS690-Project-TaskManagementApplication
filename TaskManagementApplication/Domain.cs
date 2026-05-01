namespace TaskManagementApplication;

public class TaskItem{

   public int TaskId;
   public string TaskName { get; set; }
   public string Description { get; set; }
   public DateTime Deadline{ get; set; }
   public string Priority { get; set; }
   public string Category { get; set; }
   public DateTime? StartTime { get; set; }
   public DateTime? EndTime { get; set; }
   public string Status { get; set; }
  
   
   public TaskItem(int taskID, string taskName, string description, DateTime deadline, string priority, string category,
    DateTime? starttime, DateTime? endtime, string status){
                    
            this.TaskId = taskID;        
            this.TaskName = taskName;
            this.Description = description;
            this.Deadline = deadline;
            this.Priority = priority;
            this.Category = category;
            this.Status = status;
            this.StartTime = starttime;
            this.EndTime = endtime;
         

    }  

     public override string ToString()
    {
        return $"{TaskName} - {Description} - {Priority} - {Category} - {Status}";
    }

   
};

public class TimeBlock{
    public int TaskID;
    public string TaskName;
    public DateTime Deadline{ get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; } 

     public TimeBlock(int taskID, string taskName, DateTime deadline, DateTime? starttime, DateTime? endtime){
                    
            this.TaskID = taskID;        
            this.Deadline = deadline;
            this.StartTime = starttime;
            this.EndTime = endtime;
            this.TaskName   = taskName;

    }  
      public override string ToString()
    {
        return TaskName;
    }
}

public class Reminder{
    public int TaskID;
    public string TaskName;
    public DateTime Deadline{ get; set; }
    public String Message { get; set; }


     public Reminder(int taskID, string taskName, DateTime deadline, string message){
                    
            this.TaskID = taskID;        
            this.Deadline = deadline;
           this.Message = message;
            this.TaskName   = taskName;

    }  
      public override string ToString()
    {
        return $"{TaskName} - {Message}";
    }

}

// public class User {
    
// }