using Caliburn.Micro;

namespace ObjectPoolPattern
{
    public interface IPoolableResource
    {
        EventAggregator EventAggregator { get; set; }
    }
}