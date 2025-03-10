﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIServerSmartHome.Entities
{
    [Table("PowerDevice")]
    public class PowerDevice : BasicEntity<PowerDevice>
    {
        public double PowerValue { get; set; }
        public DateTime TimeUsing { get; set; }

        public int? DeviceId { get; set; }
        public Device? Device { get; set; }
    }
}
