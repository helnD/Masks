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
using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDrawing = false;
        
        public MainWindow()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }

        private void draw(Canvas canvas, double x, double y)
        {
            int numberOfColumns = 20;
            int numberOfRows = 20;
            double sizeX = (canvas.Width / numberOfColumns);
            double sizeY = (canvas.Height / numberOfRows);

            double marginLeft = (int)(x  / sizeX) * sizeX;
            double marginTop = (int)(y / sizeY) * sizeY;

            var context = DataContext as MainViewModel;
            var clr = (byte) context.BrushBrightness;
             
            Rectangle rect = new Rectangle();
            rect.Height = canvas.Height / numberOfRows;
            rect.Width = canvas.Width / numberOfColumns;
            rect.Fill = new SolidColorBrush(Color.FromRgb(clr, clr,clr));
            rect.Margin = new Thickness(marginLeft, marginTop, 0, 0);

            canvas.Children.Add(rect);
        }

        private void DrawCanvas_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            double cursorX = e.GetPosition(DrawCanvas).X;
            double cursorY = e.GetPosition(DrawCanvas).Y;
            
            Mouse.Capture(DrawCanvas);
            isDrawing = true;

            if (cursorX < DrawCanvas.Width && cursorY < DrawCanvas.Height &&
                cursorX > 0 && cursorY > 0)
            { 
                draw(DrawCanvas, cursorX, cursorY);
            }
        }

        private void DrawCanvas_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            double cursorX = e.GetPosition(DrawCanvas).X;
            double cursorY = e.GetPosition(DrawCanvas).Y;

            if (!isDrawing) return;
            
            if (cursorX < DrawCanvas.Width && cursorY < DrawCanvas.Height &&
                cursorX > 0 && cursorY > 0)
            { 
                draw(DrawCanvas, cursorX, cursorY);
            }    
        }

        private void DrawCanvas_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            double cursorX = e.GetPosition(DrawCanvas).X;
            double cursorY = e.GetPosition(DrawCanvas).Y;

            if (cursorX < DrawCanvas.Width && cursorY < DrawCanvas.Height &&
                cursorX > 0 && cursorY > 0)
            { 
                draw(DrawCanvas, cursorX, cursorY);
            }

            isDrawing = false;
            Mouse.Capture(null);
        }

        private void ClearCanvasButton_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DrawCanvas.Children.Clear();
        }
    }
}