using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using skeleton_renderer_wpf.data;

namespace skeleton_renderer_wpf.module
{
    public class DerivedEvent
    {
        public event EventHandler OnDerived;

        public void raise(List<DerivedData> data)
        {
            DerivedEventArgs arg = new DerivedEventArgs();
            arg.data = data;
            OnDerived(this, arg);
        }
    }
}
