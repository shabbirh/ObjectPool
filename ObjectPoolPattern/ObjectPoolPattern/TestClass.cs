using System;
using Caliburn.Micro;

namespace ObjectPoolPattern
{
    public class TestClass : IDisposable, IPoolableResource
    {
        public int[] Nums { get; set; }

        public TestClass()
        {
            this.Nums = new int[1000000];
            Random rand = new Random();
            for (int i = 0; i < this.Nums.Length; i++)
            {
                this.Nums[i] = rand.Next();
            }
        }

        public double GetValue(long i)
        {
            return Math.Sqrt(this.Nums[i]);
        }

        public void Dispose()
        {
            this.EventAggregator.Publish(new RepoolResourceMessage<TestClass>(this));
        }

        public EventAggregator EventAggregator { get; set; }
    }
}