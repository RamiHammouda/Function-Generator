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
    /// <summary>
    /// Ultimate Class which controll all data to a sensorsignal, including generate signal method..
    /// </summary>
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
        [JsonProperty("Rate", Order = 3)]
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
        public static string mFilePath => Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\AppSetting") + "\\SignalProfiles.json";

        //[JsonProperty("No", Order = 7)]
        private List<long> mNo;
        //[JsonProperty("TimeStamp", Order = 8)]
        private List<long> mTimeStampArray;
        //[JsonProperty("AmplValue", Order = 9)]
        private List<double> mAmplArray;



        [JsonIgnore]

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

        /// <summary>
        /// Get timeStamp array of  generated simulation profile
        /// </summary>
        /// <returns>a long array </returns>
        public List<long> getTimeStamp()
        {
            return mTimeStampArray;
        }
        /// <summary>
        /// Get amlitude array of generated simulation profile
        /// </summary>
        /// <returns>a double array simulate signal</returns>
        public List<double> getAmplData()
        {
            return mAmplArray;
        }
        /// <summary>
        /// Get order of signal as series of number
        /// </summary>
        /// <returns>a long array</returns>
        public List<long> getNo()
        {
            return mNo;
        }
        /// <summary>
        /// Get signal profile from inside of generate signal profile object
        /// </summary>
        /// <returns>signal profile</returns>
        public SignalProfile getSignalProfile()
        {
            return mProfile;
        }
        /// <summary>
        /// Get simulation profile which is inside of generate signal profile object
        /// </summary>
        /// <returns>simulation profile</returns>
        public SimulationProfile getSimulationProfile()
        {
            return mSmProfile;
        }
        /// <summary>
        /// Get the Frequency of sensor signal profile
        /// </summary>
        /// <returns>a double value</returns>
        public double getFreq()
        {
            return mFreq;
        }
        /// <summary>
        /// Get the Sample Rate of sensor signal profile (Sample Rate means how many data points are created per second to reproduce wave form)
        /// </summary>
        /// <returns>a double value</returns>
        public double getRate()
        {
            return mRate;
        }
        /// <summary>
        /// Get the WaveForm of signal profile
        /// </summary>
        /// <returns></returns>
        public WaveForm getWave()
        {
            return mWave;
        }
        /// <summary>
        /// Get the duration of running time for each simulation process. At first purpose it will help GFai fully automate simulation process. But after discussion with GFai, they intend to run simulation process steady, without interrupting anything. So it will be Zero.
        /// </summary>
        /// <returns></returns>
        public double getDuration()
        {
            return mDuration;
        }
        /// <summary>
        /// Get maximal Amplitude of singal profile
        /// </summary>
        /// <returns>a double value</returns>
        public double getAmpl()
        {
            return mAmpl;
        }
        /// <summary>
        /// Set or change target column (fake working as sensor type) on Database. The generated value will be sent to this column on database
        /// </summary>
        /// <param name="target">column on database</param>
        public void setTargetOnDB(string target)
        {
            mTargetOnDB = target;
        }
        /// <summary>
        /// Get current set up target column on database. To which generated value will be sent
        /// </summary>
        /// <returns>a string value as name of column</returns>
        public string getTargetOnDB()
        {
            return mTargetOnDB;
        }
        /// <summary>
        /// Check whether generated values form current signal profile will be sent to database, or not. (or just a draft profile)
        /// </summary>
        /// <returns>boolean value, "true" as yes, I send it, and otherwise</returns>
        public bool checkSendToDB()
        {
            return mSendToDB;
        }
        /// <summary>
        /// As its name describle, this method will export current GenerateSingnalData object to Json
        /// </summary>
        /// <param name="beautiful">format of json with indentation, true as default, yep: beautiful as default :))</param>
        public void ExportToJson(bool beautiful = true)
        {
            //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string basePath = Directory.GetCurrentDirectory() + "\\AppData";
            Directory.CreateDirectory(basePath);
            string fileName = "SingleSignal " + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss ffff");
            string filePath = basePath + "\\" + fileName + ".json";

            JsonSerializer serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            if (!beautiful) serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };

            using (StreamWriter writer = File.CreateText(filePath)) { serializer.Serialize(writer, this); }
        }
        /// <summary>
        /// A static method to export all profiles (from profiles list as input) and saving as json to current directory of program
        /// </summary>
        /// <param name="alist">proiles list as input</param>
        /// <param name="beautiful"> indentation format</param>
        public static void ExportProfilesToJson(List<GenerateSignalData> alist, bool beautiful = true)
        {
            JsonSerializer serializer = new JsonSerializer { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            if (!beautiful) serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
            using (StreamWriter writer = File.CreateText(mFilePath)) { serializer.Serialize(writer, alist); }
        }
        /// <summary>
        /// A static method to import alle saved profile from json in default directory.
        /// </summary>
        /// <returns>A List of GenerateSignalData</returns>
        public static List<GenerateSignalData> ImportProfileFromJson()
        {
            return JsonConvert.DeserializeObject<List<GenerateSignalData>>(File.ReadAllText(mFilePath));
        }

        /// <summary>
        /// Generate signal data from current profile
        /// </summary>
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

        private bool _cancel;
        //for reference only, insert data to db without filling emty columns
        public async void StartWriteToDB()
        {
            mNo.Clear();
            mTimeStampArray.Clear();
            mAmplArray.Clear();

            if (!mSmProfile.checkedSmProfValidation() && mDuration > 0)
                return;
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

        }

        /// <summary>
        /// initiate some data/parameter for later using
        /// </summary>
        private void InitiateData()
        {
            if (!checkValidity())
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
        /// <summary>
        /// Set database to send signal value
        /// </summary>
        /// <param name="myDB"></param>
        public void setMyDB(MyDBEntity myDB)
        {
            mMyDB = myDB;
        }
        /// <summary>
        /// Get amplitude value at specified time based on sine-wave-form(input time as Ticks format:long)
        /// </summary>
        /// <param name="atTimeInTicks"></param>
        /// <returns>a double value</returns>
        private double GetSineValue(long atTimeInTicks)
        {
            return Math.Round(mAmpl * Math.Sin(2 * Math.PI * mFreq * atTimeInTicks / 1e7), 10);
        }
        /// <summary>
        /// Get amplitude value at specified time based on sawtooth-wave-form (input time as Ticks format:long)
        /// </summary>
        /// <param name="atTimeInTicks"></param>
        /// <returns>a double value</returns>
        private double GetSawtoothValue(long atTimeInTicks)
        {
            return (2 * mAmpl / Math.PI) * Math.Atan(Math.Tan((atTimeInTicks / 1e7) * Math.PI * mFreq));
        }
        /// <summary>
        /// Get amplitude value at specified time based on square-wave-form (input time as Ticks format:long)
        /// </summary>
        /// <param name="atTimeInTicks"></param>
        /// <returns>a double value</returns>
        private double GetSquareValue(long atTimeInTicks)
        {
            return mAmpl * Math.Sign(Math.Sin(2 * Math.PI * atTimeInTicks / 1e7 * mFreq));
        }
        /// <summary>
        /// Get amplitude value at specified time based on triangle-wave-form (input time as Ticks format:long)
        /// </summary>
        /// <param name="atTimeInTicks"></param>
        /// <returns>a double value</returns>
        private double GetTriangleValue(long atTimeInTicks)
        {
            return 2 * mAmpl / Math.PI * Math.Asin(Math.Sin(2 * Math.PI * mFreq * atTimeInTicks / 1e7));
        }

        private Random rand;
        /// <summary>
        /// Get random amplitude value at specified time based within max Amplitude (input time as Ticks format:long)
        /// </summary>
        /// <param name="atTimeInTicks"></param>
        /// <returns>a double value</returns>
        private double GetRandomValue(long atTimeInTicks)
        {
            return (rand.NextDouble() * mAmpl * 2 - mAmpl);
        }
        /// <summary>
        /// Get random digital value at specified time (input time as Ticks format:long)
        /// </summary>
        /// <param name="atTimeInTicks"></param>
        /// <returns>a double value 0 or 1</returns>
        private double GetRandomDigitalValue(long atTimeInTicks)
        {
            return (double)rand.Next(0, 2);
        }
        /// <summary>
        /// ToString Profile
        /// </summary>
        /// <returns>a string value</returns>
        public override string ToString()
        {
            return $"{mSmProfile} {mTargetOnDB}";
        }
        /// <summary>
        /// For testing-only method, Print all generated data from a profile with condition: duration must differ Zero
        /// </summary>
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
        /// <summary>
        /// A static method to convert all inputed values in array to a double array
        /// </summary>
        /// <param name="arr"></param>
        /// <returns>a double array</returns>
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
        /// <summary>
        /// a static method for testing only, print out information of all profiles in list
        /// </summary>
        /// <param name="alist"></param>
        public static void PrintProfilesList(List<GenerateSignalData> alist)
        {
            foreach (var item in alist)
                Console.WriteLine(item.ToString());
        }
        /// <summary>
        /// Check Validity of input parameter for current profile 
        /// </summary>
        /// <returns>boolean value, "true" is validated </returns>
        private bool checkValidity()
        {
            if (!((mAmpl > 0) && (mFreq > 0) && (mRate > 0) && (mDuration >= 0)))
            {
                Console.WriteLine("Validity checked Fail");
                return false;
            }
            return true;
        }
    }
}
