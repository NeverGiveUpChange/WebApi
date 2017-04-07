using FluentScheduler;
using System;

namespace HangFire_FluentScheduler
{
    class FluentScheduler : Registry, IFluentScheduler
    {
        public void ExcuteJob<T>(bool isRunNow, bool isRunOnce, LifeCycle lifeCycle) where T : class, IJob
        {
            if (lifeCycle != null)
            {

                var schedule = Schedule<T>();
                if (isRunOnce)
                {
                    RunOnce(schedule, lifeCycle);
                }
                else if (isRunNow)
                {
                    RunNow(schedule, lifeCycle);
                }
                else
                {
                    RunEvery(schedule, lifeCycle);
                }
            }
        }

        private void RunOnce(Schedule schedule, LifeCycle lifeCycle)
        {
            schedule.ToRunOnceIn(lifeCycle.Day * 24 * 60 * 60 + lifeCycle.Hour * 60 * 60 + lifeCycle.Minute * 60 + lifeCycle.Second).Seconds();
        }
        private void RunEvery(Schedule schedule, LifeCycle lifeCycle)
        {
            ConfigureJobLifeCycle(schedule.ToRunEvery, lifeCycle);
        }
        private void RunNow(Schedule schedule, LifeCycle lifeCycle)
        {
            ConfigureJobLifeCycle(schedule.ToRunNow().AndEvery, lifeCycle);
        }
        private void ConfigureJobLifeCycle(Func<int, TimeUnit> ConfigureJobFunc, LifeCycle lifeCycle)
        {
            if (lifeCycle.Month != default(int))
            {
                var monthUnit = ConfigureJobFunc(lifeCycle.Month).Months();
                switch (lifeCycle.Week)
                {
                    case 1:
                        monthUnit.OnTheFirst((DayOfWeek)lifeCycle.Day).At(lifeCycle.Hour, lifeCycle.Minute);
                        break;
                    case 2:
                        monthUnit.OnTheSecond((DayOfWeek)lifeCycle.Day).At(lifeCycle.Hour, lifeCycle.Minute);
                        break;
                    case 3:
                        monthUnit.OnTheThird((DayOfWeek)lifeCycle.Day).At(lifeCycle.Hour, lifeCycle.Minute);
                        break;
                    case 4:
                        monthUnit.OnTheFourth((DayOfWeek)lifeCycle.Day).At(lifeCycle.Hour, lifeCycle.Minute);
                        break;
                }
            }
            else if (lifeCycle.Week != default(int))
            {
                ConfigureJobFunc(lifeCycle.Week).Weeks().On((DayOfWeek)lifeCycle.Day).At(lifeCycle.Hour, lifeCycle.Minute);
            }
            else if (lifeCycle.Day != default(int))
            {
                ConfigureJobFunc(lifeCycle.Day).Days().At(lifeCycle.Hour, lifeCycle.Minute);
            }
            else if (lifeCycle.Hour != default(int))
            {
                ConfigureJobFunc(lifeCycle.Hour).Hours().At(lifeCycle.Minute);
            }
            else if (lifeCycle.Minute != default(int))
            {
                ConfigureJobFunc(lifeCycle.Minute).Minutes().DelayFor(lifeCycle.Second).Seconds();
            }
            else
            {
                ConfigureJobFunc(lifeCycle.Second).Seconds();
            }
        }
    }
}
