using Newtonsoft.Json;
using System;

namespace WpfApp2.Model
{

    public enum WaveForm
    {
        Sine,
        Sawtooth,
        Random
    }
    public class SignalProfile
    {
        [JsonProperty("Wave")]
        public WaveForm mWave { get; set; }
        [JsonProperty("Freq")]
        private long mFrequency;
        [JsonProperty("Ampl")]
        private double mAmplitude;
        //private int mTick;
        [JsonProperty("Rate")]
        private long mRate;

        public SignalProfile(WaveForm wave = 0, long freq = 100, double ampl = 5, long rate = 9600)
        {
            mFrequency = freq;
            mAmplitude = ampl;
            mWave = wave;
            mRate = rate;
        }
        public long getFreq()
        {
            return mFrequency;
        }
        public void setFreq(long freq = 0)
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
            Console.WriteLine($"a Profile Wave:{mWave} Freq:{mFrequency} Hz, Ampl:{mAmplitude} V, Rate:{mRate}");
        }
        public override string ToString()
        {
            return $"a Profile Wave:{mWave} Freq:{mFrequency} Hz, Ampl:{mAmplitude} V, Rate:{mRate}";
        }
    };
}
