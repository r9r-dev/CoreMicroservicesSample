using System.Threading.Tasks;

namespace ITGA.Common.Mongo
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}
