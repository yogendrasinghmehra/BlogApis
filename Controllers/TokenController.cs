using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogApis.Models;
using System.Linq;
using System.Collections;

namespace BlogApis.Controllers
{
  [Route("api/[controller]")]
  public class TokenController : Controller
  {
    private IConfiguration _config;   
     private BlogDbContext blogDbContext;
    public TokenController(IConfiguration config,BlogDbContext blogDb)
    {
      _config = config;
      blogDbContext=blogDb;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult CreateToken([FromBody]LoginModel login)
    {
      IActionResult response = Unauthorized();
      var user = Authenticate(login);

      if (user != null)
      {
        var tokenString = BuildToken(user);
        response = Ok(new { token = tokenString,expireOn=DateTime.Now.AddMinutes(30).ToString("MM/dd/yyyy hh:mm tt")});
      }
  
      return response;
    }

    private string BuildToken(UserModel user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
          _config["Jwt:Issuer"],
          expires: DateTime.Now.AddMinutes(30),
          signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
     }

     private UserModel Authenticate(LoginModel login)
     {
        UserModel user = null; 
        var getUser=blogDbContext.Admin.Where(i=>i.Name==login.Username && i.Password==login.Password).SingleOrDefault();              
        if (getUser!=null)
        {         
            user = new UserModel { Name = getUser.Name, Email = getUser.Name};
        }
        return user;
     }

    public class LoginModel
    {
      public string Username { get; set; }
      public string Password { get; set; }
    }

    private class UserModel
    {
      public string Name { get; set; }
      public string Email { get; set; }
      public DateTime Birthdate { get; set; }
    }
  }
}