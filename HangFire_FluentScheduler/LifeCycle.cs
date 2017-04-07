using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_FluentScheduler
{
   public class LifeCycle
    {
        public int Month { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }

        private int _second = 1;
        public int Second { get => _second; set => _second = value; }
    }
}
