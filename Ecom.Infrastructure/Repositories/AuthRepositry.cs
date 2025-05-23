﻿using Ecom.Core.DTO;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharingl;
using Microsoft.AspNetCore.Identity;

namespace Ecom.Infrastructure.Repositories;

public class AuthRepositry : IAuth
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailServic;
    private readonly SignInManager<AppUser> _signInManger;
    private readonly IGenerateToken _token;
    public AuthRepositry(UserManager<AppUser> userManager, IEmailService emailServic, SignInManager<AppUser> signInManger, IGenerateToken token)
    {
        _userManager = userManager;
        _emailServic = emailServic;
        _signInManger = signInManger;
        _token = token;
    }
    public async Task<string?> RegisterAsync(RegisterDTO registerDTO)
    {
        if (registerDTO is null) return null;

        if (await _userManager.FindByNameAsync(registerDTO.UserName) is not null || await _userManager.FindByEmailAsync(registerDTO.Email) is not null) return "User is Already Registed";

        AppUser user = new() { Email = registerDTO.Email, UserName = registerDTO.UserName,DispalyName= registerDTO.DispalyName };

        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded) return result.Errors.ToList()[0].Description;

        #region Send Email
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await SendEmail(user.Email, token, "active", "Active Email", "Please Active your Email Click On Button To Active");
        #endregion

        return "done";
    }

    public async Task SendEmail(string email, string code, string component, string subject, string message)
    {
        var result = new EmailDTO(email
            , "ismael.manaser@gmail.com"
            , subject
            , EmailStringBody.send(email, code, component, message));
        await _emailServic.SendEmail(result);
    }

    public async Task<string?> LoginAsync(LoginDTO LoginDTO)
    {
        if (LoginDTO is null) return null;

        var finduser = await _userManager.FindByEmailAsync(LoginDTO.Email);

        if (finduser is null) return null;

        if(!finduser.EmailConfirmed)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(finduser);
            await SendEmail(finduser.Email!, token, "active", "Active Email", "Please Active your Email Click On Button To Active");
            return "Please confirem Your Email first, we have send activat to your  E-mail";
        }
        var result = await _signInManger.CheckPasswordSignInAsync(finduser, LoginDTO.Password,true);
        if(result.Succeeded)
        {
            return _token.GetAndCreateToken(finduser);
        }
        return "please check your email and password something went wrong";
    }

    public async Task<bool> SendEmailForForgetPassword(string email)
    {
        var user=await _userManager.FindByEmailAsync(email);
        if (user is null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await SendEmail(email, token, "Reset-Password", "Reset Password", "Click On Button To Reset");
        return true;
    }

    public async Task<string?>ResetPassword(ResetPasswordDTO resetPasswordDTO)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);

        if (user is null) return null;

        var result = await _userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password);

        if (result.Succeeded) return "Password change success";
        return result.Errors.ToList()[0].Description;
    }

    public async Task<bool> ActiveAccount(ActiveAccountDTO ActiveAccountdto)
    {
        var user = await _userManager.FindByEmailAsync(ActiveAccountdto.Email);
        if (user is null) return false;
        var result = await _userManager.ConfirmEmailAsync(user, ActiveAccountdto.Token);
        if (result.Succeeded) return true;
        
        var token=await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await SendEmail(user.Email!, token, "active", "Active Email", "Please Active your Email Click On Button To Active");
        return false;
    }
}