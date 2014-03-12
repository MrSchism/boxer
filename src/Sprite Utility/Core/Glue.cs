﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boxer.Data;

namespace Boxer.Core
{
    public class Glue : MainViewModel
    {
        private bool _documentIsSaved;
        public bool DocumentIsSaved
        {
            get { return _documentIsSaved; }
            set
            {
                Set(ref _documentIsSaved, value);
            }
        }

        private Document _document;

        public Document Document
        {
            get { return _document; }
            set { Set(ref _document, value); }
        }

        protected override void InitializeCommands()
        {
        }
    }
}
