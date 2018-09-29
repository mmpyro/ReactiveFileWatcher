using System;
using Core.Events;

namespace Core.Filters
{
    public class CustomFilter : IFileFilter
    {
        private readonly Predicate<FileChangedEventArgs> _pred;

        public CustomFilter(Predicate<FileChangedEventArgs> pred)
        {
            _pred = pred;
        }

        public bool IsMatch(FileChangedEventArgs @event)
        {
            return _pred.Invoke(@event);
        }
    }
}