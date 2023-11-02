using Org.BouncyCastle.Asn1.Cms;

namespace Practice.Groups;

public class Group
{
    public int GroupID { get; set; }
    public string GroupName { get; set; }
    public string Teacher { get; set; }
    public int MaxStudentAmount { get; set; }
    public string Course { get; set; }
}