using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SGI.Core.Extensions;

namespace SGI.UnitTests.ExtensionsTests
{
    [TestClass]
   public class BlockCollectionExtensionsTests
    {
        [TestMethod]
        public void AsRateLimitedObservable()
        {
            const int maxItems = 1; // fix this to 1 to ease testing
            var during = TimeSpan.FromSeconds(1);

            // populate collection
            var items = new[] { 1, 2, 3, 4 };
            var collection = new BlockingCollection<int>();
            foreach (var i in items) collection.Add(i);
            collection.CompleteAdding();

            var observable = collection.AsRateLimitedObservable(maxItems, during, CancellationToken.None);
            var processedItems = new BlockingCollection<int>();
            var completed = new ManualResetEvent(false);
            var last = DateTime.UtcNow;
            observable
                // this is so we'll receive exceptions
            .ObserveOn(new SynchronizationContext())
            .Subscribe(item =>
            {
                if (item == 1)
                    last = DateTime.UtcNow;
                else
                {
                    TimeSpan diff = (DateTime.UtcNow - last);
                    last = DateTime.UtcNow;

                    Assert.AreEqual(diff.TotalMilliseconds, during.TotalMilliseconds, 30);
                }
                processedItems.Add(item);
            },
                () => completed.Set()
            );
            completed.WaitOne();
            CollectionAssert.AreEqual(items, processedItems);
        }
       
    }
}
