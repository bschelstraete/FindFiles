using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFilesProject
{
    public class File
    {
        public string Drive { get; private set; }
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public string FileExtension { get; private set; }
        public int FileSize { get; private set; }

        public File(string drive, string filePath, string fileName, string fileExtension, int fileSizeBytes)
        {
            Drive = drive;
            FilePath = filePath;
            FileName = fileName;
            FileExtension = fileExtension;
            FileSize = fileSizeBytes / 1048576;
        }

        public override string ToString()
        {
            return FileName + " " + FileSize + " MB";
        }
    }
}
