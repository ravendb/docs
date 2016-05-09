using System;

namespace Payroll.Infrastructure
{
    public interface IDependencyInjector
    {
        T Get<T>();
        object Get(Type type);
    }
}
