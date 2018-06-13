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
        private readonly int _hSpeedMultiplier;
        private readonly int _hSpeedOffset;
        private readonly int _vSpeedOffset;
        private readonly CoinParamsOrder _coinParamsOrder;

        private enum CoinParamsOrder
        {
            HVA, HAV, VHA, VAH, AHV, AVH
        }

        private CoinObject(
            int hSpeedMultiplier,
            int hSpeedOffset,
            int vSpeedOffset,
            CoinParamsOrder coinParamsOrder)
        {
            _hSpeedMultiplier = hSpeedMultiplier;
            _hSpeedOffset = hSpeedOffset;
            _vSpeedOffset = vSpeedOffset;
            _coinParamsOrder = coinParamsOrder;
        }

        //public static CoinObject Bobomb = new CoinObject()

    }
}
