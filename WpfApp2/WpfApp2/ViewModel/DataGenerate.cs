using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Model;

namespace WpfApp2.ViewModel
{
    public enum WaveForm
    {
        Sine,
        Sawtooth,
        Random
    }
    class DataGenerate
    {
        private long mDuration;

        private DataSavingModel mDataSaving;

        public DataGenerate(BaseWave waveform,long duration) 
        {
            LinkedList<long> amplList;
            LinkedList<double> tickList;
            for(int i = 0; i < duration*waveform.getTick(); i++)
            {
                //tickList.AddLast()
            }
        }

        public void SaveData()
        {

        }
    }
}
