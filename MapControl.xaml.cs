namespace WifiSecurity.Control.CustomControls
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using MapProjection;

    /// <summary>
    /// Interaction logic for MapControl.xaml.
    /// </summary>
    public partial class MapControl : UserControl
    {
        public MapControl()
        {
            this.InitializeComponent();
            this.MapImage.SizeChanged += this.MapImageLoaded;
        }

        private void MapImageLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MainGrid.Children.RemoveRange(2, this.MainGrid.Children.Count - 2);
            CheckCountries(this.MapImage.RenderSize);
        }

        public void CheckCountries(Size mapImageRenderSize)
        {
            foreach (var item in InCoordinates.CountriesCoordinatesMap.CountryCoordinateList)
            {
                var item1 = MercatorProjectionCalc.GetProjection(item.Latitude, item.Longitude, mapImageRenderSize);
                var outCheckResult =
                    OutCoordinates.CountryRelativeSizeFixedCoordinatesList.FirstOrDefault(
                        x => x.CountryCode.Equals(item.CountryCode));

                if (outCheckResult != null)
                {
                    PutPointAt(item1, Brushes.Red, item.CountryCode, false);
                    //var point = new Point(outCheckResult.Longitude * mapImageRenderSize.Width,
                    //    outCheckResult.Latitude * mapImageRenderSize.Height);
                    //PutPointAt(point, Brushes.Yellow, item.CountryCode);
                }
                else
                {
                    // PutPointAt(item1, Brushes.Yellow, item.CountryCode, true);
                }
            }
        }

        public void PutPointAt(Point item, Brush color, string code, bool isSmallCountry)
        {
            var path = new Path();
            path.Fill = color;
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1;
            var radius = isSmallCountry ? 1 : 3;
            path.Data = new EllipseGeometry(item, radius, radius);

            this.MainGrid.Children.Add(path);

            var label = new TextBlock();
            label.Text = code;
            label.Margin = new Thickness(item.X, item.Y, 0, 0);
            label.Foreground = color;
            this.MainGrid.Children.Add(label);
        }
    }
}