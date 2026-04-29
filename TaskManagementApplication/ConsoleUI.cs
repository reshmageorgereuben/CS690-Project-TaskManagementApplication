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
                    var taskName = AnsiConsole.Prompt(new TextPrompt<string>("Task Name:"));
                    var description = AnsiConsole.Prompt(new TextPrompt<string>("Task Description:"));
                    DateTime deadline = AnsiConsole.Prompt(new TextPrompt<DateTime>("DeadLine:"));                    

                    Task newTask = new Task(0,taskName,description,deadline);
                    dataManager.addtasksToList(newTask);

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
                    foreach (var task in taskListItems)
                    {
                        table.AddRow(

                            task.TaskId.ToString(),
                            task.TaskName,
                            task.Description,
                            task.Deadline.ToShortDateString(),
                            task.Priority,
                            task.Category,
                             "",
                             ""                          


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
                        .AddChoices( "Update Task", "Delete Task", "Set Priority", "Set Category", "Schedule Time block", "End"));


                        if(operation == "Update Task") {
                                string newName = AnsiConsole.Prompt( new TextPrompt<string>("New Task Name:").DefaultValue(editTask.TaskName));
                                string newDescription = AnsiConsole.Prompt( new TextPrompt<string>("New Description:").DefaultValue(editTask.Description));
                                DateTime newDeadLine = AnsiConsole.Prompt( new TextPrompt<DateTime>("New DeadLine").DefaultValue(editTask.Deadline));

                             
                                foreach(var eachItem in dataManager.TaskList ){
                                    if(eachItem.TaskId == editTask.TaskId){
                                        eachItem.TaskName = newName;
                                        eachItem.Description = newDescription;
                                        eachItem.Deadline = newDeadLine;
                                    }
                                }
                                
                                
                              dataManager.SaveAllData();
                                

                        } else if(operation == "Delete Task") {                            
                           
                             var confirmDelete = AnsiConsole.Confirm("Do you want to delete?");
                             if(confirmDelete){
                               
                                 dataManager.TaskList.RemoveAll(a => a.TaskId == id);
                                 
                             }
                            dataManager.SaveAllData();
                            
                        } else if(operation == "Set Priority") {                            
                             
                             string priority = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Please select your priority")
                        .AddChoices( "High", "Medium", "Low"));
                             foreach(var eachItem in dataManager.TaskList ){
                                    if(eachItem.TaskId == editTask.TaskId){
                                        eachItem.Priority = priority;
                                        
                                    }
                                }
                            dataManager.SaveAllData();
                            
                        } else if(operation == "Set Category") {                            
                             
                             string category = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Please select your priority")
                        .AddChoices( "Work", "Personal", "Errand"));
                             foreach(var eachItem in dataManager.TaskList ){
                                    if(eachItem.TaskId == editTask.TaskId){
                                        eachItem.Category = category;
                                        
                                    }
                                }
                            dataManager.SaveAllData();
                            
                        }
                    }
                    else
                    {
                        Console.WriteLine("Task not found");
                    }
                     subMenuExit = AnsiConsole.Confirm("Do you want to continue?");
               
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
}