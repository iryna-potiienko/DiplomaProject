using System.Collections.Generic;
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            //var user = await _accountRepository.GetUserByEmail(model.Email);
            if (user == null)
            {
                user = new User
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

            ModelState.AddModelError("", "Некорректні логін та(чи) пароль");
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

                ModelState.AddModelError("", "Некорректний пароль");
            }

            ModelState.AddModelError("", "Некорректні логін та(чи) пароль");
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
        // створюємо один claim
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
        };
        
        // створюємо об'єкт ClaimsIdentity
        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        
        // установка аутентифікаційних кукі
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
}