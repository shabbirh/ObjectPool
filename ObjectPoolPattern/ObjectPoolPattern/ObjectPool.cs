using System;
using System.Collections.Concurrent;
using Caliburn.Micro;

namespace ObjectPoolPattern
{
    public class ObjectPool<T> : IDisposable, IHandle<RepoolResourceMessage<T>> where T : IPoolableResource
    {
        private ConcurrentBag<T> pooledObjects;
        private Func<T> objectGenerator;
        private bool isDisposed;
        private EventAggregator eventAggregator;

        public ObjectPool(Func<T> objectGenerator)
        {
            if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
            this.eventAggregator = new EventAggregator();
            this.pooledObjects = new ConcurrentBag<T>();
            this.objectGenerator = objectGenerator;
            this.eventAggregator.Subscribe(this);
        }

        public T GetResource()
        {
            T item;
            if (this.pooledObjects.TryTake(out item)) return item;
            var newResource = this.objectGenerator();
            ((IPoolableResource)newResource).EventAggregator = this.eventAggregator;
            return newResource;
        }

        public void PoolResource(T item)
        {
            this.pooledObjects.Add(item);
        }

        #region IDisposable

        public void Dispose()
        {
            if(this.isDisposed)
            {
                return;
            }
            this.isDisposed = true;
            if(typeof(IDisposable).IsAssignableFrom(typeof(T)))
            {
                while(this.pooledObjects.Count > 0)
                {
                    T item;
                    if(this.pooledObjects.TryTake(out item))
                    {
                        ((IDisposable)item).Dispose();
                    }

                }
            }
        }

        public bool IsDisposed
        {
            get { return this.isDisposed; }
        }

        #endregion
     
        public void Handle(RepoolResourceMessage<T> message)
        {
           this.PoolResource(message.PoolableResource);
        }
    }
}
