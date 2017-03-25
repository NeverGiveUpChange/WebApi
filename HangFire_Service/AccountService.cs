using HangFire_EFModel;
using HangFire_IRepository;
using HangFire_IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Service
{
   public class AccountService:BaseService<Account>,IAccountService
    {
       readonly IAccountRepository _accountRepository;
       public AccountService(IAccountRepository accountRepository,IDbSession dbSession):base(dbSession,accountRepository)
       {
           this._accountRepository = accountRepository;
           //SetCurrentRepository<IAccountRepository>(accountRepository, dbSession);
       }
       //public override void SetCurrentRepository<K>(K currentIRepositoy, IDbSession dbSession)
       //{
       //    base._currentRepository = currentIRepositoy as IAccountRepository;
       //    base._dbSession = dbSession;
       //}
    }
}
