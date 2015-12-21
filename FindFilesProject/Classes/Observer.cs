using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFilesProject.Classes
{
    public interface Observer
    {
        void UpdateStarted();
        void UpdateFinished();
        void Update(File file);
    }
}
