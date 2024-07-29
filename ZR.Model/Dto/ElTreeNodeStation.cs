using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class ElTreeNodeStation:ElTreeNode
    {   
        public string ClientType { get; set; }

        public string StationType { get; set; }

        public new List<ElTreeNodeStation> Children { get; set; }
        public ElTreeNodeStation AddChild(ElTreeNodeStation child)
        {
            Children ??= new List<ElTreeNodeStation>();
            Children.Add(child); return child;
        }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public new ElTreeNodeStation Last => Children?.Last();
    }
    public class ElTreeNodeWobom : ElTreeNode
    {
        public string ID { get; set; }
        public string StationType { get; set; }
        public long? ItemCount { get; set; }
        public string ItemGroup { get; set; }
        public string Version { get; set; }
        public string PartType { get; set; }
        public string SPEC1 { get; set; }
        public new List<ElTreeNodeWobom> Children { get; set; }
        public ElTreeNodeWobom AddChild(ElTreeNodeWobom child)
        {
            Children ??= new List<ElTreeNodeWobom>();
            Children.Add(child); return child;
        }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public new ElTreeNodeWobom Last => Children?.Last();
    }
}
