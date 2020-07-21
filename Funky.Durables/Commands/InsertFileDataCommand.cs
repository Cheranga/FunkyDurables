using System;
using System.Collections.Generic;
using System.Text;
using Funky.Durables.Models;
using Funky.Durables.Patterns.FunctionChaining;

namespace Funky.Durables.Commands
{
    public class InsertFileDataCommand
    {
        public string Category { get; set; }
        public List<FileRecord> Records { get; set; }
    }
}
