using ElectronNET.WebApp.Domain.Entity;

namespace ElectronNET.WebApp.Domain
{
    /// <summary>
    /// This interface contains only methods which are specific to UnitOfWork.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets the delete employee repository.
        /// </summary>
        /// <value>
        /// The delete employee repository.
        /// </value>
        IBaseRepository<MenuItem> MenuItemRepository { get; }
    }
}
