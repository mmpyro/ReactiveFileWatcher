using Core.Events;

namespace Core.Filters
{
    public class ExtFilter : IFileFilter
    {
        private readonly string _ext;

        public ExtFilter(string ext)
        {
            _ext = ext;
        }

        public bool IsMatch(FileChangedEventArgs @event)
        {
            if (@event?.Name != null)
            {
                return @event.Name.EndsWith($".{_ext}");
            }
            return false;
        }
    }
}
