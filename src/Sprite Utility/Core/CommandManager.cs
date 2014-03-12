using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Core
{
    public class CommandManager
    {
        public static List<ISmartCommand> Commands { get; private set; }

        static CommandManager()
        {
            Commands = new List<ISmartCommand>();
        }

        public static void InvalidateRequerySuggested()
        {
            foreach (var c in Commands)
            {
                c.RaiseCanExecuteChanged();
            }

        }

        public static void Register(ISmartCommand command)
        {
            if (!Commands.Contains(command))
                Commands.Add(command);
        }

        public static void Unregister(ISmartCommand command)
        {
            if (Commands.Contains(command))
                Commands.Remove(command);
        }
    }
}
