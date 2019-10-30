namespace Structures {
    using System.Threading.Tasks;

    public interface Reloadable {
        Task Reload();
        bool IsReloading { get; }
    }
}