using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFilesProject.Classes
{
    public class CsvBuilder
    {
        public void BuildCsv(List<File> files, string output)
        {
            using (StreamWriter writer = new StreamWriter(output + "\\Access en Excel applicaties.csv"))
            {
                writer.WriteLine("Drive;Full path;Filename;FileExtension;Filesize");
                foreach (File file in files)
                {
                    writer.WriteLine(SerializeFile(file));
                }
            }
        }

        private string SerializeFile(File file)
        {
            string csvFile = "";

            csvFile = file.Drive + ";" + file.FilePath + ";" + file.FileName + ";" + file.FileExtension + ";" + file.FileSize.ToString() + " MB";

            return csvFile;
        }
    }
}
