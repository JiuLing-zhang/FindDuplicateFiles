using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindDuplicateFiles.SearchFile
{
    public class DuplicateFileInfo : SimpleFileInfo
    {
        public string Key { get; set; }
        public new string Size { get; set; }
    }
}
