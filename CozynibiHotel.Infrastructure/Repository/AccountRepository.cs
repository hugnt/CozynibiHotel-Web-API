using CozynibiHotel.Core.Dto;
using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Infrastructure.Data;
using HUG.CRUD.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Infrastructure.Repository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly AppDbContext _dbContext;

        public AccountRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsExists(AccountDto account)
        {
            var user = _dbContext.Accounts.SingleOrDefault(a =>
                a.Username == account.Username &&
                account.Password == account.Password
            );
            return user != null;
        }

        public Account GetAccount(AccountDto account)
        {
            return _dbContext.Accounts.SingleOrDefault(a =>
                a.Username == account.Username &&
                account.Password == account.Password
            );
        }
    }
}
