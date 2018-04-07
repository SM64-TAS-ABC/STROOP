using STROOP.Forms;
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
            List<string> output = new List<string>();
            for (int i = 0; i < 65536; i++)
            {
                float trig1 = MoreMath.InGameCosine(i);
                float trig2 = InGameTrigUtilities.InGameCosine(i);
                float diff = trig2 - trig1;
                if (diff != 0)
                {
                    output.Add(i + "\t" + trig1 + "\t" + trig2 + "\t" + diff);
                }
            }
            InfoForm.ShowValue(String.Join("\r\n", output));
        }

    }
} 
