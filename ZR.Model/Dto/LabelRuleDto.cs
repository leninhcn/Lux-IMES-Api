using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Dto
{
    public class LabelRuleDto
    {
        public class Query
        {
            [BindRequired]
            public string Status { get; set; }

            [BindRequired]
            public string RuleField { get; set; }

            [BindRequired]
            public string FilterText { get; set; }
        }
    }
}
