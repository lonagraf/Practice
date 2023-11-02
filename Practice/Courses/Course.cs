using System;

namespace Practice;

public class Course
{
    public int CourseID { get; set; }
    public string CourseName { get; set; }
    public string CourseDesc { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string Client { get; set; }
    public double Price { get; set; }
    public string LanguageStudy { get; set; }
}