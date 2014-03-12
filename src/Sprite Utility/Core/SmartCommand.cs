using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Boxer.Core
{
    public interface ISmartCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }

    public class SmartCommand<T> : RelayCommand<T>, ISmartCommand, IDisposable
    {
        public SmartCommand(Action<T> executeMethod)
            : base(executeMethod)
        {
            CommandManager.Register(this);
        }

        public SmartCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        {
            if (canExecuteMethod != null)
                CommandManager.Register(this);
        }

        public void Dispose()
        {
            CommandManager.Unregister(this);
        }
    }
}
