using System.Threading.Tasks;

namespace GypsyRig
{
    public abstract class TableProcessorBase : ITableProcessor
    {
        public virtual string Name { get; set; }
        public abstract Task Migrate();
    }
}
