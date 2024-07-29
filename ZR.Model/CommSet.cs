using System.Collections.Generic;

namespace ZR.Model
{
    /// <summary>
    /// 其它参数
    /// </summary>
    public class CommSet
    {
        [SugarColumn(IsIgnore = true)]
        public string Lang { get; set; }
    }
}
