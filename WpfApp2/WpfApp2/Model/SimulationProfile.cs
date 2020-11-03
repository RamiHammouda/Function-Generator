using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    class SimulationProfile
    {

        [JsonProperty("SignalProfile")]
        private SignalProfile mSgProfile;
        [JsonProperty("Last")]
        private long mDuration;

        public SimulationProfile(SignalProfile aProfile, long duration)
        {
            mSgProfile = aProfile;
            mDuration = duration;
        }

        public SimulationProfile(WaveForm wave = 0, long freq = 130, double ampl = 5, long rate = 9600, long duration = 1)
        {
            mSgProfile = new SignalProfile();
            mSgProfile.setWave(wave);
            mSgProfile.setFreq(freq);
            mSgProfile.setAmpl(ampl);
            mSgProfile.setRate(rate);
            mDuration = duration;
        }

        public SignalProfile getSignalProfile()
        {
            return mSgProfile;
        }
        public void setDuration(long second)
        {
            mDuration = second;
        }

        public long getDuration()
        {
            return mDuration;
        }

        public double getAmpl()
        {
            return mSgProfile.getAmpl();
        }

        public WaveForm getWave()
        {
            return mSgProfile.getWave();
        }

        public long getRate()
        {
            return mSgProfile.getRate();
        }

        public long getFreq()
        {
            return mSgProfile.getFreq();
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Simulation {mSgProfile}, Duration {mDuration}");
        }

        public override string ToString()
        {
            return $"Simulation {mSgProfile}, Duration {mDuration}";
        }
    }
}
