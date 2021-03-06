using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DiplomaProject.Models;
using DiplomaProject.Repositories;
using DiplomaProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DiplomaProject.Controllers;

public class AccountController : Controller
{
    private readonly KraftWebAppContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    //private readonly AccountRepository _accountRepository;
    
    public AccountController(KraftWebAppContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            //var user = await _accountRepository.GetUserByEmail(model.Email);
            var isExist = _context.Users.Any(u => u.Email == model.Email);
            if (!isExist)
            {
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password
                };
                var hashPassword = _passwordHasher.HashPassword(user, model.Password);
                user.Password = hashPassword;

                Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                if (userRole != null)
                    user.Role = userRole;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await Authenticate(user);

                return RedirectToAction("Index", "Home");
            }

            // var registerResult = await _accountRepository.Register(model);
            // if (registerResult)
            // {
            //     return RedirectToAction("Index", "Home");
            // }


            ModelState.AddModelError("", "???????????????????? ?????? ??????????");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == model.Email);
            //var user = await _accountRepository.GetUserByEmail(model.Email);

            if (user != null)
            {
                var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
                 if (passwordResult == PasswordVerificationResult.Success)
                 {
                     await Authenticate(user);
                     return RedirectToAction("Index", "Home");
                 }
                // var loginResult = await _accountRepository.Login(user,model.Password);
                // if (loginResult)
                // {
                //     return RedirectToAction("Index", "Home");
                // }

                ModelState.AddModelError("", "???????????????????????? ????????????");
            }

            ModelState.AddModelError("", "?????????????????????? ?????????? ????(????) ????????????");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
    
    private async Task Authenticate(User user)
    {
        // ?????????????????? ???????? claim
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
        };
        
        // ?????????????????? ????'?????? ClaimsIdentity
        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        
        // ?????????????????? ?????????????????????????????????? ????????
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
    
    [AcceptVerbs("GET", "POST")]
    public IActionResult VerifyUniqueEmail(string Email)
    {
        var isExist = _context.Users.Any(u => u.Email == Email);

        return isExist ? Json($"Email {Email} ?????? ???????????????????????????????? ?? ??????????????. ???????? ??????????, ?????????????? ?????????? email.") : Json(true);
    }
}