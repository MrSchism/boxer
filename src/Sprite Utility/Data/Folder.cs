using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Boxer.Core;
using Boxer.Properties;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Boxer.Data
{
    public sealed class Folder : NodeWithName
    {
        private ObservableCollection<Folder> _folders;
        
        [JsonProperty("folders")]
        public ObservableCollection<Folder> Folders { get { return _folders; } set { Set(ref _folders, value); } }

        private ObservableCollection<ImageData> _images;
        
        [JsonProperty("images")]
        public ObservableCollection<ImageData> Images { get { return _images; } set { Set(ref _images, value); } }

       [JsonIgnore]
        public override ObservableCollection<INode> Children
        {
            get
            {
                return _children; 
            }
            set
            {
                Set(ref _children, value);
            }
        }

        public Folder()
        {
            Folders = new ObservableCollection<Folder>();
            Images = new ObservableCollection<ImageData>();
            Children = new ObservableCollection<INode>();
            Children.CollectionChanged += ChildrenOnCollectionChanged;
            Name = "New Folder";
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in notifyCollectionChangedEventArgs.NewItems)
                {
                    if (item is ImageData)
                    {
                        Images.Add(item as ImageData);
                    }
                    if (item is Folder)
                    {
                        Folders.Add(item as Folder);
                    }
                }
            }
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in notifyCollectionChangedEventArgs.OldItems)
                {
                    if (item is ImageData)
                    {
                        Images.Remove(item as ImageData);
                    }
                    if (item is Folder)
                    {
                        Folders.Remove(item as Folder);
                    }
                }
            }
        }

        [JsonConstructor]
        public Folder(ObservableCollection<Folder> folders, ObservableCollection<ImageData> images) : this()
        {
            foreach (var folder in folders)
            {
                AddChild(folder);
            }
            foreach (var image in images)
            {
                AddChild(image);
            }
        }


        [JsonIgnore]
        public SmartCommand<object> NewFolderCommand { get; private set; }

        public bool CanExecuteNewFolderCommand(object o)
        {
            return true;
        }

        public void ExecuteNewFolderCommand(object o)
        {
            var folder = new Folder();
            AddChild(folder);
        }

        [JsonIgnore]
        public SmartCommand<object> NewImageCommand { get; private set; }

        public bool CanExecuteNewImageCommand(object o)
        {
            return true;
        }

        public void ExecuteNewImageCommand(object o)
        {
            var dialog = new OpenFileDialog {Filter = "Image Files (*.png, *.gif)|*.png;*.gif", Multiselect = true};
            var result = dialog.ShowDialog();
            if (result.Value)
            {
                foreach (var filename in dialog.FileNames)
                {
                    var image = new ImageData(filename);

                    if (Settings.Default.TrimToMinimalNonTransparentArea)
                    {
                        foreach (ImageFrame frame in image.Children)
                        {
                            var polyGroup = new PolygonGroup("Body");

                            frame.AddChild(polyGroup);

                            // Add an attack box stub
                            var attack = new Polygon();
                            attack.Name = "Attack";
                            polyGroup.AddChild(attack);

                            // Add a default foot box
                            var foot = new Polygon();
                            foot.Name = "Foot";

                            var bottom = frame.TrimRectangle.Bottom;
                            var left = frame.TrimRectangle.Left;
                            var top = frame.TrimRectangle.Top;
                            var right = frame.TrimRectangle.Right;
                            var width = frame.TrimRectangle.Width;
                            var height = frame.TrimRectangle.Height;

                            var tl = new PolyPoint(left + (int)(width * 0.25f), bottom - 2);
                            var tr = new PolyPoint(right - (int)(width * 0.25f), bottom - 2);
                            var br = new PolyPoint(right - (int)(width * 0.25f), bottom);
                            var bl = new PolyPoint(left + (int)(width * 0.25f), bottom);
                            foot.AddChild(tl);
                            foot.AddChild(tr);
                            foot.AddChild(br);
                            foot.AddChild(bl);

                            polyGroup.AddChild(foot);

                            // Set the center to best effort with no half-pixels
                            frame.CenterPointX = left + (int)(width * 0.5f);
                            frame.CenterPointY = top + (int)(height * 0.5f);
                        }
                    }

                    AddChild(image);
                }
            }
        }


        protected override void InitializeCommands()
        {
            NewFolderCommand = new SmartCommand<object>(ExecuteNewFolderCommand, CanExecuteNewFolderCommand);
            NewImageCommand = new SmartCommand<object>(ExecuteNewImageCommand, CanExecuteNewImageCommand);
            base.InitializeCommands();
        }
    }
}
