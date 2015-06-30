namespace ObjectPoolPattern
{
    public class RepoolResourceMessage<T> where T:IPoolableResource
    {
        public RepoolResourceMessage(T poolableResource)
        {
            this.PoolableResource = poolableResource;
        }

        public T PoolableResource { get; set; }
    }
}