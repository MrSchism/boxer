using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boxer.Core;
using Boxer.Data;

namespace Boxer.ViewModel
{
    public class FolderViewVM : MainViewModel
    {
        private Folder _folder;

        public Folder Folder
        {
            get { return _folder; }
            set { Set(ref _folder, value); }
        }

        protected override void InitializeCommands()
        {

        }
    }
}
