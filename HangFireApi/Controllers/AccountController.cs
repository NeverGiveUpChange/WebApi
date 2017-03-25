using HangFire_Common;
using HangFire_EFModel;
using HangFire_Infrastructure.CacheHelper;
using HangFire_Infrastructure.CacheHelper.RedisCacheHelper;
using HangFire_Infrastructure.LogHelper;
using HangFire_IService;
using HangFire_ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace HangFireApi.Controllers
{
    /// <summary>
    /// 注册登录
    /// </summary>
    public class AccountController : BaseApiController
    {
        readonly IAccountService _accountService;
        readonly CacheClient _cacheClient;
        const int expiry = 300;
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
            this._cacheClient = new CacheClient(new RedisStringCache(0));
        }
       /// <summary>
       /// 用户注册
       /// </summary>
       /// <param name="userName">用户名</param>
       /// <param name="passWord">密码</param>
       /// <param name="email">邮箱</param>
       /// <returns></returns>
        [HttpPost]
        
        public HttpResponseMessage Register([FromBody]Temp t)
        {
            var responseMassage = CommonHelper.CreateResponseData(-10004, "注册失败");
            var account = _accountService.Add(new Account { UserName = t.UserName, PassWord = t.PassWord.ToMD5Hash(), CreateTime = DateTime.Now, UpdateTime = DateTime.Now, Deleted = false, });
            if (account.Id > 0)
            {
                responseMassage.Data = account.Id;
                responseMassage.StatusCode = 1;
                responseMassage.Message = "注册成功";
            }
            
            return responseMassage.ToHttpResponseMessage();
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">用户密码</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Login(string userName, string passWord)
        {
            var responseMessage = CommonHelper.CreateResponseData(-10005, "用户名或密码错误");
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(passWord))
            {
                responseMessage.StatusCode = -10006;
                responseMessage.Message = "用户名或密码为空";
            }
            else
            {
                var account = _accountService.LoadEntities(x => x.UserName == userName).SingleOrDefault();
                var isSuccess = _accountService.Add(new Account() { UserName = "admin", PassWord = "123456", CreateTime = DateTime.Now, UpdateTime = DateTime.Now });
                if (account != null)
                {
                    if (account.PassWord == passWord.ToMD5Hash())
                    {
                        responseMessage.StatusCode = 1;
                        responseMessage.Message = "登陆成功";
                        var authInfo = new AuthorizeInfo() { UserId = account.Id, Expiry = expiry, Ticket =CommonHelper.CreateTicket(account.Id,account.UserName)};
                        _cacheClient.Set<AuthorizeInfo>(authInfo.Ticket, authInfo, TimeSpan.FromMinutes(expiry));
                        responseMessage.Data = authInfo.Ticket;
                    }
                }
            }
            return responseMessage.ToHttpResponseMessage();
        }
        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="sessionKey">检验码</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage LoginOut(string sessionKey)
        {
            var responseMessage = CommonHelper.CreateResponseData(-10007, "登出失败");
            if (_cacheClient.Remove(sessionKey))
            {
                responseMessage.StatusCode = 1;
                responseMessage.Message = "登出成功";
            }
            return responseMessage.ToHttpResponseMessage();
        }



    }
    public class Temp
    {
        public string  UserName { get; set; }
        public string  PassWord { get; set; }
        public string Email { get; set; }
    }
}
