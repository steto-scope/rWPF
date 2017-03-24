using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace rwpf.Input
{
    /// <summary>
    /// ProgressSlider
    /// </summary>
    public partial class ProgressSlider : RangeBase
    {

        #region Internal DependencyProperties
        private double PreviewValue
        {
            get { return (double)GetValue(PreviewValueProperty); }
            set { SetValue(PreviewValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviewValue.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty PreviewValueProperty =
            DependencyProperty.Register("PreviewValue", typeof(double), typeof(ProgressSlider), new PropertyMetadata(0.0));



        private double CurrentPreviewOpacity
        {
            get { return (double)GetValue(CurrentPreviewOpacityProperty); }
            set { SetValue(CurrentPreviewOpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviewOpacity.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty CurrentPreviewOpacityProperty =
            DependencyProperty.Register("CurrentPreviewOpacity", typeof(double), typeof(ProgressSlider), new PropertyMetadata(0.0));

        #endregion


        #region Dependency Properties

        /// <summary>
        /// Sets the Foreground of the Preview-ProgressBar
        /// </summary>
        public Brush PreviewForeground
        {
            get { return (Brush)GetValue(PreviewForegroundProperty); }
            set { SetValue(PreviewForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviewForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviewForegroundProperty =
            DependencyProperty.Register("PreviewForeground", typeof(Brush), typeof(ProgressSlider), new PropertyMetadata(new SolidColorBrush(Colors.Orange)));



        /// <summary>
        /// If true, the X-Button is shown that sets the Value to the Minimum on Click
        /// </summary>
        public bool DisplayMinButton
        {
            get { return (bool)GetValue(DisplayMinButtonProperty); }
            set { SetValue(DisplayMinButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayMinButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayMinButtonProperty =
            DependencyProperty.Register("DisplayMinButton", typeof(bool), typeof(ProgressSlider), new PropertyMetadata(true));


        /// <summary>
        /// If true, the CheckMark-Button is shown that sets the Value to the Maximum on click
        /// </summary>
        public bool DisplayMaxButton
        {
            get { return (bool)GetValue(DisplayMaxButtonProperty); }
            set { SetValue(DisplayMaxButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayMaxButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayMaxButtonProperty =
            DependencyProperty.Register("DisplayMaxButton", typeof(bool), typeof(ProgressSlider), new PropertyMetadata(true));



        /// <summary>
        /// If true, a second ProgressBar (Preview) of the next Value is shown on MouseOver
        /// </summary>
        public bool DisplayPreview
        {
            get { return (bool)GetValue(DisplayPreviewProperty); }
            set { SetValue(DisplayPreviewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayPreview.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayPreviewProperty =
            DependencyProperty.Register("DisplayPreview", typeof(bool), typeof(ProgressSlider), new PropertyMetadata(true));



        /// <summary>
        /// If true, visual selection is disabled. The Control acts like a ProgressBar instead of a ProgressBar/Slider combo
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ProgressSlider), new PropertyMetadata(false));



        /// <summary>
        /// If true, visual selection snaps to the nearest tick below the Value. 
        /// Setting Value in Code-behind or View Model still allowes setting arbitary values.
        /// </summary>
        public bool IsSnapToTickEnabled
        {
            get { return (bool)GetValue(IsSnapToTickEnabledProperty); }
            set { SetValue(IsSnapToTickEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSnapToTickEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSnapToTickEnabledProperty =
            DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(ProgressSlider), new PropertyMetadata(false));



        /// <summary>
        /// Number of Ticks the whole Range [Minimum,Maximum] is divided evenly. Has to be >0
        /// </summary>
        public double TickAmount
        {
            get { return (double)GetValue(TickAmountProperty); }
            set { SetValue(TickAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TickAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickAmountProperty =
            DependencyProperty.Register("TickAmount", typeof(double), typeof(ProgressSlider), new PropertyMetadata(100.0), new ValidateValueCallback(TickAmount_IsValid));

        private static bool TickAmount_IsValid(object providedValue)
        {
            try
            {
                double d = (double)providedValue;
                return d > 0;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Gets or sets the Opacity of the Preview-ProgressBar
        /// </summary>
        public double PreviewOpacity
        {
            get { return (double)GetValue(PreviewOpacityProperty); }
            set { SetValue(PreviewOpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviewOpacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviewOpacityProperty =
            DependencyProperty.Register("PreviewOpacity", typeof(double), typeof(ProgressSlider), new PropertyMetadata(0.75));


        


        /// <summary>
        /// If true the Value-Property is shown in the ProgressSlider
        /// </summary>
        public bool DisplayValue
        {
            get { return (bool)GetValue(DisplayValueProperty); }
            set { SetValue(DisplayValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayValueProperty =
            DependencyProperty.Register("DisplayValue", typeof(bool), typeof(ProgressSlider), new PropertyMetadata(true));



        /// <summary>
        /// Converter that is used to Format the Value for displaying in the ProgressSlider. 
        /// Default-Value is an internal Percentage-Converter.
        /// If null, the actual Value will be displayed.
        /// 
        /// Implementation-Hint: ConvertBack is not used. Values are 
        /// [0]: value (double), 
        /// [1]: minimum (double), 
        /// [2]: maximum (double)
        /// </summary>
        public IMultiValueConverter ValueConverter
        {
            get { return (IMultiValueConverter)GetValue(ValueConverterProperty); }
            set { SetValue(ValueConverterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueConverter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueConverterProperty =
            DependencyProperty.Register("ValueConverter", typeof(IMultiValueConverter), typeof(ProgressSlider), new PropertyMetadata(new ValueToPercentConverter()));




        /// <summary>
        /// Geometry that is used to draw the Set-to-Minimum-Button
        /// </summary>
        public Geometry MinimumButtonGeometry
        {
            get { return (Geometry)GetValue(MinimumButtonGeometryProperty); }
            set { SetValue(MinimumButtonGeometryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimumButtonGeometry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumButtonGeometryProperty =
            DependencyProperty.Register("MinimumButtonGeometry", typeof(Geometry), typeof(ProgressSlider), new PropertyMetadata(Geometry.Parse("M8,0 L32,24 L56,0 L64,8 L40,32 L64,56 L56,64 L32,40 L8,64 L0,56 L24,32 L0,8z")));



        /// <summary>
        /// Geometry that is used to draw the Set-to-Maximum-Button
        /// </summary>
        public Geometry MaximumButtonGeometry
        {
            get { return (Geometry)GetValue(MaximumButtonGeometryProperty); }
            set { SetValue(MaximumButtonGeometryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaximumButtonGeometry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumButtonGeometryProperty =
            DependencyProperty.Register("MaximumButtonGeometry", typeof(Geometry), typeof(ProgressSlider), new PropertyMetadata(Geometry.Parse("M 24,64 L64,14 L54,4 L22,46 L8,34 L0,46 z")));




        #endregion


        public ProgressSlider()
        {
            InitializeComponent();
            Maximum = 100;
            Minimum = 0;
        }

        #region Event Handlers

        private void setToMin_Click(object sender, RoutedEventArgs e)
        {
            if (!IsReadOnly)
                Value = Minimum;
        }

        private void setToMax_Click(object sender, RoutedEventArgs e)
        {
            if (!IsReadOnly)
                Value = Maximum;
        }

        private void setToValue_Click(object sender, RoutedEventArgs e)
        {
            if (!IsReadOnly)
            {
                FrameworkElement el = e.OriginalSource as FrameworkElement;
                if (el != null)
                {
                    Value = CalculateValue(el.ActualWidth, Mouse.GetPosition(el).X);
                    PreviewValue = Value;
                }
            }
        }


        private void Slider_MouseEnter(object sender, MouseEventArgs e)
        {

            if (DisplayPreview && !IsReadOnly)
            {
                CurrentPreviewOpacity = PreviewOpacity;
                var vt = (FrameworkElement)Template.FindName("valueText", this);
                if (vt.Visibility != System.Windows.Visibility.Collapsed) 
                    vt.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void Slider_MouseLeave(object sender, MouseEventArgs e)
        {
            if (DisplayPreview && !IsReadOnly)
            {
                CurrentPreviewOpacity = 0.0;
                var vt = (FrameworkElement)Template.FindName("valueText", this);
                if (vt.Visibility != System.Windows.Visibility.Collapsed) //prevent the display if DisplayValue is set to false
                    vt.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void Slider_MouseMove(object sender, MouseEventArgs e)
        {
            if (DisplayPreview && !IsReadOnly)
            {
                FrameworkElement stvButton = (FrameworkElement)Template.FindName("setToValue", this);
                FrameworkElement stminButton = (FrameworkElement)Template.FindName("setToMin", this);
                FrameworkElement stmaxButton = (FrameworkElement)Template.FindName("setToMax", this);

                if (stvButton.IsMouseOver)
                {
                    PreviewValue = CalculateValue(stvButton.ActualWidth, e.GetPosition(stvButton).X);
                }
                if (stminButton.IsMouseOver)
                {
                    PreviewValue = Minimum;
                }
                if (stmaxButton.IsMouseOver)
                {
                    PreviewValue = Maximum;
                }
            }
        }


        private void RangeBase_StylusInRange(object sender, StylusEventArgs e)
        {
            Slider_MouseEnter(sender, null);
        }

        private void RangeBase_StylusOutOfRange(object sender, StylusEventArgs e)
        {
            Slider_MouseLeave(sender, null);
        }

        private void RangeBase_StylusInAirMove(object sender, StylusEventArgs e)
        {
            RangeBase_StylusMove(sender, e);
        }

        private void RangeBase_StylusMove(object sender, StylusEventArgs e)
        {
            if (DisplayPreview && !IsReadOnly)
            {
                FrameworkElement stvButton = (FrameworkElement)Template.FindName("setToValue", this);
                FrameworkElement stminButton = (FrameworkElement)Template.FindName("setToMin", this);
                FrameworkElement stmaxButton = (FrameworkElement)Template.FindName("setToMax", this);

                if (stvButton.IsStylusOver)
                {
                    PreviewValue = CalculateValue(stvButton.ActualWidth, e.GetPosition(stvButton).X);
                }
                if (stminButton.IsStylusOver)
                {
                    PreviewValue = Minimum;
                }
                if (stmaxButton.IsStylusOver)
                {
                    PreviewValue = Maximum;
                }
            }
        }

        #endregion

        /// <summary>
        /// Calculates the Value of the ProgressBar based on its size and pointing position
        /// </summary>
        /// <param name="max">reference size</param>
        /// <param name="pos">Position</param>
        /// <returns></returns>
        private double CalculateValue(double max, double pos)
        {
            if (!IsSnapToTickEnabled)
            {
                double v = pos / max;
                return Math.Min(Maximum, Math.Max(Minimum, (Maximum) * v + (1 - v) * Minimum));
            }
            else 
            {
                double v = pos / max; //x-position of mouse, relative to width [0,1]
                double step = 1 / TickAmount; //relative step size
                double stepValue = step * (Maximum - Minimum); //absolute step size

                double stepsOfValue = Math.Floor(v / step); //lowest fitting steps for position (relative) [0,step)               
                return stepValue * stepsOfValue + Minimum;
            }
        }




    }

    /// <summary>
    /// Converter that chosses the correct display format
    /// </summary>
    internal class PassThoughConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double value = (double)values[0];
            IMultiValueConverter c = (IMultiValueConverter)values[3];

            if (c != null)
                return c.Convert(new object[] { values[0], values[1], values[2] }, targetType, parameter, culture);

            return value.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Default-Converter
    /// </summary>
    internal class ValueToPercentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double value = (double)values[0];
            double min = (double)values[1];
            double max = (double)values[2];

            if (max - min == 0)
                return "0%";

            double p = (value - min) / (max - min) * 100;

            return string.Format("{0}%", Math.Round(p, 2));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class BrushVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double val = (double)values[0];
            Brush b = (Brush)values[1];
            double max = (double)values[2];
            Brush b2 = (Brush)values[3];

            if (val == max)
                return b;
            else
                return b2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class ValueToWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double val = (double)values[0];
            double min = (double)values[1];
            double max = (double)values[2];
            double width = (double)values[3];

            double p = (val - min) / (max - min);
            return p * width;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
