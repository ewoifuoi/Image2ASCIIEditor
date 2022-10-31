﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Image2ASCIIEditor.Common;
using Console = Image2ASCIIEditor.Common.Console;
using Microsoft.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Image2ASCIIEditor.Views.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EditText : Page
{
    private bool _isMouseDown = false;
    private PointerPoint _mouseDownPosition;
    private Point _mouseDownControlPosition;
    private Canvas _canvas;

    public EditText()
    {
        this.InitializeComponent();
    }

    private void outside_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        PointerPoint currentPoint = e.GetCurrentPoint(outside);
        double s = ((double)currentPoint.Properties.MouseWheelDelta) / 1000 + 1;

        TransformGroup transformGroup = playground.RenderTransform as TransformGroup;
        transformGroup.Children.Add(new ScaleTransform() { CenterX = outside.Width / 2, CenterY = outside.Height / 2, ScaleX = s, ScaleY = s });
    }

    private void playground_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        var c = sender as Canvas;
        _isMouseDown = true;
        _mouseDownPosition = e.GetCurrentPoint(outside);
        _mouseDownControlPosition = new Point(double.IsNaN(Canvas.GetLeft(c)) ? 0 : Canvas.GetLeft(c), double.IsNaN(Canvas.GetTop(c)) ? 0 : Canvas.GetTop(c));

        c.CapturePointer(e.Pointer);
    }

    private void playground_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        var c = sender as Canvas;
        _isMouseDown = false;
        c.ReleasePointerCapture(e.Pointer);
    }

    private void playground_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (_isMouseDown)
        {
            var c = sender as Canvas;
            _canvas = c;
            var pos = e.GetCurrentPoint(outside);
            var dpx = pos.Position.X - _mouseDownPosition.Position.X;
            var dpy = pos.Position.Y - _mouseDownPosition.Position.Y;
            Canvas.SetLeft(c, _mouseDownControlPosition.X + dpx);
            Canvas.SetTop(c, _mouseDownControlPosition.Y + dpy);
        }
    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
        
        g.ColumnDefinitions.Clear();
        g.RowDefinitions.Clear();
        g.Children.Clear();
        for (int i = 0; i < 10; i++)
        {
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.RowDefinitions.Add(new RowDefinition());
        }
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                var border = new Border();
                var b = new TextBlock();
                //b.Background = new SolidColorBrush(p.Color);
                b.Text = "*";
                b.TextAlignment = TextAlignment.Center;
                b.FontSize = 45;
                b.Width = g.Width / 10;
                b.Height = g.Height / 10;
                //b.Click += B_Click;
                border.Child = b;
                border.BorderThickness = new Thickness(1); border.BorderBrush = new SolidColorBrush(Colors.Gray);
                Grid.SetColumn(border, i);
                Grid.SetRow(border, j);
                Grid.SetRowSpan(border, 1); Grid.SetColumnSpan(border, 1);
                g.Children.Add(border);

            }
        }

    }
}
