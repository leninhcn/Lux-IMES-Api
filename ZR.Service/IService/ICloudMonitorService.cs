using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model;
using ZR.Model.Business;
using ZR.Model.Dto;

namespace ZR.Service.IService
{
    public interface ICloudMonitorService
    {
        string AddTicket(PTicketStatus model);
        Resultinfo<string> AssignTicket(PTicketStatus model);
        List<PCollecterrorLog> GetInfo(string ID);
        PagedInfo<PTicketStatus> GetLsit(PTicketStatusQueryDto param);
        Resultinfo<List<PTicketReportRes>> GetReport(PTicketReportQueryDto param);
        List<PTicketTravel> GetTravel(string ID);
        Resultinfo<string> UpdateTicket(PTicketStatus model);
        Resultinfo<string> UpdateTicketStatus(PTicketStatus model);
    }
}
