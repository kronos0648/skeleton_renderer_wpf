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
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using _3DTools;


using skeleton_renderer_wpf.data;
using skeleton_renderer_wpf.module;

namespace skeleton_renderer_wpf.page
{
    /// <summary>
    /// PageSkeleton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PageSkeleton : Page
    {
        private Client client;

        private Viewport3D viewport3D;
        private PerspectiveCamera perspectiveCamera;

        private MeshGeometry3D side1Plane;
        private GeometryModel3D geometryModel3D;
        private ModelVisual3D modelVisual3D;

        public PageSkeleton(Client client)
        {
            InitializeComponent();
            this.client = client;
            this.viewport3D = new Viewport3D();
            Content = grid;
            grid.Children.Add(this.viewport3D);
            this.perspectiveCamera = GetPerspectiveCamera(new Point3D(15, 20, 15), new Vector3D(-1, -1, -1), new Vector3D(0, 1, 0), 100);
            viewport3D.Camera = perspectiveCamera;
            
        }

        private PerspectiveCamera GetPerspectiveCamera(Point3D position,Vector3D lookDirection,Vector3D upDirection,double fieldOfView)
        {
            PerspectiveCamera camera = new PerspectiveCamera();
            camera.Position = position;
            camera.LookDirection = lookDirection;
            camera.UpDirection = upDirection;
            camera.FieldOfView = fieldOfView;


            return camera;
        }

        private ScreenSpaceLines3D GetLine3D(Point3D startPoint, Point3D endPoint, double thickness, Color color)
        {
            ScreenSpaceLines3D screenSpaceLines3D = new ScreenSpaceLines3D();

            screenSpaceLines3D.Points.Add(startPoint);
            screenSpaceLines3D.Points.Add(endPoint);

            screenSpaceLines3D.Thickness = thickness;
            screenSpaceLines3D.Color = color;

            return screenSpaceLines3D;
        }

        private void DrawAxisLine(Viewport3D viewport3D)
        {
            ScreenSpaceLines3D xAxisLine = GetLine3D(new Point3D(0, 0, 0), new Point3D(10, 0, 0), 2, Colors.Red);
            ScreenSpaceLines3D yAxisLine = GetLine3D(new Point3D(0, 0, 0), new Point3D(0, 10, 0), 2, Colors.Blue);
            ScreenSpaceLines3D zAxisLine = GetLine3D(new Point3D(0, 0, 0), new Point3D(0, 0, 10), 2, Colors.Black);

            viewport3D.Children.Add(xAxisLine);
            viewport3D.Children.Add(yAxisLine);
            viewport3D.Children.Add(zAxisLine);
        }

        private void DrawXZPlane(Viewport3D viewport3D)
        {
            for (int i = -20; i < 20; i++)
            {
                ScreenSpaceLines3D xLine = GetLine3D(new Point3D(-20, 0, i * 1), new Point3D(20, 0, i * 1), 0.5, Colors.Green);
                ScreenSpaceLines3D zLine = GetLine3D(new Point3D(i * 1, 0, -20), new Point3D(i * 1, 0, 20), 1, Colors.Green);

                viewport3D.Children.Add(xLine);
                viewport3D.Children.Add(zLine);
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DrawAxisLine(viewport3D);
            DrawXZPlane(viewport3D);
        }
    }
}
