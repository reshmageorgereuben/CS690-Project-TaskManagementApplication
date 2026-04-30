namespace TaskManagementApplication;

using Spectre.Console;


public class ConsoleUI{

    DataManager dataManager;

    public ConsoleUI() {
        dataManager = new DataManager();
    }

    public void  Show(){
        AnsiConsole.MarkupLine("[blue]===============================================[/]");
        AnsiConsole.MarkupLine("[yellow]======== Task Management Application ==========[/]");
        AnsiConsole.MarkupLine("[blue]===============================================[/]");
        

        bool exit = false;
        while(!exit){
            string choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Please enter your choice")
                        .AddChoices( "Create Task", "View Tasks", "Review Performance", "End"));

            
            if( choice == "Create Task") {
                bool result;
                AnsiConsole.MarkupLine("Please enter task details");
                do {                    
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
                    // DateTime deadline = AnsiConsole.Prompt(new TextPrompt<DateTime>("DeadLine:"));   
                      DateTime deadline = AnsiConsole.Prompt(new TextPrompt<DateTime>("Deadline:").Validate(date => {
                                           return date.Date >= DateTime.Today
                                                ? ValidationResult.Success()
                                                : ValidationResult.Error("[red]Date must be today or later[/]");
                                        
                            }));
                     string priority = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Priority")
                        .AddChoices("None","High", "Medium", "Low"));
                        AnsiConsole.MarkupLine($"Selected Priority: [white]{priority}[/]");
                    var category =  AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Category")
                        .AddChoices("None","Work", "Personal", "Errand"));
                        AnsiConsole.MarkupLine($"Selected Category: [white]{category}[/]");
                    var status =  AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Status")
                        .AddChoices("None","To Do", "In Progress", "Completed"));
                        AnsiConsole.MarkupLine($"Selected Status: [white]{status}[/]");
                    DateTime starttime = AnsiConsole.Prompt(new TextPrompt<DateTime>("Start Time:").AllowEmpty());   
                    DateTime endtime = AnsiConsole.Prompt(new TextPrompt<DateTime>("End Time:").AllowEmpty());   

                   
                       Task newTask = new Task(0,taskName,description,deadline,priority,category,starttime,endtime,status);
                        dataManager.addtasksToList(newTask); 
                        AnsiConsole.MarkupLine("[green]Task Created Successfully[/]");
                    
                    

