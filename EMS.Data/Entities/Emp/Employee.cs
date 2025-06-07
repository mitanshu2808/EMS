using System.ComponentModel.DataAnnotations;
using EMS.Data;
using EMS.Data.Entities.Tasks;
using Microsoft.AspNetCore.Identity;

public class Employee : IdentityUser
{
    //internal string address;
    public string Firstname { get; set; } = string.Empty;   
    public string Lastname { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime DateofBirth { get; set; }
    public DateTime DateofJoin { get; set; }
    public string AadharNumber { get; set; } = string.Empty;
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate{ get; set; }
    public string? UpdatedBy { get; set; }
    public ICollection<TaskModel>? Tasks { get; set; }
}
