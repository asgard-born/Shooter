namespace Weapons.Abstract {
    using System.Threading.Tasks;

    public interface IReloadable {
        Task Reload();
        bool IsReloading { get; }
    }
}