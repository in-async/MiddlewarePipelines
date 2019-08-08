using System.Collections.Generic;

namespace Inasync.OnionPipelines.Tests {

    public class SpyHandler : SpyComponent {

        public SpyHandler(List<SpyComponent> invokedComponents, DummyResult result) : base(invokedComponents, result) {
        }
    }
}
