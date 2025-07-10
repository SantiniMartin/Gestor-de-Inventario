using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailSender _emailSender;

    public AccountController(UserManager<IdentityUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = encodedToken }, protocol: Request.Scheme);

        await _emailSender.SendEmailAsync(email, "Restablecer contraseña",
            $"<p>Restablecé tu contraseña haciendo clic <a href='{callbackUrl}'>aquí</a>.</p>");

        return RedirectToAction("ForgotPasswordConfirmation");
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return BadRequest("Falta información");
        }

        var model = new ResetPasswordViewModel
        {
            UserId = userId,
            Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token))
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return RedirectToAction("ResetPasswordConfirmation");
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (result.Succeeded)
        {
            return RedirectToAction("ResetPasswordConfirmation");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }
}
