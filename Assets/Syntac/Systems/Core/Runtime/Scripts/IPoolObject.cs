namespace Syntac.Core
{
    /// <summary>
    /// Lifecycle contract for pooled objects.
    /// <c>OnCreate</c> fires once at instantiation, <c>OnGet</c>/<c>OnRelease</c> on every rent/return,
    /// <c>OnTerminate</c> once at final destruction.
    /// </summary>
    /// <remarks>
    /// Syntac.Core deliberately ships no pool implementation — the contract exists so pools written
    /// per-project agree on callback names.
    /// </remarks>
    public interface IPoolObject
    {
        void OnCreate();
        void OnGet();
        void OnRelease();
        void OnTerminate();
    }
}
