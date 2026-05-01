namespace TaskManagementApplication.Test;

public class FileSaverTests
{
    string TestFile = "test-tasks.txt";
    
    public FileSaverTests()
    {
          
        if (File.Exists(TestFile))
        {
            File.Delete(TestFile);
        }
    }

    [Fact]
    public void Constructor_Should_CreateFile_When_File_DoesNot_Exist(){
        
        FileSaver fileSaver = new FileSaver(TestFile);        
        Assert.True(File.Exists(TestFile));
    }

    [Fact]
    public void AppendData_Should_Add_Task_ToFile()
    {
       
        FileSaver fileSaver = new FileSaver(TestFile);

        TaskItem task = new TaskItem(1,"Study","Read C#",DateTime.Parse("2026-05-01"),"High","Personal",DateTime.Parse("2026-05-01"), DateTime.Parse("2026-05-01"),"In Progress");

        
        fileSaver.AppendData(task);

        
        string[] lines = File.ReadAllLines(TestFile);
        Assert.Single(lines);
        Assert.Contains("Study", lines[0]);
    }

     [Fact]
    public void SaveAllData_Should_Overwrite_File_With_AllTasks()
    {
        
        FileSaver fileSaver = new FileSaver(TestFile);

        List<TaskItem> tasks = new List<TaskItem>
        {
          
            new TaskItem(1,"Study","Learn Python",DateTime.Parse("2026-05-01"),"High","Personal", DateTime.Parse("05/05/2026 09:00"), DateTime.Parse("06/05/2026 10:00"),"In Progress"),
            new TaskItem(2,"Exam","CS690 Final Exam",DateTime.Parse("05/01/2026"),"High","Work", DateTime.Parse("04/20/2026 09:00"), DateTime.Parse("04/20/2026 10:00"),"Completed")

        };

        
        fileSaver.SaveAllData(tasks);

        
        string[] lines = File.ReadAllLines(TestFile);
        Assert.Equal(2, lines.Length);
        Assert.Contains("Study", lines[0]);
        Assert.Contains("Exam", lines[1]);
    }

    [Fact]
    public void GetLastTaskID_Should_Return_LastTaskId()
    {
        
        FileSaver fileSaver = new FileSaver(TestFile);

        File.WriteAllLines(TestFile, new[]
        {
            "1|Task1|Desc|2026-05-01|High|Work|09:00|10:00|Pending",
            "2|Task2|Desc|2026-05-02|Low|Home|11:00|12:00|Completed"
        });

       
        int result = fileSaver.getLastTaskID();

        
        Assert.Equal(2, result);
    }

     [Fact]
    public void GetLastTaskID_Should_Return_Zero_When_File_IsEmpty()
    {
       
        FileSaver fileSaver = new FileSaver(TestFile);

        
        int result = fileSaver.getLastTaskID();

        
        Assert.Equal(0, result);
    }




}
