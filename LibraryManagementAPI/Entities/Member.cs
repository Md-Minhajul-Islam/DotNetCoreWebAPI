using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Entities;

public class Member
{
    public int Id {get; set;}
    public string FullName {get; set;} = string.Empty;
    [EmailAddress]
    public string Email {get; set;} = string.Empty;
    public bool IsActive {get; set;} = true;

    public ICollection<Loan> Loans {get; set;} = new List<Loan>();
}