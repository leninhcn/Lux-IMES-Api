
namespace ZR.Model.Business
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("SAJET.M_BILL_MATERIAL")]
    public class MBillMaterial
    {
        /// <summary>
        /// ItemVersion 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_VERSION")]
        public string ItemVersion { get; set; }

        /// <summary>
        /// StationType 
        /// </summary>
        [SugarColumn(ColumnName = "sTATION_TYPE")]
        public string StationType { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Id 
        /// </summary>
       // [SugarColumn(IsPrimaryKey = true, ColumnName = "ID", OracleSequenceName = "SAJET.ID", ColumnDescription = "Ö÷¼ü")]
        public string Id { get; set; }

        /// <summary>
        /// ItemCount 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_COUNT")]
        public long? ItemCount { get; set; }

        /// <summary>
        /// Ipn 
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// SPEC1 
        /// </summary>
        public string SPEC1 { get; set; }

        /// <summary>
        /// Version 
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// ItemIpn 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_IPN")]
        public string ItemIpn { get; set; }

        /// <summary>
        /// ItemSpec1 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_SPEC1")]
        public string ItemSpec1 { get; set; }

        /// <summary>
        /// ItemGroup 
        /// </summary>
        [SugarColumn(ColumnName = "iTEM_GROUP")]
        public string ItemGroup { get; set; }

        /// <summary>
        /// Site 
        /// </summary>
        public string Site { get; set; }

    }
}