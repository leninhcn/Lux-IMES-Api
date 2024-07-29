using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto.ProdDto
{
    [SugarTable("SAJET.p_material", "碑文验证表")]
    public class ImesVerification
    {
        [SugarColumn(ColumnName = "RT_ID", IsPrimaryKey = true)]
        public int rtId { get; set; }
        [SugarColumn(ColumnName = "IPN", IsPrimaryKey = true)]
        public string ipn { get; set; }
        public string datecode { get; set; }

        public string materialNo { get; set; }
        public int materialQty { get; set; }
        [SugarColumn(ColumnName = "REEL_NO", IsPrimaryKey = true)]
        public string reelNo { get; set; }
        public int reelQtY { get; set; }
        public string status { get; set; }
        public int locateId { get; set; }
        public string warehouseId { get; set; }
        public string updateUserid { get; set; }
        [SugarColumn(ColumnName = "UPDATE_TIME", IsPrimaryKey = true)]
        public DateTime? updateTime{ get; set; }  
        public string remake { get; set;}
        public string version { get; set; }    
        public int releaseQty { get; set;}
        public int keepQty { get; set;}
        public string vendor {  get; set; }
        public string lot {  get; set; }   
        public DateTime? rtReceivetime{ get; set; }
        public string reelMemo {  get; set; }
        public int rtSeq {  get; set; }
        public DateTime? inventoryTime { get; set;}
        public DateTime? createTime {  get; set; }
        public int inventoryEmpid { get; set; }

        public string datecodeYw { get; set; }
        public string whUsealtime { get; set; }
        public string expTime { get; set;}
        public string lotNo { get;set; }
        public string expDatecode { get; set; }  
        public string workId {  get; set; } 
        public string sqeflag { get; set; }
        public string parentReel { get; set; }
        public int splitQty {  get; set; }
        public string lcrscope { get; set; }
        public string lcrcontent { get; set; }
        public string lcrtime { get; set; }
        public string lcrvalue { get; set; }
        [SugarColumn(ColumnName = "PLANT", IsPrimaryKey = true)]
        public string plant { get; set; }
        [SugarColumn(ColumnName = "SITE", IsPrimaryKey = true)]
        public string site { get; set; }
        [SugarColumn(ColumnName = "INSCRIPTION_VERIFICATION", IsPrimaryKey = true)]
        public string inscriptionVerification {  get; set; }
        
    }
}
