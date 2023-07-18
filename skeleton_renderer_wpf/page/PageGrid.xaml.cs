using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Permissions;

using skeleton_renderer_wpf.data;
using skeleton_renderer_wpf.module;
using System.Windows.Threading;

namespace skeleton_renderer_wpf.page
{
    /// <summary>
    /// PageGrid.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PageGrid : Page
    {
        public DerivedEvent derivedEvent;

        public PageGrid()
        {
            InitializeComponent();
            this.derivedEvent = new DerivedEvent();
            this.derivedEvent.OnDerived += new EventHandler(OnDerived);

            grid_derivedData.AutoGenerateColumns = false;
            
            grid_derivedData.CanUserAddRows = false;
            //grid_derivedData.bin

            DataGridTextColumn[] cols = new DataGridTextColumn[8];
            for(int i=0;i<cols.Length;i++)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                grid_derivedData.Columns.Add(col);
                cols[i] = col;
            }
            string[] str_binding = { "time","part","roll","pitch","yaw","vX","vY","vZ" };
            string[] str_header = { "시각", "부위", "Roll", "Pitch", "Yaw", "vX", "vY", "vZ" };
            for(int i=0;i<cols.Length;i++)
            {
                Binding binding = new Binding(str_binding[i]);
                binding.Mode = BindingMode.OneWay;
                cols[i].Binding = binding;
                cols[i].Header = str_header[i];
                cols[i].IsReadOnly = true;
            }
        }


        private async Task<IList<DerivedData>> GetDatasAsync(int sleep, List<DerivedData> _dDatas)
        {
            await Task.Delay(sleep);
            List<DerivedData> _datas = new List<DerivedData>();
            foreach (DerivedData dData in _dDatas)
            {
                _datas.Add(dData);
            }
            return _datas;
        }

        private async void OnDerived(object sender, EventArgs e)
        {
            List<DerivedData> _dData = ((DerivedEventArgs)e).data;
            var _datas = await GetDatasAsync(1, _dData);
            grid_derivedData.Dispatcher.Invoke(new Action(delegate
            {
                grid_derivedData.ItemsSource = _datas;
            }
            ));
        }

    }
}
