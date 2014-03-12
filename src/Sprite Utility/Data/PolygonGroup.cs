using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boxer.Core;
using Newtonsoft.Json;

namespace Boxer.Data
{
    public sealed class PolygonGroup : NodeWithName
    {
        [JsonProperty("polygons")]
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

        public PolygonGroup(string name = "New Polygon Group")
        {
            Name = name;
            Children = new ObservableCollection<INode>();
        }

        [JsonConstructor]
        public PolygonGroup(ObservableCollection<Polygon> polygons)
            : this()
        {
            foreach (var polygon in polygons)
            {
                AddChild(polygon);
            }
        }

        [JsonIgnore]
        public SmartCommand<object> NewPolygonCommand { get; private set; }

        public bool CanExecuteNewPolygonCommand(object o)
        {
            return true;
        }

        public void ExecuteNewPolygonCommand(object o)
        {
            var polygonGroup = new Polygon();
            AddChild(polygonGroup);
        }

        protected override void InitializeCommands()
        {
            NewPolygonCommand = new SmartCommand<object>(ExecuteNewPolygonCommand, CanExecuteNewPolygonCommand);
            base.InitializeCommands();
        }
    }
}