                    result = AnsiConsole.Confirm("Do you want to continue?");
                } while(result);
            


            
            } else if( choice == "View Tasks") {
                              
                    List<Task> taskListItems = dataManager.TaskList;

                    var table = new Spectre.Console.Table();
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
                        
                        Task editTask = taskListItems.FirstOrDefault(t => t.TaskId == id);

                    if (editTask != null)
                    {
                       string operation = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Please enter your operation")
                        .AddChoices( "Update Task", "Delete Task", "Set Priority", "Set Category", "Schedule Time block", "Track Time", "End"));


                        if(operation == "Update Task") {
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
                                 DateTime newDeadLine = AnsiConsole.Prompt( new TextPrompt<DateTime>("New DeadLine").DefaultValue(editTask.Deadline).Validate(date =>
                                            date.Date >= DateTime.Today
                                                ? ValidationResult.Success()
                                                : ValidationResult.Error("[red]Date must be today or later[/]")
                                        ));
                                // DateTime newDeadLine = AnsiConsole.Prompt(
                                //     new TextPrompt<DateTime>($"New Deadline ( [green]{editTask.Deadline}[/])")
                                //         .Validate(date =>
                                //             date.Date >= DateTime.Today
                                //                 ? ValidationResult.Success()
                                //                 : ValidationResult.Error("[red]Date must be today or later[/]")
                                //         )
                                // );
                                 string newPriority = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                    .Title($"Please select your  new priority ([green]{editTask.Priority}[/])")
                                    .AddChoices("None","High", "Medium", "Low").DefaultValue(editTask.Priority));
                                     AnsiConsole.MarkupLine($"Selected Priority: [white]{newPriority}[/]");
                                var newCategory =  AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                    .Title($"Please select your new category ([green]{editTask.Category}[/])")
                                    .AddChoices("None","Work", "Personal", "Errand").DefaultValue(editTask.Category));
                                     AnsiConsole.MarkupLine($"Selected Priority: [white]{newCategory}[/]");
                                   var newStatus =   AnsiConsole.Prompt( new SelectionPrompt<string>()
                                    .Title($"Please select your new status ([green]{editTask.Status}[/])")
                                    .AddChoices("None","To Do", "In Progress", "Completed").DefaultValue(editTask.Status));
                                    AnsiConsole.MarkupLine($"Selected Status: [white]{newStatus}[/]");
                               
                                DateTime? newStartTime = AskOptionalDate("New Start Time:", editTask.StartTime);
                                DateTime? newEndTime = AskOptionalDate("New End Time:", editTask.EndTime);
                              

                               
                               
                                foreach(var eachItem in dataManager.TaskList ){
                                    if(eachItem.TaskId == editTask.TaskId){
                                        eachItem.TaskName = newName;
                                        eachItem.Description = newDescription;
                                        eachItem.Deadline = newDeadLine;
                                        eachItem.Priority = newPriority;
                                        eachItem.Category = newCategory;
                                        eachItem.StartTime = newStartTime;
                                        eachItem.EndTime = newEndTime;
                                        eachItem.Status = newStatus;
                                    }
                                }
                                
                                
                              dataManager.SaveAllData();
                              AnsiConsole.MarkupLine("[green]Task Updated Successfully[/]");
                                

                        } else if(operation == "Delete Task") {                            
                           
                             var confirmDelete = AnsiConsole.Confirm("Do you want to delete?");
                             if(confirmDelete){
                               
                                 dataManager.TaskList.RemoveAll(a => a.TaskId == id);
                                dataManager.SaveAllData();
                                AnsiConsole.MarkupLine("[green]Task Deleted Successfully[/]");
                                 
                             }
                           
                            
                        } else if(operation == "Set Priority") { 

                                foreach (var eachItem in dataManager.TaskList)
                                {
                                    if (eachItem.TaskId == editTask.TaskId)
                                    {   
                                        AnsiConsole.WriteLine($"Current Priority:{eachItem.Priority}");
                                        
                                        string priority = AnsiConsole.Prompt(
                                            new SelectionPrompt<string>()
                                                .Title("Please select your priority")
                                                .AddChoices("High", "Medium", "Low")
                                        );
                                        AnsiConsole.WriteLine($"Selected Priority:{priority}");
                                        eachItem.Priority = priority;
                                    }
                                }


                      
                          dataManager.SaveAllData();
                          AnsiConsole.MarkupLine("[green]Priority set successfully[/]");
                            
                        } else if(operation == "Set Category") {          

                            foreach (var eachItem in dataManager.TaskList)
                                {
                                    if (eachItem.TaskId == editTask.TaskId)
                                    {   
                                        AnsiConsole.WriteLine($"Current Category:{eachItem.Category}");
                                        
                                        string category = AnsiConsole.Prompt(
                                            new SelectionPrompt<string>()
                                                .Title("Please select your category")
                                                .AddChoices("Work", "Errand", "Personal")
                                        );
                                        AnsiConsole.WriteLine($"Selected Category:{category}");
                                        eachItem.Category = category;
                                    }
                                }


                      
                          dataManager.SaveAllData();
                          AnsiConsole.MarkupLine("[green]Category set successfully[/]");                  
                             
                        //      string category = AnsiConsole.Prompt(
                        // new SelectionPrompt<string>()
                        // .Title("Please select your catergory")
                        // .AddChoices( "Work", "Personal", "Errand"));
                        //      foreach(var eachItem in dataManager.TaskList ){
                        //             if(eachItem.TaskId == editTask.TaskId){
                        //                 eachItem.Category = category;
                                        
                        //             }
                        //         }
                        //     dataManager.SaveAllData();
                        //      AnsiConsole.MarkupLine("[green]Category set successfully[/]");
                            
                        }
                    }
                    else 
                    {
                        Console.WriteLine("Task not found");
                    }
                     subMenuExit = AnsiConsole.Confirm("Do you want to continue managing task?");
               
                    }while(subMenuExit);
                    


               

            } else if( choice == "Review Performance") {
                Console.WriteLine("Selected Performance REview");
            }  else{
                exit = AnsiConsole.Confirm("Do you really want to exit?");
                // if(exit){
                  
                // }
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
    
    DateTime? AskOptionalDate(string message, DateTime? existing)
{
    string input = AnsiConsole.Prompt(
        new TextPrompt<string>(message)
            .AllowEmpty()
    );

    return DateTime.TryParse(input, out var result)
        ? result
        : existing;
}

    }