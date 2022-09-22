using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using KinectGaitAnalysis.Model;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Text;
using System.Web;
using System.Runtime;
using System.Threading.Tasks;


namespace KinectGaitAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;

        CameraMode _mode = CameraMode.Color;

        private const float InferredZPositionClamp = 0.1f;

        public event PropertyChangedEventHandler PropertyChanged;
        private Body _body = null;
        int stepCount = 0;
        double threshold = 0.08;  // metre
        double threshold1 = 1.0;
        double threshold2 = 1.6;
        string topFoot = "left";
        DateTime firstStepTime;
        public int cadence = 0;
        public double aStepTime = 0.0;
        private int angle1;
        private double firstPosition;
        private double lastPosition;
        double totalSeconds;
        int totalMinutes;
        int totalFrames = 0;
        double TotalDistance;
        double speed;
        double ortSpeed;
        private double StrideLengthAvg;
        private double StrideLength;
        private double oneMeterDistance;
        private double StepLength;
        private double strideTime;
        private double LeftHipAngleAvg;
        private double RightArmAngle;
        private double RightArmAngleAvg;
        private double LeftArmAngle;
        private double LeftArmAngleAvg;
        private double HeadAngle;
        private double HeadAngleAvg;
        private double BackAngle;
        private double RightKneeAngle;
        private double RightKneeAngleAvg;
        private double RightAnkleAngle;
        private double RightAnkleAngleAvg;
        private double RightHipAngle;
        private double RightHipAngleAvg;
        private double LeftKneeAngle;
        private double LeftKneeAngleAvg;
        private double LeftAnkleAngle;
        private double LeftAnkleAngleAvg;
        private double LeftHipAngle;
        private double StepLengthAvg;
        private double BackAngleAvg;
        private double footWidthAvg;
        private double kneeWidthAvg;
        private double RightKneeAngleSum;
        private double RightAnkleAngleSum;
        private double HeadAngleSum;
        private double BackAngleSum;
        private double LeftArmAngleSum;
        private double RightHipAngleSum;
        private double LeftHipAngleSum;
        private double RightArmAngleSum;
        private double LeftAnkleAngleSum;
        private double LeftKneeAngleSum;
        private double footWidthSum;
        private double kneeWidthSum;
        private double StepLengthSum;
        private double StrideLengthSum;
        private double StepTimeSum;
        private double StrideTimeSum;
        private double StrideTimeAvg;
        private double StepTimeAvg;
        private double StepTimeMax;
        private double StepTimeMin = double.MaxValue;
        private double StrideTimeMax;
        private double StrideTimeMin = double.MaxValue;
        private double speedMax;
        private double speedMin = double.MaxValue;
        private double StepLengthMax;
        private double StepLengthMin = double.MaxValue;
        private double StrideLengthMax;
        private double StrideLengthMin = double.MaxValue;
        private double footWidthMax;
        private double footWidthMin = double.MaxValue;
        private double kneeWidthMax;
        private double kneeWidthMin = double.MaxValue;
        private double RightKneeAngleMax;
        private double RightKneeAngleMin = double.MaxValue;
        private double RightAnkleAngleMax;
        private double RightAnkleAngleMin = double.MaxValue;
        private double RightHipAngleMax;
        private double RightHipAngleMin = double.MaxValue;
        private double LeftKneeAngleMax;
        private double LeftKneeAngleMin = double.MaxValue;
        private double LeftAnkleAngleMax;
        private double LeftAnkleAngleMin = double.MaxValue;
        private double LeftHipAngleMax;
        private double LeftHipAngleMin = double.MaxValue;
        private double RightArmAngleMax;
        private double RightArmAngleMin = double.MaxValue;
        private double LeftArmAngleMax;
        private double LeftArmAngleMin = double.MaxValue;
        private double HeadAngleMax;
        private double BackAngleMax;
        private double BackAngleMin = double.MaxValue;
        private double HeadAngleMin = double.MaxValue;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {

            var reference = e.FrameReference.AcquireFrame();
            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == CameraMode.Color)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            // Depth
            using (var frame = reference.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == CameraMode.Depth)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            // Infrared
            using (var frame = reference.InfraredFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == CameraMode.Infrared)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                //We get an instance from the data saving class to Text.
                DataSave ds = new DataSave();
                Features features = new Features();



                if (frame != null)
                {
                    canvas.Children.Clear();
                   // _body = frame.Body();



                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body.IsTracked)
                        {
                            // COORDINATE MAPPING
                            foreach (Joint joint in body.Joints.Values)
                            {
                                if (joint.TrackingState == TrackingState.Tracked)
                                {
                                    // 3D space point
                                    CameraSpacePoint jointPosition = joint.Position;

                                    if (jointPosition.Z < 0)
                                    {
                                        jointPosition.Z = InferredZPositionClamp;
                                    }
                                    else if (jointPosition.X < 0)
                                    {
                                        jointPosition.X = InferredZPositionClamp;
                                    }
                                    else if (jointPosition.X < 0)
                                    {
                                        jointPosition.X = InferredZPositionClamp;
                                    }
                                    // 2D space point
                                    Point point = new Point();

                                    if (_mode == CameraMode.Color)
                                    {
                                        ColorSpacePoint colorPoint = _sensor.CoordinateMapper.MapCameraPointToColorSpace(jointPosition);

                                        point.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                                        point.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;

                                    }
                                    else if (_mode == CameraMode.Depth || _mode == CameraMode.Infrared) // Change the Image and Canvas dimensions to 512x424
                                    {
                                        DepthSpacePoint depthPoint = _sensor.CoordinateMapper.MapCameraPointToDepthSpace(jointPosition);

                                        point.X = float.IsInfinity(depthPoint.X) ? 0 : depthPoint.X;
                                        point.Y = float.IsInfinity(depthPoint.Y) ? 0 : depthPoint.Y;
                                    }

                                    Ellipse ellipse = new Ellipse
                                    {
                                        Fill = Brushes.Red,
                                        Width = 30,
                                        Height = 30
                                    };


                                    if (topFoot == "left" && Math.Abs(body.Joints[JointType.AnkleRight].Position.Y) < threshold2 && Math.Abs(body.Joints[JointType.AnkleRight].Position.Y) > threshold1 && body.Joints[JointType.AnkleRight].Position.Y - body.Joints[JointType.AnkleLeft].Position.Y > threshold)
                                    {
                                        topFoot = "right";
                                        stepCount += 1;
                                    }
                                    else if (topFoot == "right" && Math.Abs(body.Joints[JointType.AnkleLeft].Position.Y) < threshold2 && Math.Abs(body.Joints[JointType.AnkleLeft].Position.Y) > threshold1 && body.Joints[JointType.AnkleLeft].Position.Y - body.Joints[JointType.AnkleRight].Position.Y > threshold)
                                    {
                                        topFoot = "left";
                                        stepCount += 1;
                                    }
                                    var StrideCount = stepCount / 2;

                                    double distance = body.Joints[JointType.SpineBase].Position.Z;
                                    double kinectLength(CameraSpacePoint JointType)
                                    {
                                        return Math.Sqrt(
                                            jointPosition.X * jointPosition.X +
                                            jointPosition.Y * jointPosition.Y +
                                            jointPosition.Z * jointPosition.Z
                                        );
                                    }

                                    jointPosition = body.Joints[JointType.SpineBase].Position;
                                    distance = (kinectLength(jointPosition));
                                    distance *= 100; // Distance is converted into centimeters.
                                    distance = Math.Round(distance, 0); //the distance is rounded.

                                    if (stepCount == 0)
                                    {
                                        firstStepTime = DateTime.Now;
                                        firstPosition = distance;
                                    }
                                    else if (stepCount > 0)
                                    {
                                        lastPosition = distance;
                                        TotalDistance = firstPosition - lastPosition;
                                        DateTime stepTime = DateTime.Now;
                                        var result = stepTime.Subtract(firstStepTime);
                                        totalMinutes = (int)Math.Floor(result.TotalMinutes);
                                        Console.WriteLine("Total time: {0}", totalMinutes + "dk");
                                        totalSeconds = (int)Math.Floor(result.TotalSeconds);
                                        Console.WriteLine("Total second : {0}", totalSeconds + "sn");
                                        aStepTime = Convert.ToDouble(totalSeconds) / stepCount;
                                        aStepTime = Math.Round(aStepTime, 2);
                                        if ((totalFrames > 0) && (aStepTime > 0))
                                        {
                                            StepTimeSum += aStepTime;
                                            StepTimeAvg = StepTimeSum / totalFrames;
                                            StepTimeAvg = Math.Round(StepTimeAvg, 2);
                                            // StepTimeMin = StepTimeAvg * 20 /100;
                                            if (aStepTime >= StepTimeMax)
                                            {
                                                StepTimeMax = aStepTime;
                                            }
                                            if ((aStepTime > 0) && (aStepTime <= StepTimeMin))
                                            {
                                                StepTimeMin = aStepTime;
                                            }
                                        }
                                        if (StrideCount > 0 && totalFrames > 0)
                                        {
                                            strideTime = aStepTime * 2;
                                            StrideTimeSum += strideTime;
                                            StrideTimeAvg = StrideTimeSum / totalFrames;
                                            StrideTimeAvg = Math.Round(StrideTimeAvg, 2);
                                            if (strideTime >= StrideTimeMax)
                                            {
                                                StrideTimeMax = strideTime;
                                            }
                                            if (strideTime <= StrideTimeMin)
                                            {
                                                StrideTimeMin = strideTime;
                                            }
                                        }
                                        if (totalSeconds > 0)
                                        {
                                            cadence = (60 * stepCount) / (int)totalSeconds;
                                        }
                                    }
                                    if (speed != 0)
                                    {
                                        totalFrames += 1;
                                    }
                                    if (totalSeconds > 0)
                                    {
                                        speed = (TotalDistance / 100.0) / totalSeconds;
                                        if (speed >= speedMax)
                                        {
                                            speedMax = speed;
                                        }
                                        if (speed > 0 && speed <= speedMin)
                                        {
                                            speedMin = speed;
                                        }
                                        if (speed != 0)
                                        {
                                            ortSpeed += speed;
                                        }
                                        speed = ortSpeed / totalFrames;
                                        speed = Math.Round(speed, 3);

                                        oneMeterDistance = (totalSeconds / (TotalDistance / 100.0));
                                        oneMeterDistance = Math.Round(oneMeterDistance, 3);

                                        StepLength = TotalDistance / stepCount;
                                        if (totalFrames > 0)
                                        {
                                            StepLengthSum += StepLength;
                                            StepLengthAvg = (int)(StepLengthSum / totalFrames);
                                            if (StepLength >= StepLengthMax)
                                            {
                                                StepLengthMax = StepLength;
                                            }
                                            if ((StepLength > 0) && (StepLength <= StepLengthMin))
                                            {
                                                StepLengthMin = StepLength;
                                            }
                                        }
                                        StepLength = Math.Round(StepLength, 3);

                                        if (StrideCount > 0 && totalFrames > 0)
                                        {
                                            StrideLength = StepLength * 2;
                                            StrideLengthSum += StrideLength;
                                            StrideLengthAvg = (int)(StrideLengthSum / totalFrames);
                                            StrideLength = Math.Round(StrideLength, 3);
                                            if (StrideLength >= StrideLengthMax)
                                            {
                                                StrideLengthMax = StrideLength;
                                            }
                                            if ((StrideLength > 0) && (StrideLength <= StrideLengthMin))
                                            {
                                                StrideLengthMin = StrideLength;
                                            }
                                        }
                                    }

                                    double footWidth = (Math.Sqrt(
                                    Math.Pow((body.Joints[JointType.FootRight].Position.X - body.Joints[JointType.FootLeft].Position.X), 2) +
                                    Math.Pow((body.Joints[JointType.FootRight].Position.Y - body.Joints[JointType.FootLeft].Position.Y), 2) +
                                    Math.Pow((body.Joints[JointType.FootRight].Position.Z - body.Joints[JointType.FootLeft].Position.Z), 2)
                                    ));
                                    footWidth = footWidth * 100;
                                    if (totalFrames > 0)
                                    {
                                        footWidthSum += footWidth;
                                        footWidthAvg = (int)(footWidthSum / totalFrames);
                                        if (footWidth >= footWidthMax)
                                        {
                                            footWidthMax = footWidth;
                                            footWidthMax = Math.Round(footWidthMax, 0);
                                        }
                                        if ((footWidth > 0) && (footWidth <= footWidthMin))
                                        {
                                            footWidthMin = footWidth;
                                            footWidthMin = Math.Round(footWidthMin, 0);
                                        }
                                    }
                                    double kneeWidth = (Math.Sqrt(
                                    Math.Pow((body.Joints[JointType.KneeLeft].Position.X - body.Joints[JointType.KneeRight].Position.X), 2) +
                                    Math.Pow((body.Joints[JointType.KneeLeft].Position.Y - body.Joints[JointType.KneeRight].Position.Y), 2) +
                                    Math.Pow((body.Joints[JointType.KneeLeft].Position.Z - body.Joints[JointType.KneeRight].Position.Z), 2)
                                    ));
                                    kneeWidth = kneeWidth * 100;
                                    if (totalFrames > 0)
                                    {
                                        kneeWidthSum += kneeWidth;
                                        kneeWidthAvg = (int)(kneeWidthSum / totalFrames);
                                        if (kneeWidth >= kneeWidthMax)
                                        {
                                            kneeWidthMax = kneeWidth;
                                            kneeWidthMax = Math.Round(kneeWidthMax, 0);
                                        }
                                        if ((kneeWidth > 0) && (kneeWidth <= kneeWidthMin))
                                        {
                                            kneeWidthMin = kneeWidth;
                                            kneeWidthMin = Math.Round(kneeWidthMin, 0);
                                        }
                                    }

                                    //Angless
                                    double length(Joint j1, Joint j2)
                                    {
                                        double Length = Math.Sqrt((Math.Pow(j1.Position.X - j2.Position.X, 2)) + (Math.Pow(j1.Position.Y - j2.Position.Y, 2)));
                                        return Length;
                                    }
                                    double angle(Joint j1, Joint j2, Joint j3)
                                    {
                                        double l1 = length(j1, j2);
                                        double l2 = length(j2, j3);
                                        double l3 = length(j1, j3);
                                        double cos = (double)(Math.Pow(l1, 2) + Math.Pow(l2, 2) - Math.Pow(l3, 2)) / (double)(2 * l1 * l2);
                                        angle1 = (int)((180 / Math.PI) * (Math.Acos(cos)));
                                        return angle1;
                                    }
                                    RightKneeAngle = angle(body.Joints[JointType.AnkleRight], body.Joints[JointType.KneeRight], body.Joints[JointType.HipRight]);
                                    if (totalFrames > 0)
                                    {
                                        RightKneeAngleSum += RightKneeAngle;
                                        RightKneeAngleAvg = (int)(RightKneeAngleSum / totalFrames);
                                        if (RightKneeAngle >= RightKneeAngleMax)
                                        {
                                            RightKneeAngleMax = RightKneeAngle;
                                        }
                                        if ((RightKneeAngle > 0) && (RightKneeAngle <= RightKneeAngleMin))
                                        {
                                            RightKneeAngleMin = RightKneeAngle;
                                        }
                                    }

                                    RightAnkleAngle = angle(body.Joints[JointType.FootRight], body.Joints[JointType.AnkleRight], body.Joints[JointType.KneeRight]);
                                    if (totalFrames > 0)
                                    {
                                        RightAnkleAngleSum += RightAnkleAngle;
                                        RightAnkleAngleAvg = (int)(RightAnkleAngleSum / totalFrames);
                                        if (RightAnkleAngle >= RightAnkleAngleMax)
                                        {
                                            RightAnkleAngleMax = RightAnkleAngle;
                                        }
                                        if ((RightAnkleAngle > 0) && (RightAnkleAngle <= RightAnkleAngleMin))
                                        {
                                            RightAnkleAngleMin = RightAnkleAngle;
                                        }
                                    }

                                    RightHipAngle = angle(body.Joints[JointType.KneeRight], body.Joints[JointType.HipRight], body.Joints[JointType.SpineBase]);
                                    if (totalFrames > 0)
                                    {
                                        RightHipAngleSum += RightHipAngle;
                                        RightHipAngleAvg = (int)(RightHipAngleSum / totalFrames);
                                        if (RightHipAngle >= RightHipAngleMax)
                                        {
                                            RightHipAngleMax = RightHipAngle;
                                        }
                                        if ((RightHipAngle > 0) && (RightHipAngle <= RightHipAngleMin))
                                        {
                                            RightHipAngleMin = RightHipAngle;
                                        }
                                    }

                                    LeftKneeAngle = angle(body.Joints[JointType.AnkleLeft], body.Joints[JointType.KneeLeft], body.Joints[JointType.HipLeft]);
                                    if (totalFrames > 0)
                                    {
                                        LeftKneeAngleSum += LeftKneeAngle;
                                        LeftKneeAngleAvg = (int)(LeftKneeAngleSum / totalFrames);
                                        if (LeftKneeAngle >= LeftKneeAngleMax)
                                        {
                                            LeftKneeAngleMax = LeftKneeAngle;
                                        }
                                        if ((LeftKneeAngle > 0) && (LeftKneeAngle <= LeftKneeAngleMin))
                                        {
                                            LeftKneeAngleMin = LeftKneeAngle;
                                        }
                                    }

                                    LeftAnkleAngle = angle(body.Joints[JointType.FootLeft], body.Joints[JointType.AnkleLeft], body.Joints[JointType.KneeLeft]);
                                    if (totalFrames > 0)
                                    {
                                        LeftAnkleAngleSum += LeftAnkleAngle;
                                        LeftAnkleAngleAvg = (int)(LeftAnkleAngleSum / totalFrames);
                                        if (LeftAnkleAngle >= LeftAnkleAngleMax)
                                        {
                                            LeftAnkleAngleMax = LeftAnkleAngle;
                                        }
                                        if ((LeftAnkleAngle > 0) && (LeftAnkleAngle <= LeftAnkleAngleMin))
                                        {
                                            LeftAnkleAngleMin = LeftAnkleAngle;
                                        }
                                    }

                                    LeftHipAngle = angle(body.Joints[JointType.KneeLeft], body.Joints[JointType.HipLeft], body.Joints[JointType.SpineBase]);
                                    if (totalFrames > 0)
                                    {
                                        LeftHipAngleSum += LeftHipAngle;
                                        LeftHipAngleAvg = (int)(LeftHipAngleSum / totalFrames);
                                        if (LeftHipAngle >= LeftHipAngleMax)
                                        {
                                            LeftHipAngleMax = LeftHipAngle;
                                        }
                                        if ((LeftHipAngle > 0) && (LeftHipAngle <= LeftHipAngleMin))
                                        {
                                            LeftHipAngleMin = LeftHipAngle;
                                        }
                                    }

                                    RightArmAngle = angle(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight]);
                                    if (totalFrames > 0)
                                    {
                                        RightArmAngleSum += RightArmAngle;
                                        RightArmAngleAvg = (int)(RightArmAngleSum / totalFrames);
                                        if (RightArmAngle >= RightArmAngleMax)
                                        {
                                            RightArmAngleMax = RightArmAngle;
                                        }
                                        if ((RightArmAngle > 0) && (RightArmAngle <= RightArmAngleMin))
                                        {
                                            RightArmAngleMin = RightArmAngle;
                                        }
                                    }

                                    LeftArmAngle = angle(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft]);
                                    if (totalFrames > 0)
                                    {
                                        LeftArmAngleSum += LeftArmAngle;
                                        LeftArmAngleAvg = (int)(LeftArmAngleSum / totalFrames);
                                        if (LeftArmAngle >= LeftArmAngleMax)
                                        {
                                            LeftArmAngleMax = LeftArmAngle;
                                        }
                                        if ((LeftArmAngle > 0) && (LeftArmAngle <= LeftArmAngleMin))
                                        {
                                            LeftArmAngleMin = LeftArmAngle;
                                        }
                                    }

                                    HeadAngle = angle(body.Joints[JointType.Head], body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder]);
                                    if (totalFrames > 0)
                                    {
                                        HeadAngleSum += HeadAngle;
                                        HeadAngleAvg = (int)(HeadAngleSum / totalFrames);
                                        if (HeadAngle >= HeadAngleMax)
                                        {
                                            HeadAngleMax = HeadAngle;
                                        }
                                        if ((HeadAngle > 0) && (HeadAngle <= HeadAngleMin))
                                        {
                                            HeadAngleMin = HeadAngle;
                                        }
                                    }

                                    BackAngle = angle(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight]);
                                    if (totalFrames > 0)
                                    {
                                        BackAngleSum += BackAngle;
                                        BackAngleAvg = (int)(BackAngleSum / totalFrames);
                                        if (BackAngle >= BackAngleMax)
                                        {
                                            BackAngleMax = BackAngle;
                                        }
                                        if ((BackAngle > 0) && (BackAngle <= BackAngleMin))
                                        {
                                            BackAngleMin = BackAngle;
                                        }
                                    }
                                    

                                    //Karar Agacı yöntemi kullanılacaksa messageBox kodları yorum satırından kaldırılmalı.
                                    if (totalSeconds > 5)
                                    {
                                        if ((footWidthAvg >= 25 && footWidthAvg <= 32) && (kneeWidthAvg >= 14 && kneeWidthAvg < 25) && speed > 0.700 && StepLengthAvg >= 70 && (StrideLengthAvg >= 130 && StrideLengthAvg <= 195) && StepTimeAvg >= 0.78 && cadence >= 30)
                                        {
                                            string message = "Healthy";
                                            string title = "Gait Analysis";
                                            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

                                            //if (result == MessageBoxResult.OK)
                                            //{
                                            //    Process.GetCurrentProcess().Kill();
                                            //}

                                        }
                                        else
                                        {
                                            if (speed < 0.70 && (RightArmAngleAvg <= 50 && RightKneeAngleAvg <= 174) || (LeftArmAngleAvg <= 50 && LeftKneeAngleAvg <= 174))
                                            {
                                                string message = "Disease Found: Stroke";
                                                string title = "Gait Analysis";
                                                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

                                                //if (result == MessageBoxResult.OK)
                                                //{
                                                //    Process.GetCurrentProcess().Kill();
                                                //}

                                            }
                                            else if (StepLengthAvg < 60 && RightKneeAngleAvg < 170 && LeftKneeAngleAvg < 170 && RightArmAngleAvg < 160 && LeftArmAngleAvg < 160 && BackAngleAvg > 150 && oneMeterDistance > 3.0)
                                            {
                                                string message = "Disease Found: Parkinson's";
                                                string title = "Gait Analysis";
                                                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

                                                //if (result == MessageBoxResult.OK)
                                                //{
                                                //    Process.GetCurrentProcess().Kill();
                                                //}

                                            }

                                            else if (footWidthAvg >= 45 && kneeWidthAvg >= 30 && speed < 0.450 && StepTimeAvg > 1.00 && (RightHipAngleAvg >= 90 && LeftHipAngleAvg >= 90) && BackAngleAvg > 150)
                                            {
                                                string message = "Disease Found: Cerebellar Ataxia";
                                                string title = "Gait Analysis";
                                                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

                                                //if (result == MessageBoxResult.OK)
                                                //{
                                                //    Process.GetCurrentProcess().Kill();
                                                //}

                                            }
                                            else if (StepLengthAvg < 70 && (RightAnkleAngleAvg > 100 && RightAnkleAngleAvg < 140) && (LeftAnkleAngleAvg > 100 && LeftAnkleAngleAvg < 140) && (oneMeterDistance > 1.600 && oneMeterDistance < 2.500))
                                            {
                                                string message = "Disease Found: Toe Walking";
                                                string title = "Gait Analysis";
                                                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

                                                //if (result == MessageBoxResult.OK)
                                                //{
                                                //    Process.GetCurrentProcess().Kill();
                                                //}

                                            }

                                            else if ((footWidthAvg >= 35 && kneeWidthAvg < 18) || (kneeWidthAvg >= 25 && footWidthAvg < 25))
                                            {
                                                string message = "Disease Found: Bowleg";
                                                string title = "Gait Analysis";
                                                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

                                                //if (result == MessageBoxResult.OK)
                                                //{
                                                //    Process.GetCurrentProcess().Kill();
                                                //}

                                            }
                                            else
                                            {
                                                string message = "There is an unidentified problem in your walk \nThe clinical environment is necessary for further examination of your disease.";
                                                string title = "Gait Analysis";
                                                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

                                                //if (result == MessageBoxResult.OK)
                                                //{
                                                //    Process.GetCurrentProcess().Kill();
                                                //}
                                            }
                                        }
                                    }
                                    //Karar agacı yöntemi kullnaılacaksa bu alan yorum satırına alınmalıdır.
                                    if (totalSeconds < 5)
                                    {
                                        return;
                                    }
                                    var model = new AnalizModel();     //sending data to json model
                                    model.totalseconds = totalSeconds;
                                    model.stepcount = stepCount;
                                    model.stridecount = StrideCount;
                                    model.totaldistance = TotalDistance;
                                    model.speedmin = speedMin;
                                    model.speedavg = speed;
                                    model.speedmax = speedMax;
                                    model.steplengthmin = StepLengthMin;
                                    model.steplengthavg = StepLengthAvg;
                                    model.steplengthmax = StepLengthMax;
                                    model.stridelengthmin = StrideLengthMin;
                                    model.stridelengthavg = StrideLengthAvg;
                                    model.stridelengthmax = StrideLengthMax;
                                    model.cadence = cadence;
                                    model.steptimemin = StepTimeMin;
                                    model.steptimeavg = StepTimeAvg;
                                    model.steptimemax = StepTimeMax;
                                    model.stridetimemin = StrideTimeMin;
                                    model.stridetimeavg = StrideTimeAvg;
                                    model.stridetimemax = StrideTimeMax;
                                    model.footwidthmin = footWidthMin;
                                    model.footwidthavg = footWidthAvg;
                                    model.footwidthmax = footWidthMax;
                                    model.kneewidthmin = kneeWidthMin;
                                    model.kneewidthavg = kneeWidthAvg;
                                    model.kneewidthmax = kneeWidthMax;
                                    model.onemeterdistance = oneMeterDistance;
                                    model.rightkneeanglemin = RightKneeAngleMin;
                                    model.rightkneeangleavg = RightKneeAngleAvg;
                                    model.rightkneeanglemax = RightKneeAngleMax;
                                    model.rightankleanglemin = RightAnkleAngleMin;
                                    model.rightankleangleavg = RightAnkleAngleAvg;
                                    model.rightankleanglemax = RightAnkleAngleMax;
                                    model.righthipanglemin = RightHipAngleMin;
                                    model.righthipangleavg = RightHipAngleAvg;
                                    model.righthipanglemax = RightHipAngleMax;
                                    model.leftkneeanglemin = LeftKneeAngleMin;
                                    model.leftkneeangleavg = LeftKneeAngleAvg;
                                    model.leftkneeanglemax = LeftKneeAngleMax;
                                    model.leftankleanglemin = LeftAnkleAngleMin;
                                    model.leftankleangleavg = LeftAnkleAngleAvg;
                                    model.leftankleanglemax = LeftAnkleAngleMax;
                                    model.lefthipanglemin = LeftHipAngleMin;
                                    model.lefthipangleavg = LeftHipAngleAvg;
                                    model.lefthipanglemax = LeftHipAngleMax;
                                    model.rightarmanglemin = RightArmAngleMin;
                                    model.rightarmangleavg = RightArmAngleAvg;
                                    model.rightarmanglemax = RightArmAngleMax;
                                    model.leftarmanglemin = LeftArmAngleMin;
                                    model.leftarmangleavg = LeftArmAngleAvg;
                                    model.leftarmanglemax = LeftArmAngleMax;
                                    model.backanglemin = BackAngleMin;
                                    model.backangleavg = BackAngleAvg;
                                    model.backanglemax = BackAngleMax;

                                    // json model
                                    using (var client = new HttpClient())
                                    {
                                        var uri = new Uri("http://127.0.0.1:5000");
                                        var json = new JavaScriptSerializer().Serialize(model);
                                        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                                        var response = client.PostAsync(uri, stringContent).Result;
                                        Console.WriteLine(response);
                                        Process.Start("http://127.0.0.1:5000");
                                        Process.GetCurrentProcess().Kill();
                                    }

                                    //Incoming X Y Z data is saved in the text.

                                    ds.dataSave(joint.JointType.ToString(), jointPosition.X, jointPosition.Y, jointPosition.Z, joint.JointType.ToString());
                                    features.dataSave(Convert.ToInt16(totalMinutes), Convert.ToInt16(totalSeconds), stepCount, StrideCount, TotalDistance, speedMin, speed, speedMax, StepLengthMin, StepLengthAvg, StepLengthMax, StrideLengthMin, StrideLengthAvg, StrideLengthMax, cadence, StepTimeMin, StepTimeAvg, StepTimeMax, StrideTimeMin, StrideTimeAvg, StrideTimeMax, footWidthMin, footWidthAvg, footWidthMax, kneeWidthMin, kneeWidthAvg, kneeWidthMax, oneMeterDistance, RightKneeAngleMin, RightKneeAngleAvg, RightKneeAngleMax, RightAnkleAngleMin, RightAnkleAngleAvg, RightAnkleAngleMax, RightHipAngleMin, RightHipAngleAvg, RightHipAngleMax, LeftKneeAngleMin, LeftKneeAngleAvg, LeftKneeAngleMax, LeftAnkleAngleMin, LeftAnkleAngleAvg, LeftAnkleAngleMax, LeftHipAngleMin, LeftHipAngleAvg, LeftHipAngleMax, RightArmAngleMin, RightArmAngleAvg, RightArmAngleMax, LeftArmAngleMin, LeftArmAngleAvg, LeftArmAngleMax, BackAngleMin, BackAngleAvg, BackAngleMax);

                                    Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
                                    Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

                                }
                            }
                        }
                    }
                }
            }
        }
    }
    enum CameraMode
    {
        Color,
        Depth,
        Infrared
    }
}
