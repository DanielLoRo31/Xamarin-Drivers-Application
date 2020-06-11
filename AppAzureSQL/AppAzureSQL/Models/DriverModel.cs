using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppAzureSQL.Models
{
    public class DriverModel
    {
        public int IDDriver { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }

        public string Status { get; set; }

        public PositionModel ActualPosition { get; set; }
    }
}
