using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using skeleton_renderer_wpf.data;

namespace skeleton_renderer_wpf.module
{
    class DerivedEventArgs : EventArgs
    {
        public List<DerivedData> data;
    }
}
