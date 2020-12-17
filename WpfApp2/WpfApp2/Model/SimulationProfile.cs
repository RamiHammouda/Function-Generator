using Newtonsoft.Json;
using System;

namespace WpfApp2.Model
{
    public class SimulationProfile
    {
        [JsonProperty("SignalProfile")]
        private SignalProfile mSgProfile;
        [JsonProperty("Last")]
        private double mDuration;

        public SimulationProfile(SignalProfile aProfile, double duration=2)
        {
            mSgProfile = aProfile;
            mDuration = duration;
        }

        public SimulationProfile(WaveForm wave = 0, double freq = 0.2, double ampl = 5, double rate = 2, double duration = 0)
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

        public double getDuration()
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

        public double getRate()
        {
            return mSgProfile.getRate();
        }

        public double getFreq()
        {
            return mSgProfile.getFreq();
        }

        public void PrintInfo()
        {
            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return $"Simulation {mSgProfile}, Duration: {mDuration}";
        }

        public bool checkedSmProfValidation()
        {
            return mSgProfile.checkedSgProfValidatation() && mDuration >= 0;
        }
    }
}
