using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfApp2.Model;

namespace WpfApp2.ViewModel
{
    public class GenerateSignalData
    {
        [JsonIgnore]
        public SignalProfile mProfile;
        [JsonIgnore]
        private SimulationProfile mSmProfile;

        [JsonProperty("Wave", Order = 0)]
        public WaveForm mWave { get; set; }
        [JsonProperty("Freq", Order = 1)]
        public double mFreq { get; set; }
        [JsonProperty("Ampl", Order = 2)]
        public double mAmpl { get; set; }
        [JsonProperty("SampleRate", Order = 3)]
        public double mRate { get; set; }
        [JsonProperty("Duration", Order = 4)]
        public double mDuration { get; set; }

        [JsonProperty("SendToDB", Order = 5)]
        public bool mSendToDB { get; set; }

        [JsonProperty("TargetOnDB", Order = 6)]
        public string mTargetOnDB { get; set; }

        [JsonProperty("Remark", Order = 7)]
        public string mRemark { get; set; }
        [JsonIgnore]
        public static string mFilePath => Directory.GetCurrentDirectory() + "\\AppSetting\\SignalProfiles.json";

        //[JsonProperty("No", Order = 7)]
        private List<long> mNo;
        //[JsonProperty("TimeStamp", Order = 8)]
        private List<long> mTimeStampArray;
        //[JsonProperty("AmplValue", Order = 9)]
        private List<double> mAmplArray;



        [JsonIgnore]
        public bool WritingIsFinished = false;

        //For quick access and control;
        private long aTimeStep;
        private long mENumber;

        public delegate double GetWaveValue(long inputTime);
        [JsonIgnore]
        public GetWaveValue getWaveValue;
        

        private MyDBEntity mMyDB;
        //Either parameterless Contructor or Keyword JsonContructor for Json Deserialize
        [JsonConstructor]
        public GenerateSignalData(WaveForm wave = 0, double freq = 0.1, double ampl = 7, double rate = 2, double duration = 0, bool sendToDb = false, string targetOnDb = "Inputs_TestVarLReal", string name = "Default")
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
        }

