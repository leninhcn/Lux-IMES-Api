using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    public class Step
    {
        // <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "SEQ")]

        public string seq { get; set; }
        /// <summary>
        /// 启动
        /// </summary>
        [SugarColumn(ColumnName = "STEP")]
        public string step { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "TIME")]
        public string time { get; set; }
        /// <summary>
        /// 已完成
        /// </summary>
        [SugarColumn(ColumnName = "STATUS")]
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "DONE")]
        public bool done { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "COLOR")]
        public string color { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 

        [SugarColumn(ColumnName = "ADVICE")]
        public string advice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [SugarColumn(ColumnName = "EMP")]
        public string emp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [SugarColumn(ColumnName = "URL")]
        public string url { get; set; }
    }

    public class SubItems
    {
        /// <summary>
        /// 
        /// </summary>
        public string dictsort { get; set; }
        /// <summary>
        /// 装箱清单是否归档或确认
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 是
        /// </summary>
        public string result { get; set; }

    }

    public class StepInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        public int? id { get; set; }


        public string npino { get; set; }

        public string advice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool commit { get; set; }

        public string update_empno { get; set; }
        public string update_time { get; set; }

        public string step { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SubItems> subItems { get; set; }
    }


    public class OrderInfo
    {

        /// <summary>
        /// 
        /// </summary>
        /// 
        public int? id { get; set; }

        public string npino { get; set; }

        public string advice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool commit { get; set; }

        public string update_empno { get; set; }
        public string update_time { get; set; }

        public string step { get; set; }


        public List<Orders> orders { get; set; }


    }
    public class Orders
    {
        public string seq { get; set; }


        public string ipn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string plan_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lot { get; set; }

        public string work_order { get; set; }
        public string po { get; set; }

        public string actual_qty { get; set; }
    }

}
