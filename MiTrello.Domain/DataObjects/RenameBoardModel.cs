﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniTrello.Api.Models
{
    public class RenameBoardModel
    {
        public long BoardToRename { get; set; }
        public string NewTitle { get; set; }
    }
}