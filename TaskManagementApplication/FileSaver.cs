namespace TaskManagementApplication;

using System.IO;


public class FileSaver
{
    string fileName;
    public FileSaver(string fileName)
    {
        this.fileName = fileName;
        if (!File.Exists(this.fileName))
        {
            File.Create(this.fileName).Close();
        }
        
    }

    public void AppendLine(string line)
    {
        File.AppendAllText(this.fileName, line + Environment.NewLine);
    }

       public void AppendData(Task data) {
        File.AppendAllText(this.fileName, data.TaskId  + "|" +  data.TaskName + "|" + data.Description + "|" + data.Deadline + Environment.NewLine);
    }

      public void SaveAllData(List<Task> data) {
       

                var lines = new List<string>();

                foreach (var item in data)
                {
                    lines.Add($"{item.TaskId}|{item.TaskName}|{item.Description}|{item.Deadline}|{item.Priority}|{item.Category}");
                }
                File.WriteAllLines(this.fileName, lines);
        
       
    }

      public int getLastTaskID() {
          var taskFileContent = File.ReadAllLines("tasks-list.txt");

          if(taskFileContent.Length > 0){
            var lastItem = taskFileContent[^1];
            var parts = lastItem.Split('|');
            return int.Parse(parts[0]);
          }
          return  0;
       
    }


}