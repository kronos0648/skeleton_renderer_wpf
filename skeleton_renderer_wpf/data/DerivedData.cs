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

        public float accX { get; set; }
        public float accY { get; set; }
        public float accZ { get; set; }
        public float gyroX { get; set; }
        public float gyroY { get; set; }
        public float gyroZ { get; set; }
        public float vX { get; set; }
        public float vY { get; set; }
        public float vZ { get; set; }
        public float dX { get; set; }
        public float dY { get; set; }
        public float dZ { get; set; }

    }
}
