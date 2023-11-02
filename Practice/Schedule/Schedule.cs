using System;
using Org.BouncyCastle.Asn1.Cms;

namespace Practice.Schedule;

public class Schedule
{
    public int ScheduleID { get; set; }
    public string Weekday { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public string Group { get; set; }
}