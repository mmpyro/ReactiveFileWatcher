using Core.Events;
using System.Text.RegularExpressions;

namespace Core.Filters
{
    public class NameFilter : IFileFilter
    {
        private readonly string _name;

        public NameFilter(string name)
        {
            _name = name;
        }

        public bool IsMatch(FileChangedEventArgs @event)
        {
            var regex = new Regex($@"{_name}\.\w*");
            return regex.IsMatch(@event?.Name);
        }
    }
}