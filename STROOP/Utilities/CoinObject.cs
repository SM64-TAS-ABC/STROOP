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
        private readonly int _numCoins;
        private readonly int _hSpeedScale;
        private readonly int _vSpeedScale;
        private readonly int _vSpeedOffset;
        private readonly CoinParamsOrder _coinParamsOrder;

        private enum CoinParamsOrder
        {
            HVA, HAV, VHA, VAH, AHV, AVH
        }

        private CoinObject(
            int numCoins,
            int hSpeedScale,
            int vSpeedScale,
            int vSpeedOffset,
            CoinParamsOrder coinParamsOrder)
        {
            _numCoins = numCoins;
            _hSpeedScale = hSpeedScale;
            _vSpeedScale = vSpeedScale;
            _vSpeedOffset = vSpeedOffset;
            _coinParamsOrder = coinParamsOrder;
        }

        private float CalculateHSpeed(int rngIndex)
        {
            ushort rngValue = RngIndexer.GetRngValue(rngIndex);
            float hSpeed = (rngValue / 65536f) * _hSpeedScale;
            return hSpeed;
        }

        private float CalculateVSpeed(int rngIndex)
        {
            ushort rngValue = RngIndexer.GetRngValue(rngIndex);
            float vSpeed = (rngValue / 65536f) * _vSpeedScale + _vSpeedOffset;
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
            for (int i = 0; i < _numCoins; i++)
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
            switch (_coinParamsOrder)
            {
                case CoinParamsOrder.HVA:
                    hSpeed = CalculateHSpeed(rngIndex + 0);
                    vSpeed = CalculateVSpeed(rngIndex + 1);
                    angle = CalculateAngle(rngIndex + 2);
                    break;
                case CoinParamsOrder.HAV:
                    hSpeed = CalculateHSpeed(rngIndex + 0);
                    angle = CalculateAngle(rngIndex + 1);
                    vSpeed = CalculateVSpeed(rngIndex + 2);
                    break;
                case CoinParamsOrder.VHA:
                    vSpeed = CalculateVSpeed(rngIndex + 0);
                    hSpeed = CalculateHSpeed(rngIndex + 1);
                    angle = CalculateAngle(rngIndex + 2);
                    break;
                case CoinParamsOrder.VAH:
                    vSpeed = CalculateVSpeed(rngIndex + 0);
                    angle = CalculateAngle(rngIndex + 1);
                    hSpeed = CalculateHSpeed(rngIndex + 2);
                    break;
                case CoinParamsOrder.AHV:
                    angle = CalculateAngle(rngIndex + 0);
                    hSpeed = CalculateHSpeed(rngIndex + 1);
                    vSpeed = CalculateVSpeed(rngIndex + 2);
                    break;
                case CoinParamsOrder.AVH:
                    angle = CalculateAngle(rngIndex + 0);
                    vSpeed = CalculateVSpeed(rngIndex + 1);
                    hSpeed = CalculateHSpeed(rngIndex + 2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new CoinTrajectory(hSpeed, vSpeed, angle);
        }

        public static CoinObject Bobomb = new CoinObject(
            numCoins: 1,
            hSpeedScale: 20,
            vSpeedScale: 40,
            vSpeedOffset: 17,
            coinParamsOrder: CoinParamsOrder.HVA);

        public static CoinObject Scuttlebug = new CoinObject(
            numCoins: 3,
            hSpeedScale: 10,
            vSpeedScale: 10,
            vSpeedOffset: 46,
            coinParamsOrder: CoinParamsOrder.VHA);

        public static CoinObject TinyPiranhaPlant = new CoinObject(
            numCoins: 1,
            hSpeedScale: 10, // more like 9.929076195
            vSpeedScale: 10,
            vSpeedOffset: 46,
            coinParamsOrder: CoinParamsOrder.VHA);

        public static CoinObject Goomba = new CoinObject(
            numCoins: 1,
            hSpeedScale: 10, // more like 9.936193656
            vSpeedScale: 10,
            vSpeedOffset: 46,
            coinParamsOrder: CoinParamsOrder.VHA);

        public static CoinObject Moneybag = new CoinObject(
            numCoins: 5,
            hSpeedScale: 20,
            vSpeedScale: 40,
            vSpeedOffset: 17,
            coinParamsOrder: CoinParamsOrder.HVA);

        public static CoinObject CorkBox = new CoinObject(
            numCoins: 3,
            hSpeedScale: 20,
            vSpeedScale: 40,
            vSpeedOffset: 17,
            coinParamsOrder: CoinParamsOrder.HVA);
    }
}
