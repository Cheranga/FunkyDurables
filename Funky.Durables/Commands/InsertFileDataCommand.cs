using System;
using System.Collections.Generic;
using System.Text;
using Funky.Durables.Patterns.Monitor;

namespace Funky.Durables.Commands
{
    public class InsertFileDataCommand
    {
        public string Category { get; set; }
        public List<FileRecord> Records { get; set; }
    }
}
