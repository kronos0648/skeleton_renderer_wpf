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

namespace skeleton_renderer_wpf.page
{
    /// <summary>
    /// PageBoth.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PageBoth : Page
    {
        private PageGrid grid;
        private PageSkeleton skeleton;
        public PageBoth(PageGrid grid,PageSkeleton skeleton)
        {
            InitializeComponent();
            this.grid = grid;
            this.skeleton = skeleton;
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            frame_grid.Navigate(grid);
            frame_skeleton.Navigate(skeleton);
        }
    }
}
