
namespace ZR.Model.Business
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("SAJET.P_WO_CUTTING")]
    public class PWoCutting
    {
        /// <summary>
        /// Id 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// ������ 
        /// </summary>
        [SugarColumn(ColumnName = "wORK_ORDER")]
        public string WorkOrder { get; set; }

        /// <summary>
        /// ���� 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// �������� 
        /// </summary>
        [SugarColumn(ColumnName = "tARGET_QTY")]
        public decimal TargetQty { get; set; }

        /// <summary>
        /// BOMת������ 
        /// </summary>
        [SugarColumn(ColumnName = "bOM_RATIO")]
        public decimal BomRatio { get; set; }

        /// <summary>
        /// ����ת����Ʒ�� 
        /// </summary>
        [SugarColumn(ColumnName = "mAX_QTY")]
        public decimal MaxQty { get; set; }

        /// <summary>
        /// ����ת����Ʒ�� 
        /// </summary>
        [SugarColumn(ColumnName = "IPN")]
        public string Ipn { get; set; }

        /// <summary>
        /// UpdateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_EMPNO")]
        public string UpdateEmpno { get; set; }

        /// <summary>
        /// ����ʵ��ת������ 
        /// </summary>
        [SugarColumn(ColumnName = "oUTPUT_QTY")]
        public double? OutputQty { get; set; }

        /// <summary>
        /// �Ƿ��ѹر� Y/N 
        /// </summary>
        public string Closed { get; set; }

        /// <summary>
        /// UpdateTime 
        /// </summary>
        [SugarColumn(ColumnName = "uPDATE_TIME")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// CreateEmpno 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_EMPNO")]
        public string CreateEmpno { get; set; }

        /// <summary>
        /// CreateTime 
        /// </summary>
        [SugarColumn(ColumnName = "cREATE_TIME")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// ����ת������� 
        /// </summary>
        [SugarColumn(ColumnName = "mAX_SHELF_QTY")]
        public decimal MaxShelfQty { get; set; }

    }
}