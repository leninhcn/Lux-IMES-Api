using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Infrastructure.Enums
{
    /// <summary>
    /// 语言类型 0
    /// </summary>
    public enum LanguageType
    {
        /// <summary>
        /// 中文简体
        /// </summary>
        [Description("中文简体")]
        zh_CN = 0,
        /// <summary>
        /// 中文繁體
        /// </summary>
        [Description("中文繁體")]
        zh_TW = 1,

        /// <summary>
        /// 英语（美式）
        /// </summary>
        [Description("English")]
        en_US = 2,
        /// <summary>
        /// 越南语
        /// </summary>
        [Description("Tiếng Việt")]
        vi_VN = 3,
        /// <summary>
        /// 西班牙语（墨西哥）
        /// </summary>
        [Description("México")]
        es_MX = 4,

    }
}
