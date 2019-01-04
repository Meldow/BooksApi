namespace BooksApi.Controllers
{
    using System;
    using BooksApi.Services;
    using Microsoft.AspNetCore.Mvc;

    // https://github.com/aspnet/Identity/blob/master/src/Core/PasswordHasher.cs
    // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-2.2

    [Route("api/[controller]")]
    public class PasswordHasherController : ControllerBase
    {
        private readonly IPasswordHasherService passwordHasherService;

        public PasswordHasherController(IPasswordHasherService passwordHasherService)
        {
            this.passwordHasherService = passwordHasherService ?? throw new ArgumentNullException(nameof(passwordHasherService));
        }

        [HttpGet("{password}")]
        public ActionResult<string> HashPassword(string password)
        {
            return this.passwordHasherService.HashPassword(password) ?? (ActionResult<string>)this.NotFound();
        }

        [HttpGet("{password}/{hashedPassword}")]
        public ActionResult<bool> VerifyPassword(string password, string hashedPassword)
        {
            return this.passwordHasherService.VerifyPassword(password, hashedPassword);
        }
    }
}
