using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public class CoinObject
    {
        public readonly int NumCoins;
        public readonly int HSpeedScale;
        public readonly int VSpeedScale;
        public readonly int VSpeedOffset;
        public readonly CoinParamOrder CoinParamOrder;
        public readonly string Name;

        private CoinObject(
            int numCoins,
            int hSpeedScale,
            int vSpeedScale,
            int vSpeedOffset,
            CoinParamOrder coinParamOrder,
            string name)
        {
            NumCoins = numCoins;
            HSpeedScale = hSpeedScale;
            VSpeedScale = vSpeedScale;
            VSpeedOffset = vSpeedOffset;
            CoinParamOrder = coinParamOrder;
            Name = name;
        }

        private float CalculateHSpeed(int rngIndex)
        {
            ushort rngValue = RngIndexer.GetRngValue(rngIndex);
            float hSpeed = (rngValue / 65536f) * HSpeedScale;
            return hSpeed;
        }

        private float CalculateVSpeed(int rngIndex)
        {
            ushort rngValue = RngIndexer.GetRngValue(rngIndex);
            float vSpeed = (rngValue / 65536f) * VSpeedScale + VSpeedOffset;
            return vSpeed;
        }

        private ushort CalculateAngle(int rngIndex)
        {
            ushort rngValue = RngIndexer.GetRngValue(rngIndex);
            ushort angle = rngValue;
            return angle;
        }

        public List<CoinTrajectory> CalculateCoinTrajectories(int rngIndex)
        {
            List<CoinTrajectory> coinTrajectories = new List<CoinTrajectory>();
            for (int i = 0; i < NumCoins; i++)
            {
                CoinTrajectory coinTrajectory = CalculateCoinTrajectory(rngIndex + 3 * i);
                coinTrajectories.Add(coinTrajectory);
            }
            return coinTrajectories;
        }

        public CoinTrajectory CalculateCoinTrajectory(int rngIndex)
        {
            float hSpeed;
            float vSpeed;
            ushort angle;
            switch (CoinParamOrder)
            {
                case CoinParamOrder.HVA:
                    hSpeed = CalculateHSpeed(rngIndex + 0);
                    vSpeed = CalculateVSpeed(rngIndex + 1);
                    angle = CalculateAngle(rngIndex + 2);
                    break;
                case CoinParamOrder.HAV:
                    hSpeed = CalculateHSpeed(rngIndex + 0);
                    angle = CalculateAngle(rngIndex + 1);
                    vSpeed = CalculateVSpeed(rngIndex + 2);
                    break;
                case CoinParamOrder.VHA:
                    vSpeed = CalculateVSpeed(rngIndex + 0);
                    hSpeed = CalculateHSpeed(rngIndex + 1);
                    angle = CalculateAngle(rngIndex + 2);
                    break;
                case CoinParamOrder.VAH:
                    vSpeed = CalculateVSpeed(rngIndex + 0);
                    angle = CalculateAngle(rngIndex + 1);
                    hSpeed = CalculateHSpeed(rngIndex + 2);
                    break;
                case CoinParamOrder.AHV:
                    angle = CalculateAngle(rngIndex + 0);
                    hSpeed = CalculateHSpeed(rngIndex + 1);
                    vSpeed = CalculateVSpeed(rngIndex + 2);
                    break;
                case CoinParamOrder.AVH:
                    angle = CalculateAngle(rngIndex + 0);
                    vSpeed = CalculateVSpeed(rngIndex + 1);
                    hSpeed = CalculateHSpeed(rngIndex + 2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new CoinTrajectory(hSpeed, vSpeed, angle);
        }

        public override string ToString()
        {
            return Name;
        }

        public static List<CoinObject> GetCoinObjects()
        {
            List<CoinObject> coinObjectList =
                ControlUtilities.GetFieldsOfType<CoinObject>(typeof(CoinObject), null);
            coinObjectList.Sort((co1, co2) => String.Compare(co1.ToString(), co2.ToString()));
            return coinObjectList;
        }

        public static CoinObject Bobomb = new CoinObject(
            numCoins: 1,
            hSpeedScale: 20,
            vSpeedScale: 40,
            vSpeedOffset: 17,
            coinParamOrder: CoinParamOrder.HVA,
            name: "Bob-omb");

        public static CoinObject Scuttlebug = new CoinObject(
            numCoins: 3,
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 46,
            coinParamOrder: CoinParamOrder.VHA,
            name: "Scuttlebug");

        public static CoinObject TinyPiranhaPlant = new CoinObject(
            numCoins: 1,
            hSpeedScale: 10, // more like 9.929076195
            vSpeedScale: 10,
            vSpeedOffset: 46,
            coinParamOrder: CoinParamOrder.VHA,
            name: "Tiny Piranha Plant");

        public static CoinObject Goomba = new CoinObject(
            numCoins: 1,
            hSpeedScale: 10, // more like 9.936193656
            vSpeedScale: 10,
            vSpeedOffset: 46,
            coinParamOrder: CoinParamOrder.VHA,
            name: "Goomba");

        public static CoinObject Moneybag = new CoinObject(
            numCoins: 5,
            hSpeedScale: 20,
            vSpeedScale: 40,
            vSpeedOffset: 17,
            coinParamOrder: CoinParamOrder.HVA,
            name: "Moneybag");

        public static CoinObject CorkBox = new CoinObject(
            numCoins: 3,
            hSpeedScale: 20,
            vSpeedScale: 40,
            vSpeedOffset: 17,
            coinParamOrder: CoinParamOrder.HVA,
            name: "Cork Box");

    }
}
