using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GestionDeInventario.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionDeInventario.Pages.Usuarios
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Usuario")]
            public string UserName { get; set; } = string.Empty;

            [Required]
            [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = new ApplicationUser { UserName = Input.UserName, Email = Input.Email };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                SuccessMessage = "Usuario creado exitosamente.";
                ModelState.Clear();
                Input = new InputModel();
            }
            else
            {
                ErrorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
            }
            return Page();
        }
    }
} 