using ElectronNET.WebApp.Domain.Entity;

namespace ElectronNET.WebApp.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        #region private properties       
        private IBaseRepository<MenuItem> _menuItemRepository;

        private readonly HotelContext _hotelContext;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="hotelContext">The core connect context.</param>
        public UnitOfWork(HotelContext hotelContext)
        {
            this._hotelContext = hotelContext;
        }

        /// <summary>
        /// Gets the address repository.
        /// </summary>
        /// <value>
        /// The address repository.
        /// </value>
        public IBaseRepository<MenuItem> MenuItemRepository
        {
            get { return _menuItemRepository = this._menuItemRepository ?? new BaseRepository<MenuItem>(_hotelContext); }
        }
    }
}
