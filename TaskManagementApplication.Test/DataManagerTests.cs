namespace TaskManagementApplication.Test;

public class DataManagerTests
{
    
    string TestFile = "tasks-list.txt";

    

     public DataManagerTests()
    {
        
        if (File.Exists(TestFile))
        {
            File.Delete(TestFile);
        }
    }

    [Fact]
    public void Constructor_Should_Initialize_EmptyLists_WhenFileIsEmpty()
    {
       
        DataManager dm = new DataManager(); // Initializes TaskList array       
        Assert.NotNull(dm.TaskList);       
        Assert.Empty(dm.TaskList);
       
    }

      [Fact]
        public void AddTasksToList_Should_Add_Task_And_IncreaseCount()
        {
            
            DataManager dm = new DataManager();

            TaskItem task = new TaskItem(1,"Study","Read C#",DateTime.Parse("2026-05-01"),"High","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress");
           
            dm.addtasksToList(task);

            
            Assert.Single(dm.TaskList);
            Assert.Equal(1, dm.TaskList[0].TaskId);
        }

         [Fact]
            public void GetFileContents_Should_Return_FileLines()
            {
                
                File.WriteAllLines(TestFile, new[]
                {
                    "1|Task1|Desc|2026-05-01|High|Work|09:00|10:00|Pending"
                });

                DataManager dm = new DataManager();

                
                var result = dm.getFileContents();

                
                Assert.Single(result);
                Assert.Contains("Task1", result[0]);
            }

     [Fact]
    public void GetListOfTasks_Should_Parse_Tasks_Correctly()
    {
      
        File.WriteAllLines(TestFile, new[]
        {
            "1|Task1|Desc1|2026-05-01|High|Work|2026-05-01|2026-05-01|Pending",
            "2|Task2|Desc2|2026-05-02|Low|Home|2026-05-02|2026-05-02|Completed"
        });

        DataManager dm = new DataManager();

        
        var tasks = dm.getListOfTasks();

        
        Assert.Equal(2, tasks.Count);
        Assert.Equal("Task1", tasks[0].TaskName);
        Assert.Equal("Task2", tasks[1].TaskName);
    }

    
    [Fact]
    public void SaveAllData_Should_Write_Tasks_ToFile()
    {
        
        DataManager dm = new DataManager();
        TaskItem task = new TaskItem(1,"Study","Read C#",DateTime.Parse("2026-05-01"),"High","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress");
        dm.TaskList.Add(task);

      
        dm.SaveAllData();

    
        var lines = File.ReadAllLines(TestFile);
        Assert.Single(lines);
        Assert.Contains("Study", lines[0]);
    }






}
