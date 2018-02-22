using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Utilities;

namespace STROOP.Models
{
    public abstract class UpdatableDataModel
    {
        public abstract void Update(int dependencyLevel);
    }
}
