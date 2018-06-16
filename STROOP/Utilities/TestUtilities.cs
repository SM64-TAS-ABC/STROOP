using STROOP.Forms;
using STROOP.Managers;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class TestUtilities
    {

        public static void TestSomething()
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

    }
} 
