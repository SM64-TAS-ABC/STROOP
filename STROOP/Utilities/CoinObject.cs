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
        public readonly double HSpeedScale;
        public readonly double VSpeedScale;
        public readonly double VSpeedOffset;
        public readonly CoinParamOrder CoinParamOrder;
        public readonly int NumCoins;
        public readonly string Name;

        public CoinObject(
            double hSpeedScale,
            double vSpeedScale,
            double vSpeedOffset,
            CoinParamOrder coinParamOrder,
            int numCoins,
            string name)
        {
            NumCoins = numCoins;
            HSpeedScale = hSpeedScale;
            VSpeedScale = vSpeedScale;
            VSpeedOffset = vSpeedOffset;
            CoinParamOrder = coinParamOrder;
            Name = name;
        }

        private double CalculateHSpeed(int rngIndex)
        {
            ushort rngValue = RngIndexer.GetRngValue(rngIndex);
            double hSpeed = (rngValue / 65536d) * HSpeedScale;
            return hSpeed;
        }

        private double CalculateVSpeed(int rngIndex)
        {
            ushort rngValue = RngIndexer.GetRngValue(rngIndex);
            double vSpeed = (rngValue / 65536d) * VSpeedScale + VSpeedOffset;
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
            double hSpeed;
            double vSpeed;
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
            hSpeedScale: 20,
            vSpeedScale: 40,
            vSpeedOffset: 20,
            coinParamOrder: CoinParamOrder.HVA,
            numCoins: 1,
            name: "Bob-omb");

        public static CoinObject Bookend = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 1,
            name: "Bookend");

        public static CoinObject Chuckya = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 5,
            name: "Chuckya");

        public static CoinObject CoinBlock = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 30,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 1,
            name: "Coin Block");

        public static CoinObject CorkBox = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 3,
            name: "Cork Box");

        public static CoinObject CrazyBox = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 5,
            name: "Crazy Box");

        public static CoinObject FlyGuy = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 2,
            name: "Fly Guy");

        public static CoinObject Goomba = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 1,
            name: "Goomba");

        public static CoinObject GrabbableCorkBox = new CoinObject(
            hSpeedScale: 20,
            vSpeedScale: 40,
            vSpeedOffset: 20,
            coinParamOrder: CoinParamOrder.HVA,
            numCoins: 3,
            name: "Grabbable Cork Box");

        public static CoinObject HugePiranhaPlant = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 2,
            name: "Huge Piranha Plant");

        public static CoinObject Koopa = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 1,
            name: "Koopa");

        public static CoinObject Lakitu = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 5,
            name: "Lakitu");

        public static CoinObject LLLBowserPuzzle = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 30,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 5,
            name: "LLL Bowser Puzzle");

        public static CoinObject Moneybag = new CoinObject(
            hSpeedScale: 20,
            vSpeedScale: 40,
            vSpeedOffset: 20,
            coinParamOrder: CoinParamOrder.HVA,
            numCoins: 5,
            name: "Moneybag");

        public static CoinObject MrBlizzard = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 3,
            name: "Mr. Blizzard");

        public static CoinObject MrI = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 1,
            name: "Mr. I");

        public static CoinObject PiranhaPlant = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 1,
            name: "Piranha Plant");

        public static CoinObject Pokey = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 1,
            name: "Pokey");

        public static CoinObject Scuttlebug = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 3,
            name: "Scuttlebug");

        public static CoinObject Skeeter = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 3,
            name: "Skeeter");

        public static CoinObject Snufit = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 2,
            name: "Snufit");

        public static CoinObject Spindrift = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 3,
            name: "Spindrift");

        public static CoinObject Swooper = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 1,
            name: "Swooper");

        public static CoinObject TinyPiranhaPlant = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 1,
            name: "Tiny Piranha Plant");

        public static CoinObject WhompJumpOn = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 30,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 1,
            name: "Whomp Jump On");

        public static CoinObject WhompKill = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 5,
            name: "Whomp Kill");

        public static CoinObject WoodenPost = new CoinObject(
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 50,
            coinParamOrder: CoinParamOrder.VHA,
            numCoins: 5,
            name: "Wooden Post");
    }
}
