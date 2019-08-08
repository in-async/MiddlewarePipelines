using System.Collections.Generic;

namespace Inasync.OnionPipelines.Tests {

    public abstract class SpyComponent {
        private readonly List<SpyComponent> _invokedComponents;
        private readonly DummyResult _result;

        public SpyComponent(List<SpyComponent> invokedComponents, DummyResult result = null) {
            _invokedComponents = invokedComponents;
            _result = result;
        }

        public DummyContext ActualContext { get; private set; }

        public DummyResult Invoke(DummyContext context) {
            ActualContext = context;
            _invokedComponents.Add(this);
            return _result;
        }
    }
}
