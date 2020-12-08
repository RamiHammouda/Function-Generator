using Newtonsoft.Json;
using System;

namespace WpfApp2.Model
{

    public enum WaveForm
    {
        Sine,
        Sawtooth,
        Square,
        Triangle,
        Random,
        RandomDigital,
    }
    public class SignalProfile
    {

        private bool mBiggerThanZero = true;
        [JsonProperty("Wave")]
        public WaveForm mWave { get; set; }

        [JsonProperty("Freq")]
        private double mFrequency;

        [JsonProperty("Ampl")]
        private double mAmplitude;
        //private int mTick;
        [JsonProperty("Rate")]
        private long mRate;

        public SignalProfile(WaveForm wave = 0, double freq = 100, double ampl = 5, long rate = 9600)
        {
            mFrequency = freq;
            mAmplitude = ampl;
            mWave = wave;
            mRate = rate;
        }
        public double getFreq()
        {
            return mFrequency;
        }
        public void setFreq(double freq = 0)
        {
            mFrequency = freq;
        }
        public double getAmpl()
        {
            return mAmplitude;
        }
        public void setAmpl(double ampl)
        {
            mAmplitude = ampl;
        }

        public void setWave(WaveForm wave)
        {
            mWave = wave;
        }

        public WaveForm getWave()
        {
            return mWave;
        }
        public void setRate(long rate)
        {
            mRate = rate;
        }

        public long getRate()
        {
            return mRate;
        }

        public void PrintInfo()
        {
            Console.WriteLine(ToString()); ;
        }
        public override string ToString()
        {
            return $"a Profile Wave:{mWave} Freq:{mFrequency} Hz, Ampl:{mAmplitude} V, Rate:{mRate}";
        }

        public bool checkedSgProfValidatation()
        {
            return (mAmplitude > 0) && (mFrequency > 0) && (mRate > 0);
        }
    }
}
