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
            var color = (byte) context.BrushBrightness;
            context.SourceModel[(int) (y / sizeY)][(int) (x / sizeX)] = color;
            
            Rectangle rect = new Rectangle
            {
                Height = canvas.Height / numberOfRows,
                Width = canvas.Width / numberOfColumns,
                Fill = new SolidColorBrush(Color.FromRgb(color, color, color)),
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
            
            var context = DataContext as MainViewModel;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    context.SourceModel[i][j] = 255;
                }
            }
        }

        private void DrawResult(Canvas canvas, int x, int y, byte color)
        {
            int numberOfColumns = 20;
            int numberOfRows = 20;
            double sizeX = (canvas.Width / numberOfColumns);
            double sizeY = (canvas.Height / numberOfRows);

            Rectangle rect = new Rectangle
            {
                Height = canvas.Height / numberOfRows,
                Width = canvas.Width / numberOfColumns,
                Fill = new SolidColorBrush(Color.FromRgb(color, color, color)),
                Margin = new Thickness(x * sizeX + 1, y * sizeY + 1,
                    (x + 1) * sizeX - 1, (y + 1) * sizeY - 1)
            };

            canvas.Children.Add(rect);
        }

        private void ApplyMask_OnClick(object sender, RoutedEventArgs e)
        {
            
            ResultCanvas.Children.Clear();
            
            var context = DataContext as MainViewModel;
            context.ApplyMask(context.SourceModel);
            for (int i = 0; i < context.ResultModel.Length; i++)
            {
                for (int j = 0; j < context.ResultModel[i].Length; j++)
                {
                    DrawResult(ResultCanvas, j, i, (byte)context.ResultModel[i][j]);
                }
            }
        }
        
        private void ApplyMaskToResult_OnClick(object sender, RoutedEventArgs e)
        {
            ResultCanvas.Children.Clear();
            
            var context = DataContext as MainViewModel;
            context.ApplyMask(context.ResultModel);
            for (int i = 0; i < context.ResultModel.Length; i++)
            {
                for (int j = 0; j < context.ResultModel[i].Length; j++)
                {
                    DrawResult(ResultCanvas, j, i, (byte)context.ResultModel[i][j]);
                }
            }
        }
    }
}