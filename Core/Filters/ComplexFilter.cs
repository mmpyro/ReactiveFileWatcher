using Core.Events;
using System.Linq;

namespace Core.Filters
{
    public class ComplexFilter : IFileFilter
    {
        private readonly IFileFilter[] _filters;

        public ComplexFilter(IFileFilter[] filters)
        {
            _filters = filters;
        }

        public bool IsMatch(FileChangedEventArgs @event)
        {
            return _filters?.Any(f => f.IsMatch(@event)) ?? false;
        }
    }
}