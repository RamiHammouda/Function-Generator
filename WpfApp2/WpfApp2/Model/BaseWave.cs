using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    
    public abstract class BaseWave
    {
        private long mFrequency;
        private double mAmplitude;
        private int mTick;
        //private WaveForm mWave;

        public BaseWave(long freq = 0, double ampl = 0, int tick = 0)
        {
            mFrequency = freq;
            mAmplitude = ampl;
            mTick = tick;
        }
        public long getFreq(){
            return mFrequency;
        }
        public void setFreq(long freq=0)
        {
            mFrequency = freq;
        }
        public double getAmpl()
        {
            return mAmplitude;
        }
        public void setAmpl(float ampl)
        {
            mAmplitude = ampl;
        }

        public void setTick(int tick)
        {
            mTick = tick;
        }

        public int getTick()
        {
            return mTick;
        }
        //public void setWave(WaveForm wave)
        //{
        //    mWave = wave;
        //}

        public abstract double generateAmpY(double X);
    };
}
