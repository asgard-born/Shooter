using System.Threading.Tasks;

namespace Structures {
    public interface Reloadable {
        Task  Reload();
        bool IsReloading();
    }
}