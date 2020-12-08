﻿using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.RightsManagement;
using System.Text;
using WpfApp2.Model;

namespace WpfApp2.ViewModel
{
    public class GenerateSignalData
    {
        [JsonIgnore]
        public SignalProfile mProfile;
        [JsonProperty("SimuProfile")]
        private SimulationProfile mSmProfile;

        [JsonProperty("TargetOnDB")]
        public string mTargetOnDB { get; set; }

        [JsonProperty("Remark")]
        public string mRemark { get; set; }

        [JsonProperty("No")]
        private long[] mNo;
        [JsonProperty("TimeStamp")]
        private long[] mTimeStampArray;
        [JsonProperty("Ampl")]
        private double[] mAmplArray;

        //For quick access and control;
        
        [JsonIgnore]
        public WaveForm mWave { get; set; }
        [JsonIgnore]
        public double mFreq { get; set; }
        [JsonIgnore]
        public  double mAmpl { get; set; }
        [JsonIgnore]
        public long mRate { get; set; }
        [JsonIgnore]
        public double mDuration { get; set; }
        private long aTimeStep;
        private long mENumber;
        private delegate double GetWaveValue(long inputTime);
        private GetWaveValue getWaveValue;
        [JsonIgnore]
        public bool mSendToDB { get; set; }
       

        public GenerateSignalData()
        {
            mProfile = new SignalProfile();
            mSmProfile = new SimulationProfile(mProfile);
            mWave = mProfile.getWave();
            mFreq = mProfile.getFreq();
            mAmpl = mProfile.getAmpl();
            mRate = mProfile.getRate();
            mDuration = mSmProfile.getDuration();
            bool autoTriggerData = false;
            mSendToDB = false;
            mTargetOnDB = "Inputs_Wasserstrahl2_Status";
            mRemark = "Test2";
            InitiateData();
            if (autoTriggerData)
            { GenerateData(); }
        }
        public GenerateSignalData(SignalProfile aProfile, double duration=1, bool autoTriggerData=false, bool sendToDb = false, string targetOnDb = "Inputs_Wasserstrahl2_Status", string name = "Default")
        {
            mProfile = aProfile;
            mSmProfile = new SimulationProfile(aProfile, duration);
            mWave = mProfile.getWave();
            mFreq = mProfile.getFreq();
            mAmpl = mProfile.getAmpl();
            mRate = mProfile.getRate();
            mDuration = duration;
            mSendToDB = sendToDb;
            mTargetOnDB = targetOnDb;
            mRemark = name;
            InitiateData();
            if (autoTriggerData)
            {GenerateData();}

        }
        public GenerateSignalData(SimulationProfile simuProfile, bool autoTriggerData = false, bool sendToDb = false, string targetOnDb = "Inputs_Wasserstrahl1_Status", string name = "Default")
        {
            mSmProfile = simuProfile;
            mProfile = mSmProfile.getSignalProfile();
            mWave = mProfile.getWave();
            mFreq = mProfile.getFreq();
            mAmpl = mProfile.getAmpl();
            mRate = mProfile.getRate();
            mDuration = mSmProfile.getDuration();
            mSendToDB = sendToDb;
            mTargetOnDB = targetOnDb;
            mRemark = name;
            InitiateData();
            if (autoTriggerData)
            { GenerateData(); }
        }
        public GenerateSignalData(WaveForm wave = 0, long freq = 100, double ampl = 5, long rate = 50, double duration = 1, bool autoTriggerData = false, bool sendToDb = false, string targetOnDb = "Inputs_Wasserstrahl2_Status", string name = "Default")
        {
            mProfile = new SignalProfile(wave, freq, ampl, rate);
            mSmProfile = new SimulationProfile(mProfile, duration);
            mWave = wave;
            mFreq = freq;
            mAmpl = ampl;
            mRate = rate;
            mDuration = duration;
            mSendToDB = sendToDb;
            mTargetOnDB = targetOnDb;
            mRemark = name;
            InitiateData();
            if (autoTriggerData)
            { GenerateData(); }
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

        public double getFreq()
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

        public void setTargetOnDB(string target)
        {
            mTargetOnDB = target;
        }

        public bool checkSendToDB()
        {
            return mSendToDB;
        }

        //Hardcore to Desktop first
        public void ExportToJson()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = DateTime.Now.Ticks.ToString();
            //string filePath = desktopPath + @"\GenerateData.json";
            string filePath = desktopPath + "\\"+ fileName +".json";

            var serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            //var serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
            using (StreamWriter writer = File.CreateText(filePath)) { serializer.Serialize(writer, this); }
        }

        public void GenerateData()
        {
            if (!mSmProfile.checkedSmProfValidation())
                return;

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
            if (!mSmProfile.checkedSmProfValidation())
                return;
            mENumber = (long)Math.Round(mRate * mDuration,0);
            mNo = new long[mENumber];
            mTimeStampArray = new long[mENumber];
            mAmplArray = new double[mENumber];

            rand = new Random();
            switch (mWave)
            {
                case WaveForm.Sine:getWaveValue = GetSineValue;break;
                case WaveForm.Sawtooth:getWaveValue = GetSawtoothValue; break;
                case WaveForm.Triangle: getWaveValue = GetTriangleValue; break;
                case WaveForm.Square: getWaveValue = GetSquareValue; break;
                case WaveForm.Random: getWaveValue = GetRandomValue; break;
                default: getWaveValue = GetRandomDigitalValue; break;
            }
        }

        private double GetSineValue(long atTimeInTicks)
        {
            return Math.Round(mAmpl * Math.Sin(2 * Math.PI * mFreq * atTimeInTicks / 1e7), 10);
        }

        private double GetSawtoothValue(long atTimeInTicks)
        {
            return (2 * mAmpl / Math.PI) * Math.Atan(Math.Tan((atTimeInTicks / 1e7) * Math.PI * mFreq));
        }
        private double GetSquareValue(long atTimeInTicks)
        {
            return mAmpl * Math.Sign(Math.Sin(2 * Math.PI * atTimeInTicks / 1e7 * mFreq));
        }
        private double GetTriangleValue(long atTimeInTicks)
        {
            return 2 * mAmpl / Math.PI * Math.Asin(Math.Sin(2 * Math.PI * mFreq * atTimeInTicks / 1e7));
        }

        private Random rand;
        private double GetRandomValue(long atTimeInTicks)
        {
            return (rand.NextDouble() * mAmpl * 2 - mAmpl);
        }
        private double GetRandomDigitalValue(long atTimeInTicks)
        {
            return (double)rand.Next(0, 2);
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
