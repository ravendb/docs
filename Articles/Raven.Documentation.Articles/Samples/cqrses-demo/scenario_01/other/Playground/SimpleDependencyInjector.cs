using System;
using Ninject;
using Payroll.Infrastructure;
using Payroll.Infrastructure.InMemoryBus;

namespace Playground
{
    public class SimpleDependencyInjector : IDependencyInjector
    {
        private readonly StandardKernel _kernel;
        public SimpleDependencyInjector()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IDependencyInjector>().ToConstant(this);
        }
        
        internal void BindToConstant<T>(T constant)
        {
            _kernel.Bind<T>().ToConstant(constant);
        }

        public T Get<T>()
        {
            return _kernel.Get<T>();
        }

        public object Get(Type type)
        {
            return _kernel.Get(type);
        }
    }
}