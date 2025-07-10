using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionDeInventario.Pages.Usuarios
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
} 