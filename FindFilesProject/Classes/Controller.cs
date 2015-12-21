using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFilesProject.Classes
{
    public class Controller
    {
        public string[] Extensions { get; private set; }
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public List<File> FileList { get; private set; }

        private CsvBuilder csvBuilder = new CsvBuilder();

        private List<Observer> observerList;

        private Task searchTask;

        public Controller()
        {
            InitVariables();
        }

        private void InitVariables()
        {
            FileList = new List<File>();
            observerList = new List<Observer>();
            Extensions = new[] { "*.ade", "*.adp", "*.adn", "*.accdb", "*.accdr", "*.mdb"
                ,"*.cdb", "*.mdw", "*.mdf", "*.mde", "*.accde", "*.ldb", "*.xlsm", "*.xltm", "*.xlam", "*.xlm" };
        }

        public void Subscribe(Observer observer)
        {
            if (observerList.Find(o => o == observer) == null)
                observerList.Add(observer);
        }

        public void Unsubscribe(Observer observer)
        {
            if (observerList.Find(o => o == observer) != null)
            {
                observerList.Add(observer);
            }
        }

        private void NotifyObservers(File file)
        {
            foreach (Observer o in observerList)
            {
                o.Update(file);
            }
        }

        private void NotifyObserversStarted()
        {
            foreach (Observer o in observerList)
            {
                o.UpdateStarted();
            }
        }

        private void NotifyObserversFinished()
        {
            searchTask = null;
            foreach (Observer o in observerList)
            {
                o.UpdateFinished();
            }
        }

        private void GetFiles(string path, List<File> files)
        {
            try
            {
                var dir = new DirectoryInfo(path);
                foreach (string extension in Extensions)
                {
                    dir.GetFiles(extension)
                        .ToList()
                        .ForEach(s => { AddFiles(s); });
                }

                Directory.GetDirectories(path)
                    .ToList()
                    .ForEach(s => GetFiles(s, files));
            }
            catch (UnauthorizedAccessException)
            {

            }
        }

        private void AddFiles(FileInfo fileInfo)
        {
            int byteSize = 0;
            int.TryParse(fileInfo.Length.ToString(), out byteSize);
            File newFile = new File(fileInfo.Directory.Root.ToString(), fileInfo.FullName.ToString(), fileInfo.Name, fileInfo.Extension, byteSize);
            FileList.Add(newFile);
            NotifyObservers(newFile);
        }

        public void Start()
        {
            if (searchTask == null)
            {
                NotifyObserversStarted();
                searchTask = Task.Factory.StartNew(() =>
                {
                    GetFiles(InputPath, FileList);
                    csvBuilder.BuildCsv(FileList, OutputPath);
                    NotifyObserversFinished();
                });
            }
        }

        public void Stop()
        {
            if (searchTask.Status == TaskStatus.Running || searchTask.Status == TaskStatus.Created ||searchTask.Status == TaskStatus.RanToCompletion)
            {
                searchTask.Dispose();
                searchTask = null;
            }
        }
    }
}
