using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
    public string UserId { get; set; }
    public string Token { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; }
}