        public GenerateSignalData(SignalProfile aProfile, double duration = 0, bool autoTriggerData = false, bool sendToDb = false, string targetOnDb = "Inputs_TestVarLReal", string name = "Default", MyDBEntity myDB = null)
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
            mMyDB = myDB;
            InitiateData();
            if (autoTriggerData)
            { GenerateData(); }

        }
        public GenerateSignalData(SimulationProfile simuProfile, bool autoTriggerData = false, bool sendToDb = false, string targetOnDb = "Inputs_TestVarLReal", string name = "Default", MyDBEntity myDB = null)
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
            mMyDB = myDB;
            InitiateData();
            if (autoTriggerData)
            { GenerateData(); }
        }
        

        public List<long> getTimeStamp()
        {
            return mTimeStampArray;
        }
        public List<double> getAmplData()
        {
            return mAmplArray;
        }

        public List<long> getNo()
        {
            return mNo;
        }

        public SignalProfile getSignalProfile()
        {
            return mProfile;
        }

        public SimulationProfile getSimulationProfile()
        {
            return mSmProfile;
        }

        public double getFreq()
        {
            return mFreq;
        }

        public double getRate()
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
        public string getTargetOnDB()
        {
            return mTargetOnDB;
        }

        public bool checkSendToDB()
        {
            return mSendToDB;
        }

        public void ExportToJson(bool beautiful = true)
        {
            //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string basePath = Directory.GetCurrentDirectory() + "\\AppData";
            Directory.CreateDirectory(basePath);
            string fileName = "SingleSignal " + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss ffff");
            string filePath = basePath + "\\" + fileName + ".json";

            JsonSerializer serializer;
            if (beautiful)
                serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            else
                serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };

            using (StreamWriter writer = File.CreateText(filePath)) { serializer.Serialize(writer, this); }
        }

        public static void ExportProfilesToJson(List<GenerateSignalData> alist, bool beautiful = true)
        {
            //string filePath = Directory.GetCurrentDirectory() + "\\AppSetting\\SignalProfiles.json";
            JsonSerializer serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            if (!beautiful)
                serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };

            using (StreamWriter writer = File.CreateText(mFilePath)) { serializer.Serialize(writer, alist); }
        }

        public static List<GenerateSignalData> ImportProfileFromJson()
        {
            return JsonConvert.DeserializeObject<List<GenerateSignalData>>(File.ReadAllText(mFilePath));
        }

        //for reference only: generate Data und save to Json
        public void GenerateData()
        {
            if (!mSmProfile.checkedSmProfValidation())
                return;

            // A Time-Step in Ticks
            aTimeStep = (long)Math.Round(1e7 / mRate, 0);
            long start = DateTime.Now.Ticks;

            for (long i = 0; i < mENumber; i++)
            {
                mNo.Add(i);
                mTimeStampArray.Add(start + i * aTimeStep);
                mAmplArray.Add(getWaveValue(i * aTimeStep));
            };
        }


        //private CancellationTokenSource _canceller;
        private bool _cancel;
        //for reference only, insert data to db without filling emty columns
        public async void StartWriteToDB()
        {
            mNo.Clear();
            mTimeStampArray.Clear();
            mAmplArray.Clear();

            if (!mSmProfile.checkedSmProfValidation() && mDuration > 0)
                return;
            //_canceller = new CancellationTokenSource();
            _cancel = false;
            int waitingTime = (int)(1000 / mRate);
            mMyDB.InsertInTargetColumn(mTargetOnDB);
            long now;
            if (mDuration == 0)
            {

                await Task.Run(() =>
                {
                    int i = 0;
                    while (!_cancel)
                    //while (!_canceller.Token.IsCancellationRequested)
                    {
                        now = DateTime.Now.Ticks;
                        mNo.Add(i);
                        mTimeStampArray.Add(now);
                        mAmplArray.Add(Math.Round(getWaveValue(mTimeStampArray[i]), 3));
                        mMyDB.Insert(mAmplArray[i]);
                        Console.WriteLine("{0}  -  {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), mAmplArray[i]);  //For Debug Only
                        i++;
                        Thread.Sleep(waitingTime);
                    }
                });

            }
            else
            {
                await Task.Run(() =>
                {
                    for (int i = 0; i < mENumber; i++)
                    {
                        //if (_canceller.Token.IsCancellationRequested)
                        if (_cancel)
                            return;
                        now = DateTime.Now.Ticks;
                        mNo.Add(i);
                        mTimeStampArray.Add(now);
                        mAmplArray.Add(getWaveValue(mTimeStampArray[i]));
                        mMyDB.Insert(mAmplArray[i]);
                        //Console.WriteLine("Timestamp:{0}  Value:{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), mAmplArray[i]);  //For Debug Only
                        Thread.Sleep(waitingTime);
                    };
                });
            }
            Console.WriteLine("Finished Writing");
            //_canceller.Dispose();

        }

        public void Stop()
        {
            //try
            //{
            //    _canceller.Cancel();
            //}
            //catch (ObjectDisposedException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            _cancel = true;
        }

        private void InitiateData()
        {
            if (!mSmProfile.checkedSmProfValidation())
                return;
            mENumber = (long)Math.Round(mRate * mDuration, 0);
            mNo = new List<long>();
            mTimeStampArray = new List<long>();
            mAmplArray = new List<double>();

            rand = new Random();
            switch (mWave)
            {
                case WaveForm.Sine: getWaveValue = GetSineValue; break;
                case WaveForm.Sawtooth: getWaveValue = GetSawtoothValue; break;
                case WaveForm.Triangle: getWaveValue = GetTriangleValue; break;
                case WaveForm.Square: getWaveValue = GetSquareValue; break;
                case WaveForm.Random: getWaveValue = GetRandomValue; break;
                default: getWaveValue = GetRandomDigitalValue; break;
            }
        }

        public void setMyDB(MyDBEntity myDB)
        {
            mMyDB = myDB;
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

        public override string ToString()
        {
            return $"{mSmProfile} {mTargetOnDB}";
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
            for (long i = 0; i < length; i++)
            {
                result[i] = (double)arr[i];
            }
            return result;
        }

        public static void PrintProfilesList(List<GenerateSignalData> alist)
        {
            foreach (var item in alist)
                Console.WriteLine(item.ToString());
        }

    }
}
