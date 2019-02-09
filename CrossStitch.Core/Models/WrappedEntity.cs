using System;

namespace CrossStitch.Core.Models
{
    public abstract class WrappedEntity<TEntity> : NotifyPropertyChanged
    {
        public WrappedEntity(TEntity wrapped)
        {
            if (wrapped == null)
                throw new ArgumentException("Cannot wrap null entity");

            Wrapped = wrapped;
        }

        public TEntity Wrapped { get; set; }
    }

    public class SelectionWrapper<TEntity> : WrappedEntity<TEntity>
    {
        public SelectionWrapper(TEntity wrapped) : base(wrapped)
        { }

        public bool IsSelected { get; set; }
    }
}