using System.ComponentModel.DataAnnotations;

namespace API.Entities;
public class AppUser
{
    [Key]   
     public int Id { get; private set; }
    public required string UserName { get; set; }
}