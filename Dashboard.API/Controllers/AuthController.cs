using AutoMapper;
using Dashboard.API.Configure;
using Dashboard.API.Entities;
using Dashboard.API.Helper;
using Dashboard.API.Models;
using Dashboard.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IMapper _mapper;
        private readonly IMyTools _myTools;
        private readonly JwtConfig _jwtConfig;

        public AuthController(IAccountsRepository accountsRepository,
                              IRefreshTokensRepository refreshTokensRepository,
                              IMapper mapper,
                              IOptionsMonitor<JwtConfig> options,
                              IMyTools myTools)
        {
            this._accountsRepository = accountsRepository;
            this._refreshTokensRepository = refreshTokensRepository;
            this._mapper = mapper;
            this._myTools = myTools;
            this._jwtConfig = options.CurrentValue;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="accountForLogin"></param>
        [HttpPost("login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<AccountsDTO> Login([FromBody] AccountLoginDTO accountForLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var account = _accountsRepository.GetAccount(accountForLogin.Username);

            if (account == null)
            {
                return NotFound("Username or Password incorrect");
            }

            var verifyAccount = BCrypt.Net.BCrypt.Verify(accountForLogin.Password, account.Password);

            // determine the password matches
            if (!verifyAccount)
            {
                return NotFound("Username or Password incorrect");
            }

            var accountsDTO = GenerateToken(account);

            return Ok(accountsDTO);
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="refreshTokenRequest"></param>
        [HttpPost("refreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AccountsDTO> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var account = VerifyToken(refreshTokenRequest);

            if (account == null)
            {
                return BadRequest("Token or Refresh Token invalid");
            }

            var accountsDTO = GenerateToken(account);

            return Ok(accountsDTO);
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="accountsRegister"></param>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] AccountsRegisterDTO accountsRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var account = _mapper.Map<Accounts>(accountsRegister);

            _accountsRepository.AddAccount(account);

            return Ok(new { account.Username, account.Firstname, account.Lastname, account.Email });
        }

        private AccountsDTO GenerateToken(Accounts account)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(10), signingCredentials: signIn);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshTokens
            {
                Username = account.Username,
                Token = jwtToken,
                RefreshToken = _myTools.RandomString(35) + Guid.NewGuid(),
                IsUsed = false,
                IsRevorked = false,
                CreateDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(1)
            };

            refreshToken = _refreshTokensRepository.AddRefreshToken(refreshToken);

            var accountsDTO = _mapper.Map<AccountsDTO>(account);

            accountsDTO.JwtToken = jwtToken;
            accountsDTO.RefreshToken = refreshToken.RefreshToken;

            return accountsDTO;
        }

        private Accounts VerifyToken(RefreshTokenRequest refreshTokenRequest)
        {
            var refreshTokens = _refreshTokensRepository.GetRefreshToken(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken);

            if (refreshTokens == null) return null;

            refreshTokens.IsUsed = true;
            _refreshTokensRepository.UpdateRefreshToken(refreshTokens);

            return _accountsRepository.GetAccount(refreshTokens.Username);
        }
    }
}
