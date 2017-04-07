using FluentScheduler;

namespace HangFire_FluentScheduler
{
    public interface IFluentScheduler
    {
        /// <summary>
        /// 执行任务计划
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isRunNow">是否立即执行</param>
        /// <param name="isRunOnce">是否只执行一次</param>
        /// <param name="lifeCycle">执行周期</param>
        void ExcuteJob<T>(bool isRunNow=true,bool  isRunOnce=false, LifeCycle lifeCycle = null) where T : class, IJob;

    }
}
