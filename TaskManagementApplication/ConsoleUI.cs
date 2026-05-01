namespace TaskManagementApplication;

using Spectre.Console;


public class ConsoleUI{

    DataManager dataManager;
    TaskService taskService;
    

    public ConsoleUI()
    {
        dataManager = new DataManager();
        taskService = new TaskService(dataManager);
       
        getUserName();
       
    }

    public void  Show(){

        AnsiConsole.MarkupLine("[blue]===============================================[/]");
        AnsiConsole.MarkupLine("[yellow]======== Task Management Application ==========[/]");
        AnsiConsole.MarkupLine("[blue]===============================================[/]");             

        ShowReminders();

        bool exit = false;
        while(!exit){
            string choice = ShowMenu("Please enter your choice",
                                        new[]{
                                            "Create Task",
                                            "View Tasks",
                                            "Time Tracker",
                                            "Review Performance",
                                            "Daily Status Update",
                                            "End"
                                        }
                                    );

            
            if( choice == "Create Task") {
                bool result;
                AnsiConsole.MarkupLine("Please enter task details");
                do {                   
                                                         
                    createTask();
                    result = AnsiConsole.Confirm("Do you want to continue?");
                } while(result);
            


            
            } else if( choice == "View Tasks") {
                              
                    List<TaskItem> taskListItems = taskService.GetAllTasks();

                    var table = new Spectre.Console.Table().Title("[bold cyan] Task Dashboard[/]");
                    table.AddColumn("TaskId");
                    table.AddColumn("Task Name");
                    table.AddColumn("Description");
                    table.AddColumn("Deadline");
                    table.AddColumn("Priority");
                    table.AddColumn("Category");
                    table.AddColumn("Start Time");
                    table.AddColumn("End Time");
                    table.AddColumn("Status");
                    
                    foreach (var task in taskListItems)
                    {
                        table.AddRow(
                            task.TaskId.ToString(),
                            task.TaskName ?? "",
                            task.Description ?? "",
                            task.Deadline.ToShortDateString(),
                            task.Priority ?? "",
                            task.Category ?? "",
                            task.StartTime.ToString() ?? "",
                            task.EndTime.ToString() ?? "",
                            task.Status ?? ""
                           


                        );
                    }

                    AnsiConsole.Write(table);
                    bool subMenuExit;
                    do{
                        int id = GetValidInt("Please enter a Task Id:");                        
                        // Task? editTask = taskListItems.FirstOrDefault(t => t.TaskId == id);
                        TaskItem? editTask = taskService.GetTaskById(id);

                    if (editTask != null)
                    {
                    
                        string operation = ShowMenu($"Please enter your operation",new[]{"Update Task", "Delete Task", "Set Priority", "Set Category", "Schedule Time Block", "Update Status","End"} );

                        if(operation == "Update Task") {
                                updateTask(editTask);
                                

                        } else if(operation == "Delete Task") {                            
                           
                             var confirmDelete = AnsiConsole.Confirm("Do you want to delete?");
                             if(confirmDelete){
                                taskService.DeleteTask(id);                              
                                
                                AnsiConsole.MarkupLine("[green]Task Deleted Successfully[/]");
                                 
                             }
                           
                            
                        } else if(operation == "Set Priority") { 
                       
                          AnsiConsole.WriteLine($"Current Priority:{editTask.Priority}");
                         string priority = ShowSelectionPromptWithPreselectValue("Priority",new[]{"None","High", "Medium", "Low"}, editTask.Priority );
                          AnsiConsole.WriteLine($"Selected Priority:{priority}");
                         taskService.UpdatePriority(id,priority);
                          AnsiConsole.MarkupLine("[green]Priority set successfully[/]");
                            
                        } else if(operation == "Set Category") {   
                         AnsiConsole.WriteLine($"Current Category:{editTask.Category}");
                         string category = ShowSelectionPromptWithPreselectValue("Priority",new[]{"Work", "Errand", "Personal"}, editTask.Category );
                          AnsiConsole.WriteLine($"Selected Category:{category}");
                         taskService.UpdateCategory(id,category);
                          AnsiConsole.MarkupLine("[green]Category set successfully[/]");  
                         
                        } else if(operation == "Update Status") {  

                         AnsiConsole.WriteLine($"Current Status:{editTask.Status}");
                         string status = ShowSelectionPromptWithPreselectValue("Status",new[]{"To Do", "In Progress", "Completed"}, editTask.Status );
                          AnsiConsole.WriteLine($"Selected Status:{status}");
                         taskService.UpdateStatus(id,status);
                          AnsiConsole.MarkupLine("[green]Status set successfully[/]");                  
                             
                   
                            
                        } 
                    }
                    else 
                    {
                       AnsiConsole.MarkupLine("[red]Task Not Found[/]");
                    }
                     subMenuExit = AnsiConsole.Confirm("Do you want to continue managing task?");
               
                    }while(subMenuExit);
                    


               

            } else if( choice == "Review Performance") {
                var table = new Table().Title("[bold cyan] Task Status Review[/]");

                table.AddColumn("ID");
                table.AddColumn("Task");
                table.AddColumn("Deadline");
                table.AddColumn("Status");

                foreach (var task in taskService.GetAllTasks().OrderBy(t => t.Status))
                {
                    string statusColor =
                        task.Status == "Completed" ? "green" :
                        task.Status == "In Progress" ? "yellow" :
                        task.Status == "None" ? "white" :
                        "red";

                    table.AddRow(
                        task.TaskId.ToString(),
                        task.TaskName,
                        task.Deadline.ToShortDateString(),
                        $"[{statusColor}]{task.Status}[/]"
                    );
                }

            AnsiConsole.Write(table);

            AnsiConsole.MarkupLine($"[bold cyan] Task Status Summary Report:[/]");
            int none  = taskService.GetAllTasks().Count(t => t.Status == "None");
            int todo = taskService.GetAllTasks().Count(t => t.Status == "To Do");
            int inProgress = taskService.GetAllTasks().Count(t => t.Status == "In Progress");
            int completed = taskService.GetAllTasks().Count(t => t.Status == "Completed");

            AnsiConsole.MarkupLine($"[white]None:[/] {none}");
            AnsiConsole.MarkupLine($"[red]To Do:[/] {todo}");
            AnsiConsole.MarkupLine($"[yellow]In Progress:[/] {inProgress}");
            AnsiConsole.MarkupLine($"[green]Completed:[/] {completed}");
            } else if(choice == "Time Tracker") {  

                    var tableTrackTime = new Spectre.Console.Table().Title("[bold cyan] Time Tracking Dashboard[/]");
                    tableTrackTime.AddColumn("TaskId");
                    tableTrackTime.AddColumn("Task Name");
                    tableTrackTime.AddColumn("Deadline");
                    tableTrackTime.AddColumn("Start Time");
                    tableTrackTime.AddColumn("End Time");
                    tableTrackTime.AddColumn("Time Spent");
                    double timespent = 0.0;
                    foreach (var task in taskService.GetAllTasks().OrderBy(t => t.Deadline))
                    {
                        if(task.StartTime.HasValue && task.EndTime.HasValue) {
                        timespent = (task.EndTime.Value - task.StartTime.Value).TotalHours;
                        }
                        tableTrackTime.AddRow(
                            task.TaskId.ToString(),
                            task.TaskName ?? "",
                            task.Deadline.ToString(),
                            task.StartTime.ToString() ?? "",
                            task.EndTime.ToString() ?? "",
                            timespent.ToString() ?? "N/A"


                        );
                    }

                    AnsiConsole.Write(tableTrackTime);       
                             
                   
                            
             
             } else if( choice == "Daily Status Update") {
                              
                    List<TaskItem> taskListItems = taskService.GetAllTasks();

                    var table = new Spectre.Console.Table().Title("[bold cyan] Daily Status Update[/]");
                    table.AddColumn("TaskId");
                    table.AddColumn("Task Name");
                    table.AddColumn("Description");
                    table.AddColumn("Deadline");
                    table.AddColumn("Priority");
                    table.AddColumn("Status");
                    
                    foreach (var task in taskListItems)
                    {
                        table.AddRow(
                            task.TaskId.ToString(),
                            task.TaskName ?? "",
                            task.Description ?? "",
                            task.Deadline.ToShortDateString(),
                            task.Priority ?? "",
                            task.Status ?? ""                        


                        );
                    }

                    AnsiConsole.Write(table);
                  
                    int id = GetValidInt("Please enter a Task Id:");                        
                    TaskItem? editTask = taskListItems.FirstOrDefault(t => t.TaskId == id);
                    if (editTask != null) {
                        AnsiConsole.WriteLine($"Current Status:{editTask.Status}");
                        string status = ShowSelectionPromptWithPreselectValue("Status",new[]{"To Do", "In Progress", "Completed"}, editTask.Status );
                        AnsiConsole.WriteLine($"Selected Status:{status}");
                        taskService.UpdateStatus(id,status);
                        AnsiConsole.MarkupLine("[green]Status set successfully[/]");                                       
                    }  else 
                    {
                       AnsiConsole.MarkupLine("[red]Task Not Found[/]");
                    }
                     
                    


               

            } else{
                exit = AnsiConsole.Confirm("Do you really want to exit?");
               
            }
        }
        

        
    }

