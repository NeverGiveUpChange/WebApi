using HangFire_EFModel;
using HangFire_IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Repository
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        private  readonly HangFire_DevEntities _context;
        public AccountRepository(HangFire_DevEntities context) :base(context)
        {
            _context = context;
        }
    }
}
