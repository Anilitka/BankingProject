using AutoMapper;
using FinalProjectWebAPI.Dto;
using FinalProjectWebAPI.Models;

namespace FinalProjectWebAPI.Helper
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile() 
        {
            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();
            CreateMap<Transaction, TransactionDto>();
            CreateMap<TransactionDto, Transaction>();
            CreateMap<LogInRequestDto, Account>();
        }
    }
}
