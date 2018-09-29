using Core.Events;

namespace Core.Filters
{
    public interface IFileFilter
    {
        bool IsMatch(FileChangedEventArgs @event);
    }
}