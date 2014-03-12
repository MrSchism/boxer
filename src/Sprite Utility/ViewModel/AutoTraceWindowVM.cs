using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boxer.Core;
using GalaSoft.MvvmLight.Messaging;

namespace Boxer.ViewModel
{

    public class CloseAutoTraceWindowMessage
    {
        
    }
    public class AutoTraceWindowVM : MainViewModel
    {
        public bool IsOk
        {
            get; set;
        }

        private bool _holeDetection;

        public bool HoleDetection
        {
            get { return _holeDetection; }
            set { Set(ref _holeDetection, value); }
        }

        private bool _multipartDetection;

        public bool MultipartDetection
        {
            get { return _multipartDetection; }
            set { Set(ref _multipartDetection, value); }
        }

        private float _hullTolerence;

        public float HullTolerence
        {
            get { return _hullTolerence; }
            set { Set(ref _hullTolerence, value); }
        }

        private byte _alphaTolerence;

        public byte AlphaTolerence
        {
            get { return _alphaTolerence; }
            set { Set(ref _alphaTolerence, value); }
        }

        public SmartCommand<object> OkCommand { get; private set; }

        public bool CanExecuteOkCommand(object o)
        {
            return true;
        }

        public void ExecuteOkCommand(object o)
        {
            Messenger.Default.Send(new CloseAutoTraceWindowMessage());
        }

        protected override void InitializeCommands()
        {
            OkCommand = new SmartCommand<object>(ExecuteOkCommand, CanExecuteOkCommand);
        }
    }
}
