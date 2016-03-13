using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace rwpf.Helpers
{
    /// <summary>
    /// Containing AttachedProperties for ToggleButton and subclasses.
    /// 
    /// OriginalValue - is an attached property to highlight changed values in ToggleButtons, CheckBoxes, RadioButtons and every other ToggleButton-based control
    /// </summary>
    public class ToggleHelper : FrameworkElement
    {
        /// <summary>
        /// Gets the original boolean value
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool? GetOriginalValue(DependencyObject obj)
        {
            return (bool?)obj.GetValue(OriginalValueProperty);
        }

        /// <summary>
        /// Sets the original boolean value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetOriginalValue(DependencyObject obj, bool? value)
        {
            obj.SetValue(OriginalValueProperty, value);
        }

        /// <summary>
        /// Property of OriginalValue
        /// </summary>
        public static readonly DependencyProperty OriginalValueProperty = DependencyProperty.RegisterAttached("OriginalValue", typeof(bool?), typeof(ToggleHelper),
            new PropertyMetadata(null, new PropertyChangedCallback(OriginalValueChanged)));

        /// <summary>
        /// Registeres needed events to the ToogleButton
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OriginalValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleButton cb = d as ToggleButton;
            if (cb != null)
            {
                if (e.NewValue != null) //the property should be used, register checked/unchecked events to react on changes
                {
                    cb.Checked += cb_Checked;
                    cb.Unchecked += cb_Checked;
                    cb_Checked(cb, null);
                }
                else //deregister
                {
                    cb.Checked -= cb_Checked;
                    cb.Unchecked -= cb_Checked;
                    cb.FontWeight = FontWeights.Normal;
                }
            }

        }

        /// <summary>
        /// Performs the reaction of changed values. Sets the FontWeight to Bold if Values missmatch with OriginalValue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void cb_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton cb = sender as ToggleButton;
            if (cb != null)
            {
                if (cb.IsChecked.HasValue && GetOriginalValue(cb) != null)
                {
                    if (cb.IsChecked.Value != GetOriginalValue(cb))
                        cb.FontWeight = FontWeights.Bold;
                    else
                        cb.FontWeight = FontWeights.Normal;
                }
            }
        }
    }
}
