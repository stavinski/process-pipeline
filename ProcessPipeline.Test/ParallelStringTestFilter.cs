using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProcessPipeline.Domain;
using System.Diagnostics;

namespace ProcessPipeline.Test
{
    public class ParallelStringTestFilter : IFilter<string>
    {
        public Action<string> Callback = _ => { };
        
        public IEnumerable<string> Execute(IEnumerable<string> input)
        {
            foreach (var content in input)
            {
                Callback(content);
                yield return content;
            }
        }
    }
}
