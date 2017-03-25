using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_ViewModel
{
   public class Vm_ResponseData
    {
       /// <summary>
       /// 状态码
       /// </summary>
        public int StatusCode { get; set; }
       /// <summary>
       /// 信息
       /// </summary>
        public string Message { get; set; }
       /// <summary>
       /// 返回数据
       /// </summary>
        public object Data { get; set; }
    }
}
