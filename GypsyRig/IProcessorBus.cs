using System.Threading.Tasks;

namespace GypsyRig
{
    public interface IProcessorBus
    {
        Task Migrate();
        Task Migrate(string processorName);
    }
}