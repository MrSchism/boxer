using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boxer.Core;
using Xceed.Wpf.Toolkit;

namespace Boxer.ViewModel
{
    public class PreferencesVM : MainViewModel
    {

        public SmartCommand<object> SaveCommand { get; private set; }

        public bool CanExecuteSaveCommand(object o)
        {
            return true;
        }

        public void ExecuteSaveCommand(object o)
        {
            Properties.Settings.Default.Save();
            MessageBox.Show("Settings has been saved!");
        }

        protected override void InitializeCommands()
        {
            SaveCommand = new SmartCommand<object>(ExecuteSaveCommand, CanExecuteSaveCommand);
        }
    }
}
