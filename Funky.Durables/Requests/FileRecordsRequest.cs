using System.Collections.Generic;
using Funky.Durables.Patterns.Monitor;

namespace Funky.Durables.Requests
{
    public class FileRecordsRequest
    {
        public List<FileRecord> Records { get; set; }
    }
}