// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shopping.Identity.Data;
using Shopping.Identity.Helper;
using Shopping.Identity.Models;
using static Shopping.Identity.Enum.Enums;

namespace Shopping.Identity.Pages.Create;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IIdentityServerInteractionService _interaction;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public Index(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IIdentityServerInteractionService interaction)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _interaction = interaction;
    }

    public IActionResult OnGet(string? returnUrl)
    {
        Input = new InputModel { ReturnUrl = returnUrl };
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        // Check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // Handle cancellation
        if (Input.Button != "create")
        {
            if (context != null)
            {
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                return context.IsNativeClient() ? this.LoadingPage(Input.ReturnUrl) : Redirect(Input.ReturnUrl ?? "~/");
            }
            else
            {
                return Redirect("~/");
            }
        }

        // Validate user input
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = Input.Username,
                Email = Input.Email,
                PasswordHash = Input.Password,
                NormalizedUserName = Input.Name,
                
                // Add other properties as needed, e.g., Name, PhoneNumber, etc.
            };

            // Attempt to create the user
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                // Assign role if needed (e.g., "Customer")
                if (await _roleManager.RoleExistsAsync(userRole.customer.ToString()))
                {
                    await _userManager.AddToRoleAsync(user, userRole.customer.ToString());
                }

                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Redirect to the return URL if provided
                if (context != null)
                {
                    return context.IsNativeClient() ? this.LoadingPage(Input.ReturnUrl) : Redirect(Input.ReturnUrl ?? "~/");
                }

                if (Url.IsLocalUrl(Input.ReturnUrl))
                {
                    return Redirect(Input.ReturnUrl);
                }
                else
                {
                    return Redirect("~/");
                }
            }
            else
            {
                // Handle errors, add them to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }

        return Page();
    }
}

