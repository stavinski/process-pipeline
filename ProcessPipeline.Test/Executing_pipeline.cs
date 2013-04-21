using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using ProcessPipeline.Domain;
using Machine.Fakes;
using Rhino.Mocks;

namespace ProcessPipeline.Test
{
    [Subject("Executing pipeline")]
    public class When_filters_wants_to_stop_pipeline : WithSubject<Pipeline<string>>
    {
        static IList<SequentialStringTestFilter> filters;
        static IList<string> items;
        
        Establish context = () =>
        {
            filters = new List<SequentialStringTestFilter>
            {
                new SequentialStringTestFilter() { ExitAfter = 2 },
                new SequentialStringTestFilter()
            };
            items = new List<string>();
            filters[0].Callback = content => items.Add(content);
            filters[1].Callback = content => items.Add(content);
                        
            foreach (var filter in filters)
                Subject.Register(filter);
        };

        Because of = () => Subject.Execute(new[] { "one", "two" });

        It should_not_execute_subsequent_sequential_filters = () =>
        {
            items.Count.ShouldEqual(1);
        };
    }

    [Subject("Executing pipeline")]
    public class When_filters_can_run_in_parallel : WithSubject<Pipeline<string>>
    {
        static IList<ParallelStringTestFilter> filters;
        static IList<string> items;
        
        Establish context = () => {
            filters = new List<ParallelStringTestFilter>
            {
                new ParallelStringTestFilter(),
                new ParallelStringTestFilter()
            };
            items = new List<string>();
            filters[0].Callback = content => items.Add(content);
            filters[1].Callback = content => items.Add(content);

            foreach (var filter in filters)
                Subject.Register(filter);
        };

        Because of = () =>
            {
                Subject.Execute(new[] { "one", "two" });
            };

        It should_execute_each_filter_per_item = () =>
        {
            items[0].ShouldBeEqualIgnoringCase("one");
            items[1].ShouldBeEqualIgnoringCase("one");
            items[2].ShouldBeEqualIgnoringCase("two");
            items[3].ShouldBeEqualIgnoringCase("two");
        };

    }

    [Subject("Executing pipeline")]
    public class When_filters_run_sequentially : WithSubject<Pipeline<string>>
    {
        static IList<SequentialStringTestFilter> filters;
        static IList<string> items;

        Establish context = () =>
        {
            filters = new List<SequentialStringTestFilter>
            {
                new SequentialStringTestFilter(),
                new SequentialStringTestFilter()
            };
            items = new List<string>();
            filters[0].Callback = content => items.Add(content);
            filters[1].Callback = content => items.Add(content);

            foreach (var filter in filters)
                Subject.Register(filter);
        };

        Because of = () =>
        {
            Subject.Execute(new[] { "one", "two" });
        };

        It should_execute_each_filter_all_at_once = () =>
        {
            items[0].ShouldBeEqualIgnoringCase("one");
            items[1].ShouldBeEqualIgnoringCase("two");
            items[2].ShouldBeEqualIgnoringCase("one");
            items[3].ShouldBeEqualIgnoringCase("two");
        };
    }

   
}
