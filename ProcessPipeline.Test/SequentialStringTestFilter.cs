using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProcessPipeline.Domain;
using System.Diagnostics;

namespace ProcessPipeline.Test
{
    public class SequentialStringTestFilter : IFilter<string>
    {
        private int _currentCount;

        public Action<string> Callback = _ => { };
        public int ExitAfter { get; set; }

        public IEnumerable<string> Execute(IEnumerable<string> input)
        {
            foreach (var content in input)
            {
                if (ExitAfter > 0)
                {
                    _currentCount++;
                    if (_currentCount == ExitAfter) return null;
                }

                Callback(content);
            }

            return input;
        }
    }
}
