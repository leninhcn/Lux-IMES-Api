using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class ResultInfo
    {
        public string Result { get; set; }
        public string Errcode { get; set; }
    }
    public class Resultinfo<T>
    {
        //值为OK或者IMESERR
        public string Result { get; set; } = "OK";
        //语言
        public string Lange { get; set; }
        //程式报错位置
        public string ErrCode { get; set; }
        //接口报错信息参数
        public string ResErrCodeParam { get; set; }
       // public string ResErrCode { get; set; } = "Success";
       //补充的报错信息
        public string ResMSG { get; set; }
        public string ClientType { get; set; } = "Backend";
        public string GetResMSG => Result == "OK"? $"Success{ResMSG}":$"{Result}_{ResErrCodeParam}";

        public T Data { get; set; }

        public Resultinfo()
        {
        }

        public Resultinfo(T value)
        {
            Data = value;
        }
    }
}
