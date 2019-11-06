namespace Structures.Weapons.Interfaces {
    using System.Threading.Tasks;

    public interface Reloadable {
        Task Reload();
        bool IsReloading { get; }
    }
}