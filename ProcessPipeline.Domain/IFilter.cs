using System;
using System.Collections.Generic;
namespace ProcessPipeline.Domain
{
    public interface IFilter<T>
    {
        IEnumerable<T> Execute(IEnumerable<T> input);
    }
}
