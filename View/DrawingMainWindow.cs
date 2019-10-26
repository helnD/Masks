using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ViewModel;

namespace View
{
    public partial class MainWindow : Window
    {
        private bool _isDrawing;

        private void Draw(Canvas canvas, double x, double y)
        {
            int numberOfColumns = 20;
            int numberOfRows = 20;
            double sizeX = (canvas.Width / numberOfColumns);
            double sizeY = (canvas.Height / numberOfRows);

            double marginLeft = (int)(x  / sizeX) * sizeX;
            double marginTop = (int)(y / sizeY) * sizeY;

            var context = DataContext as MainViewModel;
            var clr = (byte) context.BrushBrightness;

            Rectangle rect = new Rectangle
            {
                Height = canvas.Height / numberOfRows,
                Width = canvas.Width / numberOfColumns,
                Fill = new SolidColorBrush(Color.FromRgb(clr, clr, clr)),
                Margin = new Thickness(marginLeft, marginTop, 0, 0)
            };

            canvas.Children.Add(rect);
        }

        private void DrawCanvas_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            double cursorX = e.GetPosition(DrawCanvas).X;
            double cursorY = e.GetPosition(DrawCanvas).Y;
            
            Mouse.Capture(DrawCanvas);
            _isDrawing = true;

            if (cursorX < DrawCanvas.Width && cursorY < DrawCanvas.Height &&
                cursorX > 0 && cursorY > 0)
            { 
                Draw(DrawCanvas, cursorX, cursorY);
            }
        }

        private void DrawCanvas_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            double cursorX = e.GetPosition(DrawCanvas).X;
            double cursorY = e.GetPosition(DrawCanvas).Y;

            if (!_isDrawing) return;
            
            if (cursorX < DrawCanvas.Width && cursorY < DrawCanvas.Height &&
                cursorX > 0 && cursorY > 0)
            { 
                Draw(DrawCanvas, cursorX, cursorY);
            }    
        }

        private void DrawCanvas_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            double cursorX = e.GetPosition(DrawCanvas).X;
            double cursorY = e.GetPosition(DrawCanvas).Y;

            if (cursorX < DrawCanvas.Width && cursorY < DrawCanvas.Height &&
                cursorX > 0 && cursorY > 0)
            { 
                Draw(DrawCanvas, cursorX, cursorY);
            }

            _isDrawing = false;
            Mouse.Capture(null);
        }

        private void ClearCanvasButton_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DrawCanvas.Children.Clear();
        }
    }
}