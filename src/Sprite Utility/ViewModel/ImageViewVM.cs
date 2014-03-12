using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boxer.Core;
using Boxer.Data;

namespace Boxer.ViewModel
{
    public class ImageViewVM : MainViewModel
    {
        private ImageData _image;

        public ImageData Image
        {
            get { return _image; }
            set { Set(ref _image, value); }
        }

        protected override void InitializeCommands()
        {
            
        }
    }
}
