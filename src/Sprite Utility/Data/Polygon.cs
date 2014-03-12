using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Boxer.Data
{
    public sealed class Polygon : NodeWithName
    {
        [JsonProperty("points")]
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

        public Polygon()
        {
            Name = "New Polygon";
            Children = new ObservableCollection<INode>();
        }

        [JsonConstructor]
        public Polygon(ObservableCollection<PolyPoint> points)
            : this()
        {
            foreach (var point in points)
            {
                AddChild(point);
            }
        }
    }
}
