using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class StationAction
    {
        public string GroupId { get; set; }
        public int Step { get; set; }
        public int JobSeq { get; set; }

        string valueKind;

        public string ValueKind
        {
            get => valueKind;
            set
            {
                valueKind = GetValueKind(value);
            }
        }

        string valueTransFormation;
        public string ValueTransFormation
        {
            get => valueTransFormation;
            set
            {
                valueTransFormation = GetValueTransFormation(value);
            }
        }
        public int LoopCount { get; set; }
        public string JobName { get; set; }
        public string JobLogicProSql { get; set; }
        public string LogicType { get; set; }
        public string InputParam { get; set; }
        public string OutputParam { get; set; }
        public string TypeName { get; set; }
        public string TypeProcParam { get; set; }
        public bool ShowBom { get; set; }
        public int InputCount { get; set; }
        public bool CheckLine { get; set; }
        public bool PrintFlag { get; set; }
        public string GroupName { get; set; }
        public bool AutoReadSn { get; set; }
        public string AutoReadPath { get; set; }
        public bool CheckFont { get; set; }
        public string TypeDesc { get; set; }
        public int PrintQty { get; set; }

        static string GetValueKind(string valuekind)
        => valuekind switch
        {
            "0" => "N",
            "1" => "O",
            "2" => "M",
            "3" => "L",
            _ => valuekind,
        };

        static string GetValueTransFormation(string valuetransformation)
        => valuetransformation switch
        {
            "0" => "N",
            "1" => "U",
            "2" => "L",
            _ => valuetransformation,
        };
    }
}
