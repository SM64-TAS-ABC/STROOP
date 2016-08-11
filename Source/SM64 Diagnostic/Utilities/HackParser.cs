using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace SM64_Diagnostic.Utilities
{
    public static class HackParser
    {
        public static void LoadHack(ProcessStream stream, string hackFileName)
        {
            var dataUntrimmed = File.ReadAllText(hackFileName);
            var data = Regex.Replace(dataUntrimmed, @"\s+", "");

            int nextEnd;
            int prevEnd = data.IndexOf(":");

            if (prevEnd < 8 || prevEnd == data.Length - 1)
                return;

            var hackAddresses = new List<Tuple<uint, byte[]>> (); 

            string remData = data.Substring(prevEnd + 1);

            do
            {
                nextEnd = remData.IndexOf(":");

                uint address = ParsingUtilities.ParseHex(data.Substring(prevEnd - 8, 8));
                string byteData = (nextEnd == -1) ? remData : remData.Substring(0, nextEnd - 8);

                var hackBytes = new byte[byteData.Length / 2];
                for (int i = 0; i < hackBytes.Length; i++)
                {
                    hackBytes[i] = (byte)ParsingUtilities.ParseHex(byteData.Substring(i * 2, 2));
                }

                hackAddresses.Add(new Tuple<uint, byte[]>(address, hackBytes));

                remData = remData.Substring(nextEnd + 1);
                prevEnd += nextEnd + 1;
            }
            while (nextEnd != -1);

            stream.Suspend();

            foreach (var address in hackAddresses)
                stream.WriteRam(address.Item2, address.Item1);

            stream.Resume();
        }
    }
}