    public int GetValidInt(string message)
    {
        int value;
        string input = AnsiConsole.Ask<string>(message);

        while (!int.TryParse(input, out value))
        {
            AnsiConsole.MarkupLine("[red]Invalid number. Try again.[/]");
            input = AnsiConsole.Ask<string>(message);
        }

        return value;
}
    
    public DateTime? AskOptionalDate(string message, DateTime? existing = null)
{
    string input = AnsiConsole.Prompt(
        new TextPrompt<string>(message)
            .AllowEmpty()
    );

    return DateTime.TryParse(input, out var result)
        ? result
        : existing;
}

public void ShowReminders()
{
    var today = DateTime.Today;
    var tomorrow = DateTime.Today.AddDays(1);
    // var msgColor = "";
    List<Reminder> reminderList = new List<Reminder>();
    foreach(var eachItem in taskService.GetAllTasks()){
        if(eachItem.Deadline.Date < today){
            // msgColor = "red";
            reminderList.Add(new Reminder(eachItem.TaskId, eachItem.TaskName,eachItem.Deadline,"OverDue"));
        } else if (eachItem.Deadline.Date == today) {
            // msgColor = "yellow";

            reminderList.Add(new Reminder(eachItem.TaskId, eachItem.TaskName,eachItem.Deadline,"Due Today"));
        } else if(eachItem.Deadline.Date  == tomorrow) {
            // msgColor = "blue";
             reminderList.Add(new Reminder(eachItem.TaskId, eachItem.TaskName,eachItem.Deadline,"Due Tomorrow"));
        }

    }
      

    if (reminderList.Count == 0)
    {
        AnsiConsole.MarkupLine("[green]✔ No upcoming reminders[/]");
    }
     else {
        var table = new Spectre.Console.Table().Title("[bold cyan] Task Reminder Dashboard[/]");
                    table.AddColumn("TaskId");
                    table.AddColumn("Task Name");
                    table.AddColumn("Deadline");
                    table.AddColumn("Reminder Message");
                    foreach (var task in reminderList.OrderBy(t => t.Deadline))
                    {
                        string msgColor =
                        task.Message == "OverDue" ? "red" :
                            task.Message == "Due Today" ? "yellow" :
                            "blue";
                        table.AddRow(
                            task.TaskID.ToString(),
                            task.TaskName ?? "",
                            task.Deadline.ToShortDateString(),
                            $"[{msgColor}]{task.Message}[/]"


                        );
                    }

                    AnsiConsole.Write(table);
     }

                    

}

public static string ShowMenu(string title, string[] choices)
{
    return AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title(title)
            .AddChoices(choices)
    );
}

