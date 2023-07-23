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
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Windows.Threading;

using skeleton_renderer_wpf.module;
using skeleton_renderer_wpf.page;
using skeleton_renderer_wpf.data;

namespace skeleton_renderer_wpf
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    public enum GridColumn
    {
        time = 0,
        part = 1,
        roll = 2,
        pitch = 3,
        yaw = 4,
        vX = 5,
        vY = 6,
        vZ = 7
    }

    public partial class MainWindow : Window
    {
        private Client client;
        private Thread tcp_thread;

        private PageBoth pageBoth;
        private PageGrid pageGrid;
        private PageSkeleton pageSkeleton;

        private DispatcherTimer timer_bodyparts;
        

        public MainWindow()
        {
            InitializeComponent();

            client = new Client();
            pageGrid = new PageGrid(client);
            pageSkeleton = new PageSkeleton(client);
            txt_ip.Text = "192.168.1.168";
            txt_port.Text = "8000";
            timer_bodyparts = new DispatcherTimer();
            timer_bodyparts.Tick += BodyPartTimer_Tick;
            timer_bodyparts.Interval = TimeSpan.FromMilliseconds(1000);
            timer_bodyparts.Start();

        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            if (!client.Connected)
            {
                string ip = txt_ip.Text;
                int port = int.Parse(txt_port.Text);
                lb_hasCon.Content = "Connected";
                lb_hasCon.Background = Brushes.Green;
                tcp_thread = new Thread(new ThreadStart(delegate ()
                {
                    try
                    {
                        client.Connect(ip, port);
                    }
                    catch(Exception exception)
                    {
                        if(exception is IOException) //클라이언트 측의 연결 해제
                        {
                            Dispatcher.Invoke(new Action(
                            delegate
                            {
                                client.Close();
                                lb_hasCon.Content = "Not Connected";
                                lb_hasCon.Background = Brushes.Red;
                            }
                            ));
                        }
                        else if(exception is SocketException) //서버 종료
                        {
                            Dispatcher.Invoke(new Action(
                            delegate
                            {
                                string msg = exception.Message;
                                MessageBox.Show(msg, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                client.Close();
                                lb_hasCon.Content = "Not Connected";
                                lb_hasCon.Background = Brushes.Red;
                            }
                            ));
                        }
                        else
                        {
                            Dispatcher.Invoke(new Action(
                            delegate
                            {
                                string msg = exception.Message;
                                MessageBox.Show(msg, "Unknown Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                client.Close();
                                lb_hasCon.Content = "Not Connected";
                                lb_hasCon.Background = Brushes.Red;

                            }
                            ));
                        }
                    }
                }
                ));
                tcp_thread.Start();

            }
            else
            {
                client.Close();
                lb_hasCon.Content = "Not Connected";
                lb_hasCon.Background = Brushes.Red;
            }
        }

        private async void BodyPartTimer_Tick(object sender,EventArgs e)
        {
            var _bodyDatas = await GetDatasAsync(1, client.currentData);
            list_bodypart.Dispatcher.Invoke(new Action(delegate
            {
                list_bodypart.ItemsSource = _bodyDatas;
            }
            ));
        }

        private async void OnDerived(object sender, EventArgs e)
        {
            List<DerivedData> _dDatas = ((DerivedEventArgs)e).data;
            var _bodyDatas = await GetDatasAsync(1,_dDatas);
            list_bodypart.Dispatcher.Invoke(new Action(delegate
            {
                list_bodypart.ItemsSource = _bodyDatas;
            }
            ));
        }

        private async Task<IList<string>> GetDatasAsync(int sleep,List<DerivedData> _dDatas)
        {
            await Task.Delay(sleep);
            List<string> _parts = new List<string>();
            foreach (DerivedData dData in _dDatas)
            {
                _parts.Add(dData.part);
            }
            return _parts; 
        }

        private void rb_Checked(object sender, RoutedEventArgs e)
        {
            if(!client.Connected)
            {
                MessageBox.Show("Connect Sensor Receiver First");
                return;
            }
            RadioButton rb = (RadioButton)sender;
            if(rb==rb_grid)
            {
                frame_content.Navigate(pageGrid);
            }
            else if(rb==rb_skeleton)
            {
                frame_content.Navigate(pageSkeleton);
            }
            else if(rb==rb_both)
            {
                pageBoth = new PageBoth(pageGrid, pageSkeleton);
                frame_content.Navigate(pageBoth);
            }
        }
    }
}
