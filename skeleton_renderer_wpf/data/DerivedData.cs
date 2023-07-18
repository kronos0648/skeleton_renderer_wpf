using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skeleton_renderer_wpf.data
{
    public class DerivedData
    {
        public string time { get; set; }
        public string part { get; set; }
        public float roll { get; set; }
        public float pitch { get; set; }
        public float yaw { get; set; }
        public float vX { get; set; }
        public float vY { get; set; }
        public float vZ { get; set; }

        public object[] GetRow()
        {
            object[] row = new object[8];
            row[0] = time;
            row[1] = part;
            row[2] = roll;
            row[3] = pitch;
            row[4] = yaw;
            row[5] = vX;
            row[6] = vY;
            row[7] = vZ;
            return row;
        }

    }
}
