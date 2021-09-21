using System.Threading.Tasks;

namespace GypsyRig
{
    public interface ITableProcessor
    {
        string Name { get; }
        Task Migrate();
    }
}
