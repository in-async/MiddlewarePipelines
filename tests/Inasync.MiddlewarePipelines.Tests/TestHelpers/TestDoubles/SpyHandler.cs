using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inasync.MiddlewarePipelines.Tests {

    public class SpyHandler : SpyComponent {

        public SpyHandler(List<SpyComponent> invokedComponents, Task result) : base(invokedComponents, result) {
        }
    }
}
