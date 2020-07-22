﻿using System.Collections.Generic;
using Funky.Durables.Models;
using Funky.Durables.Patterns.FunctionChaining;

namespace Funky.Durables.Requests
{
    public class FileRecordsRequest
    {
        public List<CustomerFileRecord> Records { get; set; }
    }
}