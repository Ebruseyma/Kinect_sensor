using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KinectGaitAnalysis.Model;

namespace KinectGaitAnalysis
{
    public class Features

    {
        public Features()
        {
            CreateFolder();

        }
        public List<AnalizModel> MyData { get; set; }
        private void CreateFolder()
        {
            //Proje dizininde Data adında klasör oluşturur.
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\");
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Features");
        }
        public List<AnalizModel> getAll()
        {
            return MyData;
        }
        public void dataSave(short totalMinutes, short totalSeconds, int stepCount, int StrideCount, double TotalDistance, double speedMin, double speed, double speedMax, double StepLengthMin, double StepLengthAvg, double StepLengthMax, double StrideLengthMin, double StrideLengthAvg, double StrideLengthMax, int cadence, double StepTimeMin, double StepTimeAvg, double StepTimeMax, double StrideTimeMin, double StrideTimeAvg, double StrideTimeMax, double footWidthMin, double footWidthAvg, double footWidthMax, double kneeWidthMin, double kneeWidthAvg, double kneeWidthMax, double oneMeterDistance, double RightKneeAngleMin, double RightKneeAngleAvg, double RightKneeAngleMax, double RightAnkleAngleMin, double RightAnkleAngleAvg, double RightAnkleAngleMax, double RightHipAngleMin, double RightHipAngleAvg, double RightHipAngleMax, double LeftKneeAngleMin, double LeftKneeAngleAvg, double LeftKneeAngleMax, double LeftAnkleAngleMin, double LeftAnkleAngleAvg, double LeftAnkleAngleMax, double LeftHipAngleMin, double LeftHipAngleAvg, double LeftHipAngleMax, double RightArmAngleMin, double RightArmAngleAvg, double RightArmAngleMax, double LeftArmAngleMin, double LeftArmAngleAvg, double LeftArmAngleMax, double BackAngleMin, double BackAngleAvg, double BackAngleMax)
        {
            try
            {
                string _data = "totalMinutes" + "   " + totalMinutes + "   " + "  totalSeconds" + "   " + totalSeconds + "   " + "   stepCount" + "   " + stepCount + "   " + "   StrideCount" + "   " + StrideCount + "   " + "   TotalDistance" + "   " + TotalDistance + "   " + "   speedMin" + "   " + speedMin + "   " + "   " + "   speedAvg" + "   " + speed + "   " + "   speedMax" + "   " + speedMax + "   "
                    + "   StepLengthMin" + "   " + StepLengthMin + "   " + "   StepLengthAvg" + "   " + StepLengthAvg + "   " + "   StepLengthMax" + "   " + StepLengthMax + "   " + "   StrideLengthMin" + "   " + StrideLengthMin + "   " + "   StrideLengthAvg" + "   " + StrideLengthAvg + "   StrideLengthMax" + "   " + StrideLengthMax + "   " + "   cadence" + "   " + cadence + "   " + "   StepTimeMin" + "   " + StepTimeMin + "   " + "   StepTimeAvg" + "   " + StepTimeAvg + "   " + "   " + "   StepTimeMax" + "   " + StepTimeMax + "   " + "   strideTimeMin" + "   " + StrideTimeMin + "   " + "   strideTimeAvg" + "   " + StrideTimeAvg + "   " + "   " + "   strideTimeMax" + "   " + StrideTimeMax + "   " + " footWidthMin " + "   " + footWidthMin + "   " + "   footWidthAvg" + "   " + footWidthAvg + "   " + " footWidthMax " + "   " + footWidthMax + "   " + " kneeWidthMin " + "   " + kneeWidthMin + "   " + " kneeWidthAvg " + "   " + kneeWidthAvg + "   " + " kneeWidthMax" + "   " + kneeWidthMax + "   " + " oneMeterDistance" + "   " + oneMeterDistance + "   " + " RightKneeAngleMin" + "   " + RightKneeAngleMin + "   " + " RightKneeAngleAvg" + "   " + RightKneeAngleAvg + "   "
                  + " RightKneengleMax" + "   " + RightKneeAngleMax + "   " + " RightAnkleAngleMin" + "   " + RightAnkleAngleMin + "   " + " RightAnkleAngleAvg" + "   " + RightAnkleAngleAvg + " RightAnkleAngleMax" + "   " + RightAnkleAngleMax + "   " + " RightHipAngleMin" + "   " + RightHipAngleMin + "   " + " RightHipAngleAvg" + "   " + RightHipAngleAvg + "   " + " RightHipAngleMax" + "   " + RightHipAngleMax + "   "
                   + " LeftKneeAngleMin" + "   " + LeftKneeAngleMin + "   " + " LeftKneeAngleAvg" + "   " + LeftKneeAngleAvg + "   " + " LeftKneeAngleMax" + "   " + LeftKneeAngleMax + "   " + " LeftAnkleAngleMin" + "   " + LeftAnkleAngleMin + "   " + "LeftAnkleAngleAvg" + "   " + LeftAnkleAngleAvg + "   " + " LeftAnkleAngleMax" + "   " + LeftAnkleAngleMax + "   " + " LeftHipAngleMin" + "   " + LeftHipAngleMin + "   " + " LeftHipAngleAvg" + "   "
                    + LeftHipAngleAvg + "   " + " LeftHipAngleMax" + "   " + LeftHipAngleMax + "   " + " RightArmAngleMin" + "   " + RightArmAngleMin + "   " + " RightArmAngleAvg" + "   " + RightArmAngleAvg + "   " + " RightArmangleMax" + "   " + RightArmAngleMax + "   " + " LeftArmAngleMin" + "   " + LeftArmAngleMin + "   " + " LeftArmAngleAvg" + "   " + LeftArmAngleAvg + "   " + " LeftArmAngleMax" + "   " + LeftArmAngleMax + "   " + " BackAngleMin" + "   " + BackAngleMin + "   " + " BackAngleAvg" + "   " + BackAngleAvg + " " + " BackAngleMax" + "   " + BackAngleMax + "   ";

                string dosyaAdi;
                string yolEx;
                dosyaAdi = "Features.csv";
                StreamWriter sw = null;
                yolEx = System.IO.Directory.GetCurrentDirectory() + @"\Features\\" + dosyaAdi;
                sw = new StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\Features\\" + dosyaAdi, true, Encoding.Default);
                //Veriyi texte yazar.
                sw.WriteLine(_data);
                sw.Close();
            }
            catch (Exception hata)
            {
            }
        }
    }
}
