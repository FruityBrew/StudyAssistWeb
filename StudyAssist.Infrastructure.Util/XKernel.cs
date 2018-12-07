using System;
using Ninject;
using Ninject.Modules;
using StudyAssist.Core;
using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;

namespace StudyAssist.Infrastructure.Util
{
    public class XKernel : StandardKernel
    {
        private static XKernel _instance;

        static XKernel()
        {
            _instance = new XKernel();
        }

        public static XKernel Instance
        {
            get { return _instance; }
        }

        private XKernel()
        {
            Bind<IProblem>().To<Problem>();
            Bind<IRepeatCalculator>().To<RepeatDateCalculator>();
        }
    }
}
