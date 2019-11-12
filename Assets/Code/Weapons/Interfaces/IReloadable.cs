namespace Weapons.Interfaces {
    using System.Threading.Tasks;

    public interface IReloadable {
        Task Reload();
        bool IsReloading { get; }
    }
}