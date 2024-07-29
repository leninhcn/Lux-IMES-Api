using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.ServiceCore.Model.Dto
{
    public class FaeTransfer
    {
    }
    /// <summary>
    /// SN 相关信息
    /// </summary>
    public class SNTInfo
    {
        public string SN { get; set; }
        public string WO { get; set; }
        public string LINE { get; set; }
        public string IPN { get; set; }
        public string MODEL { get; set; }
        public string ROUTE { get; set; }
        public string KOL { get; set; }

        public string TARTGET { get; set; }
        public string INPUT { get; set; }
        public string OUTPUT { get; set; }
        public string NEEDINPUT { get; set; }
        //public string IPN { get; set; }
        //public string MODEL { get; set; }
        //public string ROUTE { get; set; }

        public string KPSN { get; set; }
        public string TOOL { get; set; }
        public string GLUENO { get; set; }
        public string REELNO { get; set; }

    }

    public class faeTransInfo:SNTInfo
    { 
        public string outFromUserNo { get; set; }

        public string outToUserNo { get; set; }

        public string outToUserPhone { get; set; }  
        public string outLab { get; set; }
    }

    public class WOTInfo
    {
        private string wo = "";
        public string WO
        {
            get { return wo; }
            set { wo = value; }
        }

        private string target = "";
        public string TARTGET
        {
            get { return target; }
            set { target = value; }
        }

        private string input = "";
        public string INPUT
        {
            get { return input; }
            set { input = value; }
        }

        private string output = "";
        public string OUTPUT
        {
            get { return output; }
            set { output = value; }
        }

        private string needinput = "";
        public string NEEDINPUT
        {
            get { return needinput; }
            set { needinput = value; }
        }

        private string ipn = "";
        public string IPN
        {
            get { return ipn; }
            set { ipn = value; }
        }

        private string model = "";
        public string MODEL
        {
            get { return model; }
            set { model = value; }
        }

        private string route = "";
        public string ROUTE
        {
            get { return route; }
            set { route = value; }
        }
    }

    public class FAETransferInfo
    {
        public int ID { get; set; }
        public string DEFECTSTAGE { get; set; }
        public int PASSCOUNT { get; set; }
        public string SN { get; set; }
        public string Model { get; set; }
        public string IPN { get; set; }
        public string WORK_ORDER { get; set; }
        public string AREA { get; set; }
        public string LINE { get; set; }
        public int STATUS { get; set; }
        public DateTime OUTDATE { get; set; }
        public string OUTFROMUSERID { get; set; }
        public string OUTTOUSERID { get; set; }
        public string OUTTOUSERPHONE { get; set; }
        public DateTime BACKDATE { get; set; }
        public string BACKFROMUSERID { get; set; }
        public string BACKTOUSERID { get; set; }
        public string REPAIRAREA { get; set; }
        public DateTime UPDATE_TIME { get; set; }
        public string UPDATE_EMPNO { get; set; }
        public DateTime CREATE_TIME { get; set; }
        public string CREATE_EMPNO { get; set; }

        public string TRAN_STATIONTYPE { get; set; }
        public string LAB { get; set; }
        public decimal RECID { get; set; }
    }
}
