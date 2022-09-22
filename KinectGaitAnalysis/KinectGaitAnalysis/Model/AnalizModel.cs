using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectGaitAnalysis.Model
{
    [JsonObject]
    [Serializable]
    public class AnalizModel
    {
        public double totalseconds { get; set; }
        public int stepcount { get; set; }
        public int stridecount { get; set; }
        public double totaldistance { get; set; }
        public double speedmin { get; set; }
        public double speedavg { get; set; }
        public double speedmax { get; set; }
        public double steplengthmin { get; set; }
        public double steplengthavg { get; set; }
        public double steplengthmax { get; set; }
        public double stridelengthmin { get; set; }
        public double stridelengthavg { get; set; }
        public double stridelengthmax { get; set; }
        public int cadence { get; set; }
        public double steptimemin { get; set; }
        public double steptimeavg { get; set; }
        public double steptimemax { get; set; }
        public double stridetimemin { get; set; }
        public double stridetimeavg { get; set; }
        public double stridetimemax { get; set; }
        public double footwidthmin { get; set; }
        public double footwidthavg { get; set; }
        public double footwidthmax { get; set; }
        public double kneewidthmin { get; set; }
        public double kneewidthavg { get; set; }
        public double kneewidthmax { get; set; }
        public double onemeterdistance { get; set; }
        public double rightkneeanglemin { get; set; }
        public double rightkneeangleavg { get; set; }
        public double rightkneeanglemax { get; set; }
        public double rightankleanglemin { get; set; }
        public double rightankleangleavg { get; set; }
        public double rightankleanglemax { get; set; }
        public double righthipanglemin { get; set; }
        public double righthipangleavg { get; set; }
        public double righthipanglemax { get; set; }
        public double leftkneeanglemin { get; set; }
        public double leftkneeangleavg { get; set; }
        public double leftkneeanglemax { get; set; }
        public double leftankleanglemin { get; set; }
        public double leftankleangleavg { get; set; }
        public double leftankleanglemax { get; set; }
        public double lefthipanglemin { get; set; }
        public double lefthipangleavg { get; set; }
        public double lefthipanglemax { get; set; }
        public double rightarmanglemin { get; set; }
        public double rightarmangleavg { get; set; }
        public double rightarmanglemax { get; set; }
        public double leftarmanglemin { get; set; }
        public double leftarmangleavg { get; set; }
        public double leftarmanglemax { get; set; }
        public double backanglemin { get; set; }
        public double backangleavg { get; set; }
        public double backanglemax { get; set; }
}    
}
