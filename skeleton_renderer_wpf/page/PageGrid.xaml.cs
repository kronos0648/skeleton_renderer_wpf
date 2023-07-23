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
        private DispatcherTimer timer;

        private Client client;

        public PageGrid(Client client)
        {
            InitializeComponent();
            this.client = client;
            grid_derivedData.AutoGenerateColumns = false;
            grid_derivedData.CanUserAddRows = false;
            string[] str_binding = { "time", "part", "roll", "pitch", "yaw", "accX", "accY", "accZ", "gyroX", "gyroY", "gyroZ", "vX", "vY", "vZ", "dX", "dY", "dZ" };
            string[] str_header = { "시각", "부위", "Roll", "Pitch", "Yaw", "accX", "accY", "accZ", "gyroX", "gyroY", "gyroZ", "vX", "vY", "vZ", "dX", "dY", "dZ" };
            DataGridTextColumn[] cols = new DataGridTextColumn[str_header.Length];
            for(int i=0;i<cols.Length;i++)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                grid_derivedData.Columns.Add(col);
                cols[i] = col;
            }
            
            for(int i=0;i<cols.Length;i++)
            {
                Binding binding = new Binding(str_binding[i]);
                binding.Mode = BindingMode.OneWay;
                cols[i].Binding = binding;
                cols[i].Header = str_header[i];
                cols[i].IsReadOnly = true;
            }
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Start();
        }

        private async void Timer_Tick(object sender,EventArgs e)
        {
            var _datas = await GetDatasAsync(1, client.currentData);
            grid_derivedData.Dispatcher.Invoke(new Action(delegate
            {
                grid_derivedData.ItemsSource = _datas;
            }
            ));
        }



        private async Task<IList<DerivedData>> GetDatasAsync(int sleep, List<DerivedData> _dDatas)
        {
            await Task.Delay(sleep);
            List<DerivedData> _datas = new List<DerivedData>();
            foreach (DerivedData dData in _dDatas)
            {
                _datas.Add(dData);
            }
            _datas = _datas.OrderBy(x => x.part).ToList();
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
