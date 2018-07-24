﻿using STROOP.Forms;
using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class TestUtilities
    {

        public static void TestSomething6()
        {
            List<List<int>> dustFrameLists = TtcMain.FindIdealPendulumManipulation(0x8033E788, 5);
            List<string> outputList = dustFrameLists.ConvertAll(dustFrameList => "[" + String.Join(", ", dustFrameList) + "]");
            string output = String.Join("\r\n", outputList);
            Config.Print(output);
        }

        public static void TestSomething()
        {
            MoveBoxes(false);
        }

        public static void MoveBoxes(bool upwards)
        {
            List<double> marioPositions = upwards ? marioPositions1 : marioPositions2;
            int yDiff = upwards ? 78 : -100;

            List<double> qSteps = new List<double>();
            for (int i = 0; i < marioPositions.Count - 1; i++)
            {
                double x1 = marioPositions[i];
                double x2 = marioPositions[i + 1];
                double diff = x2 - x1;
                qSteps.Add(x1 + diff * 1 / 4.0);
                qSteps.Add(x1 + diff * 2 / 4.0);
                qSteps.Add(x1 + diff * 3 / 4.0);
                qSteps.Add(x1 + diff * 4 / 4.0);
            }
            List<int> qStepsTruncated = qSteps.ConvertAll(qStep => (int)Math.Truncate(qStep));
            List<ObjectDataModel> objects = Config.ObjectSlotsManager.SelectedObjects;
            if (objects.Count == 0) return;

            objects.Sort((obj1, obj2) =>
            {
                string label1 = Config.ObjectSlotsManager.GetSlotLabelFromObject(obj1);
                string label2 = Config.ObjectSlotsManager.GetSlotLabelFromObject(obj2);
                int pos1 = ParsingUtilities.ParseInt(label1);
                int pos2 = ParsingUtilities.ParseInt(label2);
                return pos1 - pos2;
            });

            int initialX = (int)objects[0].X;
            int initialY = (int)objects[0].Y;
            int initialZ = (int)objects[0].Z;
            int xOffset = initialX - qStepsTruncated[0];
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].X = qStepsTruncated[i] + xOffset;
                objects[i].Y = initialY + i * yDiff;
                objects[i].Z = initialZ;
            }
        }

        public static void TestSomething4()
        {
            TtcMain.TtcMainMethod();
        }

        public static void TestSomething3()
        {
            List<DataManager> dataManagerList = Config.GetDataManagers();
            InfoForm.ShowValue(String.Join("\r\n\r\n", dataManagerList));
        }

        public static void TestSomething2()
        {
            List<string> output = new List<string>();
            for (int i = 0; i < 65536; i++)
            {
                float trig2 = InGameTrigUtilities.InGameCosine(i);
            }
            InfoForm.ShowValue(String.Join("\r\n", output));
        }

        public static List<double> marioPositions1 = new List<double>()
        {
            -5625.607422,-5598.169922,-5570.271484,-5541.921875,-5513.130859,-5483.910156,-5454.267578,
            -5424.214844,-5393.761719,-5362.916016,-5331.6875,-5300.085938,-5268.119141,-5235.794922,
            -5204.470703,-5172.775391,-5140.716797,-5109.658203,-5078.222656,-5046.417969,-5014.251953,
            -4983.085938,-4951.544922,-4919.638672,-4887.373047,-4856.107422,-4824.46875,-4792.466797,
            -4761.464844,-4730.083984,-4698.332031,-4666.21875,-4635.105469,-4603.617188,-4571.759766,
            -4539.542969,-4508.326172,-4476.736328,-4444.78125,-4412.46875,-4381.15625,-4349.472656,
            -4317.425781,-4286.378906,-4254.953125,-4223.158203,-4191.003906,-4159.849609,-4128.318359,
            -4096.421875,-4064.166016,-4032.910156,-4001.28125,-3969.288086,-3936.939453,-3905.59082,
            -3873.871094,-3841.789063,-3810.707031,-3779.248047,-3747.419922,-3715.232422,-3684.044922,
            -3652.482422,-3620.553711,-3588.267578,-3556.981445,-3525.323242,-3493.300781,-3462.27832,
            -3430.87793,-3399.107422,-3366.975586,-3335.84375,-3304.335938,-3272.460938,-3240.227539,
            -3208.994141,-3177.386719,-3145.415039,-3113.086914,-3081.758789,-3050.058594,-3017.996094,
            -2986.933594,-2955.493164,-2923.683594,-2891.513672,-2860.34375,-2828.798828,-2796.887695,
            -2764.619141,-2733.350586,-2701.708984,-2669.703125,-2638.697266,-2638.697266,
        };

        public static List<double> marioPositions2 = new List<double>()
        {
            -5575.169922,-5547.271484,-5518.921875,-5490.130859,-5460.910156,-5431.267578,
            -5401.214844,-5370.761719,-5339.916016,-5308.6875,-5277.085938,-5245.119141,-5212.794922,
            -5181.470703,-5149.775391,-5117.716797,-5086.658203,-5055.222656,-5023.417969,-4991.251953,
            -4960.085938,-4928.544922,-4896.638672,-4864.373047,-4833.107422,-4801.46875,-4769.466797,
            -4738.464844,-4707.083984,-4675.332031,-4643.21875,-4612.105469,-4580.617188,-4548.759766,
            -4516.542969,-4485.326172,-4453.736328,-4421.78125,-4389.46875,-4358.15625,-4326.472656,
            -4294.425781,-4263.378906,-4231.953125,-4200.158203,-4168.003906,-4136.849609,-4105.318359,
            -4073.421143,-4041.165283,-4009.909424,-3978.280518,-3946.287354,-3913.938721,-3882.590088,
            -3850.870361,-3818.78833,-3787.706299,-3756.247314,-3724.419189,-3692.231689,-3661.044189,
            -3629.481689,-3597.552979,-3565.266846,-3533.980713,-3502.32251,-3470.300049,-3439.277588,
        };
    }
} 
