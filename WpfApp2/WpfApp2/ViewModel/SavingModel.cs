using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.ViewModel
{
    class DataSavingModel
    {
        private LinkedList<long> mTickTime;
        private LinkedList<double> mAmplTick;

        public DataSavingModel(LinkedList<long> ticktime, LinkedList<double> ampltick) 
        {
            mTickTime = ticktime;
            mAmplTick = ampltick;
        }
        public DataSavingModel(long ticktime, double ampltick)
        {
            mTickTime.AddLast(ticktime);
            mAmplTick.AddLast(ampltick);
        }
        public LinkedList<long> getTimeList()
        {
            return mTickTime;
        }
        public LinkedList<double> getAmplList()
        {
            return mAmplTick;
        }
    }
}
