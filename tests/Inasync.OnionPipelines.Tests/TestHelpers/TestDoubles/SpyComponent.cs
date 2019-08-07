using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inasync.OnionPipelines.Tests {

    public abstract class SpyComponent {
        private readonly List<SpyComponent> _invokedComponents;
        private readonly Task _result;

        public SpyComponent(List<SpyComponent> invokedComponents, Task result = null) {
            _invokedComponents = invokedComponents;
            _result = result;
        }

        public DummyContext ActualContext { get; private set; }

        public Task InvokeAsync(DummyContext context) {
            ActualContext = context;
            _invokedComponents.Add(this);
            return _result;
        }
    }
}
