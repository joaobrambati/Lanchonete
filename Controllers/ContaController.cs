﻿using Lanchonete.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lanchonete.Controllers
{
    public class ContaController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ContaController (UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("login")]
        public IActionResult Login (string retornoUrl)
        {
            return View(new LoginViewModel()
            {
                RetornoUrl = retornoUrl
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginViewModel loginVm)
        {
            if (!ModelState.IsValid)
                return View(loginVm);

            var usuario = await _userManager
                          .FindByNameAsync(loginVm.Usuario);

            if (usuario != null)
            {
                var result = await _signInManager
                             .PasswordSignInAsync(usuario, loginVm.Senha, false, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVm.RetornoUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginVm.RetornoUrl);
                }
            }
            ModelState.AddModelError("", "Falha ao realizar o login!");
            return View(loginVm);
        }

        [HttpGet("registro")]
        public IActionResult Registro ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro (LoginViewModel registroVm)
        {
            if (ModelState.IsValid)
            {
                var usuario = new IdentityUser { UserName = registroVm.Usuario };
                var resultado = await _userManager.CreateAsync(usuario, registroVm.Senha);

                if (resultado.Succeeded)
                {
                    //await _signInManager.SignInAsync(usuario, isPersistent: false);
                    return RedirectToAction("Login", "Conta");
                }
                else
                {
                    this.ModelState.AddModelError("Registro", "Erro ao registrar o usuário");
                }
            }
            return View(registroVm);
        }
    }
}
