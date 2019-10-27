using System;
using System.Linq;
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
            if (ShowValuesCheckBox.IsChecked == true)
            {
                canvas.Children.Add(new Label()
                {
                    Content = context.SourceModel[(int) (y / sizeY)][(int) (x / sizeX)],
                    Margin = new Thickness(marginLeft - 3, marginTop - 3, 0, 0),
                    Foreground = GetFontColor(context.SourceModel[(int) (y / sizeY)][(int) (x / sizeX)]),
                    FontSize = 9,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalContentAlignment = VerticalAlignment.Center
                });
            }
        }

        private Brush GetFontColor(int colorBrightness)
        {
            if (colorBrightness < 127)
            {
                return Brushes.White;
            }
            else
            {
                return Brushes.Black;
            }
        }

        private void DrawGrid(Canvas canvas)
        {
            int numberOfColumns = 20;
            int numberOfRows = 20;
            double sizeX = (canvas.Width / numberOfColumns);
            double sizeY = (canvas.Height / numberOfRows);
            
            for (int i = 1; i < numberOfColumns; i++)
            {
                Line line = new Line
                {
                    X1 = sizeX * i,
                    Y1 = canvas.Margin.Top,
                    X2 = sizeX * i,
                    Y2 = canvas.Height + canvas.Margin.Top,
                    Stroke = Brushes.DarkGray,
                    StrokeThickness = 0.5,
                    StrokeDashArray = {6, 3}
                };
                Panel.SetZIndex(line, 10);
                canvas.Children.Add(line);
            }
            for (int i = 1; i < numberOfRows; i++)
            {
                Line line = new Line
                {
                    Y1 = sizeX * i,
                    X1 = canvas.Margin.Top,
                    Y2 = sizeX * i,
                    X2 = canvas.Width + canvas.Margin.Top,
                    Stroke = Brushes.DarkGray,
                    StrokeThickness = 0.5,
                    StrokeDashArray = {6, 3}
                };
                Panel.SetZIndex(line, 10);
                canvas.Children.Add(line);
            }
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
            RemoveRectangles(DrawCanvas);
            RemoveRectangles(ResultCanvas);
            
            var context = DataContext as MainViewModel;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    context.SourceModel[i][j] = 255;
                    context.ResultModel[i][j] = 255;
                }
            }
        }

        private void DrawCanvas_OnInitialized(object sender, EventArgs e)
        {
            DrawGrid(sender as Canvas);
        }

        private void ShowValuesCheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            RemoveLabels(DrawCanvas);
            RemoveLabels(ResultCanvas);
            
            if (ShowValuesCheckBox.IsChecked == true)
            {
                var context = DataContext as MainViewModel;
                DrawLabels(DrawCanvas, context.SourceModel);
                DrawLabels(ResultCanvas, context.ResultModel);
            }
        }

        private void RemoveLabels(Canvas canvas)
        {
            for (int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                if (canvas.Children[i] is Label)
                {
                    canvas.Children.RemoveAt(i);
                }
            }
        }
        
        private void DrawLabels(Canvas canvas, int[][] model)
        {
            int numberOfColumns = 20;
            int numberOfRows = 20;
            double sizeX = (canvas.Width / numberOfColumns);
            double sizeY = (canvas.Height / numberOfRows);
            
            for (int i = 0; i < model.Length; i++)
            {
                for (int j = 0; j < model[i].Length; j++)
                {
                    if (model[i][j] != 255)
                    {
                        double marginLeft = j * sizeX;
                        double marginTop = i * sizeY;
                                
                        canvas.Children.Add(new Label()
                        {
                            Content = model[i][j],
                            Margin = new Thickness(marginLeft - 3, marginTop - 3, 0, 0),
                            Foreground = GetFontColor(model[i][j]),
                            FontSize = 9,
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                            VerticalContentAlignment = VerticalAlignment.Center
                        });
                    }
                }
            }
        }

        private void DrawImageModel(Canvas canvas, int x, int y, byte color)
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
            RemoveRectangles(ResultCanvas);

            var context = DataContext as MainViewModel;
            context.ApplyMask();
            for (int i = 0; i < context.ResultModel.Length; i++)
            {
                for (int j = 0; j < context.ResultModel[i].Length; j++)
                {
                    DrawImageModel(ResultCanvas, j, i, (byte)context.ResultModel[i][j]);
                }
            }
            
            if (ShowValuesCheckBox.IsChecked == true)
                DrawLabels(ResultCanvas, context.ResultModel);
        }
        
        private void ApplyMaskToResult_OnClick(object sender, RoutedEventArgs e)
        {
            RemoveRectangles(DrawCanvas);
            RemoveRectangles(ResultCanvas);

            var context = DataContext as MainViewModel;
            context.ApplyMaskToResult();
            for (int i = 0; i < context.ResultModel.Length; i++)
            {
                for (int j = 0; j < context.ResultModel[i].Length; j++)
                {
                    DrawImageModel(ResultCanvas, j, i, (byte)context.ResultModel[i][j]);
                    DrawImageModel(DrawCanvas, j, i, (byte)context.SourceModel[i][j]);

                }
            }

            if (ShowValuesCheckBox.IsChecked == true)
            {
                DrawLabels(DrawCanvas, context.SourceModel);
                DrawLabels(ResultCanvas, context.ResultModel); 
            }
        }

        private void RemoveRectangles(Canvas canvas)
        {
            for (int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                if (canvas.Children[i] is Rectangle || canvas.Children[i] is Label) 
                    canvas.Children.RemoveAt(i);
            }
        }
    }
}