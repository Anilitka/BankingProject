using AutoMapper;
using Azure.Core;
using FinalProjectWebAPI.Dto;
using FinalProjectWebAPI.Interfaces;
using FinalProjectWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace FinalProjectWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankingController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public BankingController(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public IActionResult Register(AccountDto request)
        {
            var account = _mapper.Map<Account>(request);

            try
            {
                var createdAccount = _accountRepository.Create(account, request.Password, request.ConfirmPassword);
                return Ok(createdAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Login")]
        public IActionResult LogIn([FromBody] LogInRequestDto request)
        {
            var account = _accountRepository.LogIn(request.AccNumber, request.Pass);
            if (account == null)
                return Unauthorized("Invalid account number or password");

            var token = _accountRepository.CreateToken(account);
            var refreshToken = _accountRepository.GenerateRefreshToken();
            _accountRepository.SetRefreshToken(account, refreshToken);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken.Token
            });
        }

        [HttpGet("GetAllAccounts")]
        public ActionResult<IEnumerable<AccountDto>> GetAllAccounts()
        {
            try
            {
                var accounts = _accountRepository.GetAccounts();
                var accountsDto = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                return Ok(accountsDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Deposit")]
        public IActionResult Deposit(string accNumber, decimal amount)
        {
            try
            {
                _accountRepository.Deposit(accNumber, amount);
                return Ok("Deposit successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Withdraw")]
        public IActionResult Withdraw(string accNumber, decimal amount)
        {
            try
            {
                _accountRepository.Withdraw(accNumber, amount);
                return Ok("Withdrawal successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetById{id}")]
        public ActionResult<AccountDto> GetAccount(int id)
        {
            var account = _accountRepository.GetById(id);
            if (account == null)
                return NotFound();

            var accountDto = _mapper.Map<AccountDto>(account);
            return Ok(accountDto);
        }


    }
}
