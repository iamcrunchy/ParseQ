using System.ComponentModel.DataAnnotations;

namespace ParseQ.Models;

public class IndexModel
{
    [Required]
    public IFormFile? File { get; set; }
}