public  string ShowSelectionPromptWithPreselectValue(string title, string[] choices, string existing)
{
    return AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title(title)
            .AddChoices(choices).DefaultValue(existing));
    
}

public void createTask(){
    var taskName = AnsiConsole.Prompt(new TextPrompt<string>("Task Name:").Validate(input =>{
                            return input.Contains("|")
                                ? ValidationResult.Error("[red]Please do not use | symbol.[/]")
                                : ValidationResult.Success();
                        }));
                    var description = AnsiConsole.Prompt(new TextPrompt<string>("Task Description:").Validate(input => {
                                return input.Contains("|")
                                    ? ValidationResult.Error("[red]Please do not use | symbol.[/]")
                                    : ValidationResult.Success();
                            }));
                      DateTime deadline = AnsiConsole.Prompt(new TextPrompt<DateTime>("Deadline [grey](format: MM/dd/yyyy):[/]").Validate(date => {
                                           return date.Date >= DateTime.Today
                                                ? ValidationResult.Success()
                                                : ValidationResult.Error("[red]Date must be today or later[/]");
                                        
                            }));
                    

                         string priority = ShowMenu("Priority",new[]{"None","High", "Medium", "Low"} );
                        AnsiConsole.MarkupLine($"Selected Priority: [white]{priority}[/]");                   

                         string category = ShowMenu("Category",new[]{"None","Work", "Personal", "Errand"} );
                        AnsiConsole.MarkupLine($"Selected Category: [white]{category}[/]");                   
                       
                       string status = ShowMenu("Status",new[]{"None","To Do", "In Progress", "Completed"} );
                        AnsiConsole.MarkupLine($"Selected Status: [white]{status}[/]");

                        DateTime? starttime = AskOptionalDate("Start Time [grey](format: MM/dd/yyyy):[/]");           
                        DateTime? endtime = AskOptionalDate("End Time [grey](format: MM/dd/yyyy):[/]");                
                   
                       
                        TaskItem newTask = new TaskItem(0,taskName,description,deadline,priority,category,starttime,endtime,status);
                       taskService.AddTask(newTask);
                        // dataManager.addtasksToList(newTask); 
                        AnsiConsole.MarkupLine("[green]Task Created Successfully[/]");
}

