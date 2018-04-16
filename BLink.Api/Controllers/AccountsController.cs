﻿using BLink.Core.Constants;
using BLink.Core.Services;
using BLink.Models;
using BLink.Models.RequestModels.Accounts;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BLink.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMembersService _membersService;
        public AccountsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMembersService membersService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _membersService = membersService;
            _roleManager = roleManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] CreateUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    Member = new Member
                    {
                        FirstName = userViewModel.FirstName,
                        LastName = userViewModel.LastName,
                        Weight = userViewModel.Weight,
                        Height = userViewModel.Height
                    }
                };

                IdentityResult result = await _userManager.CreateAsync(user, userViewModel.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(userViewModel.Role.ToString()))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(userViewModel.Role.ToString()));
                    }

                    await _userManager.AddToRoleAsync(user, userViewModel.Role.ToString());
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return new JsonResult(GetLoginCredentials(user));
                }

                return Errors(result);

            }

            return Error("Unexpected error");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUser loginUser)
        {
            if (loginUser != null && ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, false);
                if (signInResult.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginUser.Email);
                    return new JsonResult(GetLoginCredentials(user));
                }

                return BadRequest();
            }

            return BadRequest();
        }

        private Dictionary<string, object> GetLoginCredentials(ApplicationUser user)
        {
            return new Dictionary<string, object>
                    {
                        {"access_token", GetAccessToken(user.Email) },
                        { "id", GetIdToken(user)},
                        { "roles", string.Join(", ", _roleManager
                            .Roles
                            .Where(r => user.Roles.Any(ur => ur.RoleId == r.Id ))
                            .Select(r => r.Name))
                        }
                    };
        }

        // TODO Add JwtService
        private string GetIdToken(IdentityUser user)
        {
            var payload = new Dictionary<string, object>
            {
                { "id", user.Id },
                { "sub", user.Email },
                { "email", user.Email },
                { "emailConfirmed", user.EmailConfirmed },
            };

            return GetToken(payload);
        }

        private string GetAccessToken(string email)
        {
            var payload = new Dictionary<string, object>
            {
                { "sub", email },
                { "email", email }
            };

            return GetToken(payload);
        }

        private string GetToken(Dictionary<string, object> payload)
        {
            var secret = _jwtSettings.SecretKey;

            payload.Add("iss", _jwtSettings.Issuer);
            payload.Add("aud", _jwtSettings.Audience);
            payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(7)));
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                .Select(x => x.Description)
                .ToArray();
            return new JsonResult(items) { StatusCode = 400 };
        }

        private JsonResult Error(string message)
        {
            return new JsonResult(message) { StatusCode = 400 };
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
