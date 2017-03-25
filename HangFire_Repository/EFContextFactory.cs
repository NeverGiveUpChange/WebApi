using HangFire_EFModel;
using System.Runtime.Remoting.Messaging;

namespace HangFire_Repository
{
    class EFContextFactory
    {
        /// <summary>
        /// 获取数据上下文，保持线程内部唯一
        /// </summary>
        /// <returns></returns>
        public static HangFire_DevEntities GetCurrentDbContext()
        {
            HangFire_DevEntities _dbContext = CallContext.GetData("DbContext") as HangFire_DevEntities;
            if (_dbContext == null) {
                _dbContext = new HangFire_DevEntities();
                    
                CallContext.SetData("DbContext", _dbContext);
            }
            return _dbContext;
        }
    }
}
