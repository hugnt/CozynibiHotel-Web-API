using CozynibiHotel.Core.Dto;
using CozynibiHotel.Services.Models;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IAccountService
    {
        IEnumerable<AccountDto> GetAccounts();
        AccountDto GetAccount(int accountId);
        ResponseModel CreateAccount(AccountDto accountCreate);
        ResponseModel UpdateAccount(int accountId, AccountDto updatedAccount);
        ResponseModel DeleteAccount(int accountCategoryId);
        Task<ResponseModel> ValidateAccount(AccountDto account);
        Task<ResponseModel> RenewToken(TokenModel model);
    }
}
