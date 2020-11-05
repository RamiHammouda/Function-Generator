using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using WpfApp2.Model;

namespace WpfApp2.ViewModel
{
    class GenerateSignalData
    {
        private SignalProfile mProfile;
        [JsonProperty("SimuProfile")]
        private SimulationProfile mSmProfile;

        [JsonProperty("No")]
        private long[] mNo;
        [JsonProperty("TimeStamp")]
        private long[] mTimeStampArray;
        [JsonProperty("Ampl")]
        private double[] mAmplArray;

        //For quick access and control;
        private WaveForm mWave;
        private long mFreq;
        private double mAmpl;
        private long mRate;
        private double mDuration;
        private long aTimeStep;
        private long mENumber;
        private delegate double GetWaveValue(long inputTime);
        private GetWaveValue getWaveValue;

        public GenerateSignalData(SignalProfile aProfile, double duration)
        {
            mProfile = aProfile;
            mSmProfile = new SimulationProfile(aProfile, duration);
            mWave = mProfile.getWave();
            mFreq = mProfile.getFreq();
            mAmpl = mProfile.getAmpl();
            mRate = mProfile.getRate();
            mDuration = duration;
            InitiateData();
            GenerateData();
        }
        public GenerateSignalData(SimulationProfile simuProfile)
        {
            mSmProfile = simuProfile;
            mProfile = mSmProfile.getSignalProfile();
            mWave = mProfile.getWave();
            mFreq = mProfile.getFreq();
            mAmpl = mProfile.getAmpl();
            mRate = mProfile.getRate();
            mDuration = mSmProfile.getDuration();
            InitiateData();
            GenerateData();
        }
        public GenerateSignalData(WaveForm wave = 0, long freq = 100, double ampl = 5, long rate = 50, double duration = 1)
        {
            mProfile = new SignalProfile(wave, freq, ampl, rate);
            mSmProfile = new SimulationProfile(mProfile, duration);
            mWave = wave;
            mFreq = freq;
            mAmpl = ampl;
            mRate = rate;
            mDuration = duration;
            InitiateData();
            GenerateData();
        }

        public long[] getTimeStamp()
        {
            return mTimeStampArray;
        }
        public double[] getAmplData()
        {
            return mAmplArray;
        }

        public long[] getNo()
        {
            return mNo;
        }

        public SignalProfile getSignalProfile()
        {
            return mProfile;
        }

        public long getFreq()
        {
            return mFreq;
        }

        public long getRate()
        {
            return mRate;
        }

        public WaveForm getWave()
        {
            return mWave;
        }
        public double getDuration()
        {
            return mDuration;
        }

        public double getAmpl()
        {
            return mAmpl;
        }
        //Hardcore to Desktop first
        public void ExportToJson()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = desktopPath + @"\SignalGenerator.json";

            var serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            //var serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
            using (StreamWriter writer = File.CreateText(filePath)) { serializer.Serialize(writer, this); }
        }

        public void GenerateData()
        {
            // A Time-Step in Ticks
            aTimeStep = (long)Math.Round(1e7m / mRate, 0);
            long start = DateTime.Now.Ticks;

            for (long i = 0; i < mENumber; i++)
            {
                mNo[i] = i;
                mTimeStampArray[i] = start + i * aTimeStep;
                mAmplArray[i] = getWaveValue(i * aTimeStep);
            };

        }

        private void InitiateData()
        {
            mENumber = (long)Math.Round(mRate * mDuration,0);
            mNo = new long[mENumber];
            mTimeStampArray = new long[mENumber];
            mAmplArray = new double[mENumber];
            //GetWaveValue getWaveValue;
            if (mWave == WaveForm.Sine)
                getWaveValue = GetSineValue;
            else if (mWave == WaveForm.Sawtooth)
                getWaveValue = GetSawtoothValue;
            else getWaveValue = GetRandomValue;
        }
        
        public double GetSineValue(long atTimeInTicks)
        {
            return Math.Round(mAmpl * Math.Sin(2 * Math.PI * mFreq * atTimeInTicks / 1e7), 10);
        }
        public double GetRandomValue(long atTimeInTicks)
        {
            return 3.5;
        }

        public double GetSawtoothValue(long atTimeInTicks)
        {
            return 2.5;
        }

        public void PrintData()
        {
            mSmProfile.PrintInfo();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < mENumber; i++)
            {
                sb.AppendLine($"{mNo[i]}-{mTimeStampArray[i]}-{mAmplArray[i]}");
            }
            Console.WriteLine(sb);
        }

        public static double[] ConvertToDouble(long[] arr)
        {
            long length = arr.Length;
            double[] result = new double[length];
            for (long i = 0; i < length;i++ )
            {
                result[i] = (double)arr[i];
            }
            return result;
        }

       
    }
}
