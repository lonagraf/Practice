using System;

namespace Practice.Groups;

public class Attendance
{
    public int AttendanceID { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Client { get; set; }
    public string AttendanceStatus { get; set; }
}