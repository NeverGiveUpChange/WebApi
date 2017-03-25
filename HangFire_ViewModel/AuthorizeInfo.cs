using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_ViewModel
{
    public class AuthorizeInfo
    {
        /// <summary>
        /// 登陆后返给用户的ticket
        /// </summary>
        public string Ticket { get; set; }
        /// <summary>
        /// Account.Id
        /// </summary>
        public int   UserId { get; set; }
        /// <summary>
        /// 过期时间分钟数
        /// </summary>
        public int Expiry{ get; set; }
       
    }
   
}
