using FitnessApp.Data;

namespace FitnessApp.Logic.Services
{
    public class BaseService : IDisposable
    {
        protected readonly ProductContext _context;
        private bool _isDisposed = false;
        protected BaseService(ProductContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool flag)
        {
            if (_isDisposed) return;

            _context?.Dispose();
            _isDisposed = true;

            if (flag) GC.SuppressFinalize(this);
        }

        ~BaseService()
        {
            Dispose(false);
        }
    }
}