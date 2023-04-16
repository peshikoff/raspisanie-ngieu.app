using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Try1RASP.Models
{
    public partial class RaspisanieModel
    {
        public string Week { get; set; } = null!;
        public string Day { get; set; } = null!;

        public byte Number { get; set; }
        public string Time { get; set; } = null!;
        public string Group { get; set; } = null!;

        public string Lesson { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string FIO { get; set; } = null!;

        public string Room { get; set; } = null!;
    }
}

/*
 * Text ="{Binding raspisanie.Day}"
                              Text ="{Binding raspisanie.Number}"
                              Text ="{Binding raspisanie.Time}"
                              Text="{Binding raspisanie.Group}"
                              Text ="{Binding raspisanie.Lesson}"
                              Text="{Binding raspisanie.Type}"
                              Text="{Binding raspisanie.FIO}"
                              Text="{Binding raspisanie.Room}"/>
 */
