using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    public class Sawtooth : BaseWave
    {
        public Sawtooth(long freq = 0, double ampl = 0, int tick = 0) : base(freq, ampl, tick) { }
        public override double generateAmpY(double X)
        {
            return Math.Sin(X);
        }
    }
}
