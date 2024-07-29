using SqlSugar;

namespace ZR.Model.System
{
    [SugarTable("SAJET.m_zr_post", "岗位表")]
    [Tenant("0")]
    public class SysPost : SysBase
    {
        /// <summary>
        /// 岗位Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true/*, IsIdentity = true*/, OracleSequenceName = "SAJET.zr_sys_post_id")]
        public long PostId { get; set; }
        [SugarColumn(ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string PostCode { get; set; }
        [SugarColumn(ExtendedAttribute = ProteryConstant.NOTNULL)]
        public string PostName { get; set; }
        [SugarColumn(ExtendedAttribute = ProteryConstant.NOTNULL)]
        public int PostSort { get; set; }
        [SugarColumn(Length = 1)]
        public string Status { get; set; }
    }
}
