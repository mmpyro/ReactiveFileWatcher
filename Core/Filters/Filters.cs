using Core.Events;
using System;

namespace Core.Filters
{
    public static class Filters
    {
        public static IFileFilter ExtFilter(string ext)
        {
            return new ExtFilter(ext);
        }

        public static IFileFilter Complex(params IFileFilter[] filters)
        {
            return new ComplexFilter(filters);
        }

        public static IFileFilter Name(string name)
        {
            return new NameFilter(name);
        }

        public static IFileFilter Custom(Predicate<FileChangedEventArgs> pred)
        {
            return new CustomFilter(pred);
        }
    }
}
