using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DiplomaProject.IRepositories;
using DiplomaProject.Models;
using DiplomaProject.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.Repositories;

public class AccountRepository: IAccountRepository
{
    private readonly KraftWebAppContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    public AccountRepository(KraftWebAppContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    // public int GetCurrentUserId()
    // {
    //     //var user = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
    //     //return user.Id;
    // }
        
    public User GetCurrentUser(string username)
    {
        return _context.Users.FirstOrDefault(u => u.Email == username);
    }

    public async Task<bool> Register(RegisterViewModel model)
    {
        // var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        var user = await GetUserByEmail(model.Email);
        if (user != null) return false;
        
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

        return true;
        //return RedirectToAction("Index", "Home");

        // ModelState.AddModelError("", "Некорректні логін та(чи) пароль");
        //
        // return View(model);
    }

    public async Task<bool> Login(User user, string password)
    {
        
            // var user = await _context.Users
            //     .Include(u => u.Role)
            //     .FirstOrDefaultAsync(u => u.Email == model.Email);
            
                var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (passwordResult == PasswordVerificationResult.Success)
                {
                    await Authenticate(user);
                    //return RedirectToAction("Index", "Home");
                    return true;
                }
                else
                    return false;
                //ModelState.AddModelError("", "Некорректний пароль");


                //ModelState.AddModelError("", "Некорректні логін та(чи) пароль");

    }

    // public async Task<IActionResult> Logout()
    // {
    //     await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //     return RedirectToAction("Index", "Home");
    // }
    
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
        //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
}
