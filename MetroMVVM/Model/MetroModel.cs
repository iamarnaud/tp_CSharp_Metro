﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MetroMVVM.Model
{
   public class MetroModel
    {
        // classes générées par json2csharp. 
        // Cette classe est l'objet
        public string id { get; set; }
        public string name { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
        public List<string> lines { get; set; }

        public int Dist { get; set; }
    }
}
