using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GypsyRig
{
    public class ProcessorBus : IProcessorBus
    {
        private readonly IList<ITableProcessor> _processors;

        public ProcessorBus(IList<ITableProcessor> processors)
        {
            _processors = processors;
        }

        public Task Migrate()
            => Task.WhenAll(_processors.Select(p => p.Migrate()));

        public Task Migrate(string processorName)
            => _processors.FirstOrDefault(p => p.Name == processorName)?.Migrate();
    }
}
