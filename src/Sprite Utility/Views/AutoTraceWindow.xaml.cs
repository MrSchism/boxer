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
using System.Windows.Shapes;
using Boxer.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace Boxer.Views
{
    /// <summary>
    /// Interaction logic for AutoTraceWindow.xaml
    /// </summary>
    public partial class AutoTraceWindow : Window
    {
        private bool _properClose;

        public AutoTraceWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<CloseAutoTraceWindowMessage>(this, p =>
            {
                _properClose = true; 
                Close(); });

            Closed += AutoTraceWindow_Closed;
        }

        void AutoTraceWindow_Closed(object sender, EventArgs e)
        {
            if (_properClose)
            {
                (this.DataContext as AutoTraceWindowVM).IsOk = true;
            }
            else
            {
                (this.DataContext as AutoTraceWindowVM).IsOk = false;
            }
        }
    }
}
