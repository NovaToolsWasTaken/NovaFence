﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaFence.Common.Models
{
    internal class Update
    {
        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
