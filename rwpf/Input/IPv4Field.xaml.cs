using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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
    /// Control for a IPv4 Address
    /// </summary>
    public partial class IPv4Field : UserControl
    {
        public IPv4Field()
        {
            InitializeComponent();
       
            DataObject.AddPastingHandler(octett1, PasteHandler);
            DataObject.AddPastingHandler(octett2, PasteHandler);
            DataObject.AddPastingHandler(octett3, PasteHandler);
            DataObject.AddPastingHandler(octett4, PasteHandler);
        }
            

        #region Custom DependencyProperties

        /// <summary>
        /// Gets or sets the IP-Address that is shown by the control
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(rwpf.TypeConverters.IPAddressConverter))]
        public IPAddress IP
        {
            get { return (IPAddress)GetValue(IPProperty); }
            set { SetValue(IPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IP.  This enables animation, styling, binding, etc...
        [System.ComponentModel.TypeConverter(typeof(rwpf.TypeConverters.IPAddressConverter))]        
        public static readonly DependencyProperty IPProperty =
            DependencyProperty.Register("IP", typeof(IPAddress), typeof(IPv4Field), new PropertyMetadata(null, new PropertyChangedCallback(IPChanged)));

        

        /// <summary>
        /// Gets or sets the first octett of the IP-Address
        /// </summary>
        public byte FirstOctett
        {
            get { return (byte)GetValue(FirstOctettProperty); }
            set { SetValue(FirstOctettProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstOctett.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstOctettProperty =
            DependencyProperty.Register("FirstOctett", typeof(byte), typeof(IPv4Field), new PropertyMetadata((byte)0, new PropertyChangedCallback(OctettChanged)));



        /// <summary>
        /// Gets or sets the seconds octett of the IP-Address
        /// </summary>
        public byte SecondOctett
        {
            get { return (byte)GetValue(SecondOctettProperty); }
            set { SetValue(SecondOctettProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SecondOctett.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondOctettProperty =
            DependencyProperty.Register("SecondOctett", typeof(byte), typeof(IPv4Field), new PropertyMetadata((byte)0, new PropertyChangedCallback(OctettChanged)));



        /// <summary>
        /// Gets or sets the third octett of the IP-Address
        /// </summary>
        public byte ThirdOctett
        {
            get { return (byte)GetValue(ThirdOctettProperty); }
            set { SetValue(ThirdOctettProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThirdOctett.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThirdOctettProperty =
            DependencyProperty.Register("ThirdOctett", typeof(byte), typeof(IPv4Field), new PropertyMetadata((byte)0, new PropertyChangedCallback(OctettChanged)));



        /// <summary>
        /// Gets or sets the fourth octett of the IP-Address
        /// </summary>
        public byte FourthOctett
        {
            get { return (byte)GetValue(FourthOctettProperty); }
            set { SetValue(FourthOctettProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FourthOctett.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FourthOctettProperty =
            DependencyProperty.Register("FourthOctett", typeof(byte), typeof(IPv4Field), new PropertyMetadata((byte)0, new PropertyChangedCallback(OctettChanged)));




        /// <summary>
        /// Gets or sets a value that indicates that the control only allows valid subnet masks
        /// </summary>
        public bool NetmaskOnly
        {
            get { return (bool)GetValue(NetmaskOnlyProperty); }
            set { SetValue(NetmaskOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NetmaskOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NetmaskOnlyProperty =
            DependencyProperty.Register("NetmaskOnly", typeof(bool), typeof(IPv4Field), new PropertyMetadata(false));


        #endregion

        #region Handlers and Callbacks

        private void PasteHandler(object sender, DataObjectPastingEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pasteText = e.DataObject.GetData(typeof(string)) as string;

                IPAddress addr;
                if (pasteText.Contains('.') && IPAddress.TryParse(pasteText, out addr))
                {
                    IP = addr;
                    e.CancelCommand();
                }

                string preview = tb.Text.Insert(tb.SelectionStart, pasteText);
                int n;
                if (int.TryParse(preview, out n) && n >= 0 && n <= 255)
                {

                }
                else
                    e.CancelCommand();
            }
        }

        private bool suppressIPUpdates = false;

        /// <summary>
        /// Gets called when the IP-Property has been changed. This callback will update the Octetts
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private static void IPChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            IPv4Field box = o as IPv4Field;    
            IPAddress ip = (IPAddress)e.NewValue;

            if(ip!=null && e.NewValue!=e.OldValue && box.NetmaskOnly)
            {
                byte[] octetts = ip.GetAddressBytes();
                if(!IsNetmask(octetts[0],octetts[1],octetts[2],octetts[3]))
                {
                    box.IP = (IPAddress)e.OldValue;
                    return;
                }
            }
            
            if(ip!=null)
            {
                byte[] octetts = ip.GetAddressBytes();                
                box.FirstOctett = octetts[0];
                box.SecondOctett = octetts[1];
                box.ThirdOctett = octetts[2];
                box.FourthOctett = octetts[3];                
            }            
        }

        /// <summary>
        /// Gets called when an octett has been changed. This callback will update the IP-Property
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
         private static void OctettChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            IPv4Field box = o as IPv4Field;
            
             //if only netmasks allowed and the ip can´t be a netmask
             if(box.NetmaskOnly && !IsNetmask(box.FirstOctett,box.SecondOctett,box.ThirdOctett,box.FourthOctett))
             {
                 if (e.NewValue != e.OldValue)
                 {
                     //revert and cancel
                     switch (e.Property.Name)
                     {
                         case "FirstOctett": box.FirstOctett = (byte)e.OldValue; break;
                         case "SecondOctett": box.SecondOctett = (byte)e.OldValue; break;
                         case "ThirdOctett": box.ThirdOctett = (byte)e.OldValue; break;
                         case "FourthOctett": box.FourthOctett = (byte)e.OldValue; break;
                     }
                 }
                 return;
             }
            
             //update only if the change was performed by this method (stack overflow protection)
             if(!box.suppressIPUpdates)
             {
                 IPAddress ip = new IPAddress(new byte[] { box.FirstOctett, box.SecondOctett, box.ThirdOctett, box.FourthOctett });
                 box.suppressIPUpdates = true;
                 box.IP = ip;
                 box.suppressIPUpdates = false;
             }
            
        }


         private void octett_KeyDown(object sender, KeyEventArgs e)
         {
             //ignore space. not handled by PreviewTextInput
             if (e.Key == Key.Space)
                 e.Handled = true;

             TextBox box = sender as TextBox;

             //copy IP
             if (e.Key == Key.C && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
             {
                 Clipboard.SetText(IP.ToString());
                 e.Handled = true;
                 return;
             }

             //move to previous octett
             if (box != null && ((e.Key == Key.Back && box.Text.Length <= 0) || (e.Key == Key.Left && box.SelectionStart == 0) || (e.Key == Key.Tab && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))))
             {
                 if (box == octett4)
                 {
                     MoveCaretToOctett(octett3);
                     e.Handled = true;
                     if (e.Key == Key.Tab)
                         octett3.SelectAll();
                 }
                 if (box == octett3)
                 {
                     MoveCaretToOctett(octett2);
                     e.Handled = true;
                     if (e.Key == Key.Tab)
                         octett2.SelectAll();
                 }
                 if (box == octett2)
                 {
                     MoveCaretToOctett(octett1);
                     e.Handled = true;
                     if (e.Key == Key.Tab)
                         octett1.SelectAll();
                 }
                 return;
             }

             //move to next octett
             if (box != null && ((box.Text.Length == 3 && box.SelectionLength == 0 && e.Key != Key.Left && e.Key != Key.Back && e.Key != Key.Delete) || e.Key == Key.Tab || (e.Key == Key.Right && box.SelectionStart == box.Text.Length)))
             {
                 TextBox b = box;
                 if (box == octett1)
                 {
                     if (!(box.Text.Length == 3 && box.SelectionLength == 0))
                     {
                         MoveCaretToOctett(octett2, e.Key == Key.Right);
                         b = octett2;
                         e.Handled = true;
                         b.SelectAll();
                     }
                     else
                     {
                         MoveCaretToOctett(octett2, e.Key == Key.Right);
                         b = octett2;
                     }
                 }
                 if (box == octett2)
                 {
                     if (!(box.Text.Length == 3 && box.SelectionLength == 0))
                     {
                         MoveCaretToOctett(octett3, e.Key == Key.Right);
                         b = octett3;
                         e.Handled = true;
                         b.SelectAll();
                     }
                     else
                     {
                         MoveCaretToOctett(octett3, e.Key == Key.Right);
                         b = octett3;
                     }
                 }
                 if (box == octett3)
                 {
                     if (!(box.Text.Length == 3 && box.SelectionLength == 0))
                     {
                         MoveCaretToOctett(octett4, e.Key == Key.Right);
                         b = octett4;
                         e.Handled = true;
                         b.SelectAll();
                     }
                     else
                     {
                         MoveCaretToOctett(octett4, e.Key == Key.Right);
                         b = octett4;
                     }
                 }

                 //unselect when leaving the control by using Tab
                 box.SelectionLength = 0;

                 return;
             }

         }


         /// <summary>
         /// Processes Text Input
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void octett_PreviewTextInput(object sender, TextCompositionEventArgs e)
         {
             TextBox box = sender as TextBox;

             //adding text is completely handled by this method
             e.Handled = true;

             //a digit was entered
             if (box != null && Regex.IsMatch(e.Text, @"\d{1,1}"))
             {
                 string s = box.Text.Remove(box.SelectionStart, box.SelectionLength).Insert(box.SelectionStart, e.Text);
                 int result;
                 if (int.TryParse(s, out result) && result >= 0 && result <= 255) //valid byte number
                 {
                     box.Text = result.ToString(); //set text
                     box.SelectionStart = box.Text.Length;
                 }
                 return;
             }


             //. entered, move to next octett
             if (e.Text == ".")
             {
                 if (box == octett1)
                 {
                     MoveCaretToOctett(octett2);
                     octett2.SelectAll();
                     return;
                 }
                 if (box == octett2)
                 {
                     MoveCaretToOctett(octett3);
                     octett3.SelectAll();
                     return;
                 }
                 if (box == octett3)
                 {
                     MoveCaretToOctett(octett4);
                     octett4.SelectAll();
                     return;
                 }
             }
         }

        #endregion

        #region Helpers

         private static readonly List<byte> netmaskvalues = new List<byte>() { 0,128,192,224,240,248,252,254,255 };
        /// <summary>
        /// Checks if IPv4 Address is a valid netmask
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <param name="o3"></param>
        /// <param name="o4"></param>
        /// <returns></returns>
        protected static bool IsNetmask(byte o1, byte o2, byte o3, byte o4)
         {
            int maxindex = 8;
            int current =8;

            current = netmaskvalues.IndexOf(o1);
            if (current < 0 || current > maxindex) 
                return false;
            maxindex = current!=8 ? 0 : current;

            current = netmaskvalues.IndexOf(o2);
            if (current < 0 || current > maxindex)
                return false;
            maxindex = current != 8 ? 0 : current;

            current = netmaskvalues.IndexOf(o3);
            if (current < 0 || current > maxindex)
                return false;
            maxindex = current != 8 ? 0 : current;

            current = netmaskvalues.IndexOf(o4);
            if (current < 0 || current > maxindex)
                return false;

            return true;
         }

        private void MoveCaretToOctett(TextBox octett, bool start=false)
        {
            Keyboard.Focus(octett);
            if (!start)
            {
                octett.SelectionStart = octett.Text.Length;
                octett.SelectionLength = 0;
            }
        }

        #endregion


    }
}
