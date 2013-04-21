using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessPipeline.Domain
{
    public class Pipeline<T>
    {
        private readonly IList<IFilter<T>> _filters = new List<IFilter<T>>();

        public void Register(IFilter<T> filter)
        {
            _filters.Add(filter);
        }

        public void Execute(IEnumerable<T> input)
        {
            IEnumerable<T> current = input;

            foreach (IFilter<T> filter in _filters)
            {
                current = filter.Execute(current);
                // filter wants to stop pipeline
                if (current == null) return;
            }
            IEnumerator<T> enumerator = current.GetEnumerator();
            while (enumerator.MoveNext());
        }
    }
}
