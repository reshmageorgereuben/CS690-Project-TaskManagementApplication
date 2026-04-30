namespace TaskManagementApplication;

public class Task{

   public int TaskId;
   public string TaskName { get; set; }
   public string Description { get; set; }
   public DateTime Deadline{ get; set; }
   public string Priority { get; set; }
   public string Category { get; set; }
//    public DateTime StartTime { get; set; }
//    public DateTime EndTime { get; set; }
     // Optional fields (safe)
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
   public string Status;
   


    public Task(int taskID, string taskName, string description, DateTime deadline, string priority, string category,
    DateTime? starttime, DateTime? endtime, string status){
                    
            this.TaskId = taskID;        
            this.TaskName = taskName;
            this.Description = description;
            this.Deadline = deadline;
            this.Priority = priority;
            this.Category = category;
            this.Status = status;

    }  

     public override string ToString()
    {
        return $"{TaskName} - {Description} - {Priority} - {Category} - {Status}";
    }

   
};