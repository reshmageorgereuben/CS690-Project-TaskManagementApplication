namespace TaskManagementApplication;

using System.IO;


public class FileSaver
{
    public string fileName;
    public FileSaver(string fileName)
    {
        this.fileName = fileName;
        if (!File.Exists(this.fileName))
        {
            File.Create(this.fileName).Close();
        }
        
    }
    public void AppendData(TaskItem data) {
        File.AppendAllText(this.fileName, data.TaskId  + "|" +  data.TaskName + "|" + data.Description + "|" + data.Deadline + "|" + data.Priority + "|" + data.Category + "|" + data.StartTime + "|" + data.EndTime + "|" +  data.Status+"|" + Environment.NewLine);
    }

      public void SaveAllData(List<TaskItem> data) {     

            var lines = new List<string>();

            foreach (var item in data)
            {
                lines.Add($"{item.TaskId}|{item.TaskName}|{item.Description}|{item.Deadline}|{item.Priority}|{item.Category}|{item.StartTime}|{item.EndTime}|{item.Status}");
            }
            File.WriteAllLines(this.fileName, lines);
    
       
    }

      public int getLastTaskID() {
          var taskFileContent = File.ReadAllLines(this.fileName);

          if(taskFileContent.Length > 0){
            var lastItem = taskFileContent[^1];
            var parts = lastItem.Split('|');
           if (int.TryParse(parts[0], out int id))
            {
                return id;
            }
          
       
    }

  return 0;
}

}