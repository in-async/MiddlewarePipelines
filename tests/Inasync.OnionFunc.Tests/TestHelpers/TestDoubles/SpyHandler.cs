using System.Collections.Generic;

namespace Inasync.OnionFunc.Tests {

    public class SpyHandler : SpyComponent {

        public SpyHandler(List<SpyComponent> invokedComponents, DummyResult result) : base(invokedComponents, result) {
        }
    }
}
