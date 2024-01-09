namespace NerdStore.Core.Data
{
    public interface IUnityOfWork
    {
        Task<bool> Commit();
    }
}
