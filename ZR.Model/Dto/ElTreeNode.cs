using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class ElTreeNode
    {
        public string Label { get; set; }

        public List<ElTreeNode> Children { get; set; }

        public int IconIndex { get; set; }

        public bool Disabled { get; set; }

        public ElTreeNode AddChild(ElTreeNode child)
        {
            Children ??= new List<ElTreeNode>();
            Children.Add(child); return child;
        }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public ElTreeNode Last => Children?.Last();
    }
}
