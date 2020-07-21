using System.Collections.Generic;

namespace Funky.Durables.Models
{
    public class FileInformation
    {
        public List<FileRecord> ValidRecords { get; set; }
        public List<FileRecord> InvalidRecords { get; set; }
        public List<FileRecord> InvalidRowRecords { get; set; }
    }
}