public void updateTask(TaskItem editTask) {
    string newName = AnsiConsole.Prompt( new TextPrompt<string>("New Task Name:").DefaultValue(editTask.TaskName).Validate(input => {
                                                return input.Contains("|")
                                                    ? ValidationResult.Error("[red]Please do not use | symbol.[/]")
                                                    : ValidationResult.Success();
                                            }));
                                string newDescription = AnsiConsole.Prompt( new TextPrompt<string>("New Description:").DefaultValue(editTask.Description).Validate(input => {
                                                return input.Contains("|")
                                                    ? ValidationResult.Error("[red]Please do not use | symbol.[/]")
                                                    : ValidationResult.Success();
                                            }));
                                 DateTime newDeadLine = AnsiConsole.Prompt( new TextPrompt<DateTime>("New DeadLine [grey](format: MM/dd/yyyy):[/]").DefaultValue(editTask.Deadline).Validate(date =>
                                            date.Date >= DateTime.Today
                                                ? ValidationResult.Success()
                                                : ValidationResult.Error("[red]Date must be today or later[/]")
                                        ));                              
                                
                                  string newPriority = ShowSelectionPromptWithPreselectValue($"Please select your new priority ([green]{editTask.Priority}[/])",new[]{"None","High", "Medium", "Low"},editTask.Priority );
                                     AnsiConsole.MarkupLine($"Selected Priority: [white]{newPriority}[/]");                               

                                string newCategory = ShowSelectionPromptWithPreselectValue($"Please select your new category ([green]{editTask.Category}[/])",new[]{"None","Work", "Personal", "Errand"},editTask.Category );
                                    AnsiConsole.MarkupLine($"Selected Category: [white]{newCategory}[/]");                              

                                string newStatus = ShowSelectionPromptWithPreselectValue($"Please select your new status ([green]{editTask.Status}[/])",new[]{"None","To Do", "In Progress", "Completed"},editTask.Status );
                                    AnsiConsole.MarkupLine($"Selected Status: [white]{newStatus}[/]");
                               
                                DateTime? newStartTime = AskOptionalDate("New Start Time [grey](format: MM/dd/yyyy):[/]", editTask.StartTime);
                                DateTime? newEndTime = AskOptionalDate("New End Time [grey](format: MM/dd/yyyy):[/]", editTask.EndTime);                            
                              

                                TaskItem newTask = new TaskItem(editTask.TaskId,newName,newDescription,newDeadLine,newPriority,newCategory,newStartTime,newEndTime,newStatus);
                                
                                taskService.UpdateTask(newTask);                        
                                
                            
                              AnsiConsole.MarkupLine("[green]Task Updated Successfully[/]");
}
}

public void createProductivityReport(ProductivityReport productivity){                   

                    var table = new Table()
                    
                    .Title($"[bold cyan] Daily Productivity Report[/]{DateTime.Today}");

                table.AddColumn("Metric");
                table.AddColumn("Value");

               
                table.AddRow("Total Tasks", productivity.TotalTask.ToString());
                table.AddRow("Completed", productivity.Completed.ToString());
                table.AddRow("In Progress", productivity.InProgress.ToString());
                table.AddRow("To Do", productivity.ToDo.ToString());
                table.AddRow("Completion Rate", $"{productivity.CompletionRate:0.0}%");

                
                string status =
                    productivity.CompletionRate >= 80 ? "[green]Excellent[/]" :
                    productivity.CompletionRate >= 50 ? "[yellow]Average[/]" :
                    "[red]Low Productivity [/]";

                table.AddRow("Performance", status);

                AnsiConsole.Write(table);
        }
  


public void createSchedule(TimeBlock timeBlock){

    var table = new Table().Title("[bold cyan] Time Block Schedule[/]");

    table.AddColumn("Task Id");
    table.AddColumn("Task Name");
    table.AddColumn("DeadLine");
    table.AddColumn("Start Time");
    table.AddColumn("End Time");
    table.AddColumn("Duration in hrs");

      

            table.AddRow(
                timeBlock.TaskID.ToString(),
                timeBlock.TaskName,
                timeBlock.Deadline.ToString() ?? "",
                timeBlock.StartTime.ToString() ?? "",
                timeBlock.EndTime.ToString() ?? "",
                timeBlock.EstimatedTime.ToString()
            );
            AnsiConsole.Write(table);
    }

    public void getUserName(){
        
        if (string.IsNullOrWhiteSpace(dataManager.User.username))
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();

            User user = new User(name);

            dataManager.User = user;
            dataManager.SaveUser(user);

            Console.WriteLine("User saved successfully!");
        }
        else
        {
            Console.WriteLine($"Welcome back, {dataManager.User.username}!");
        }
    }



}