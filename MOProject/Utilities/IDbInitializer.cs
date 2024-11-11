namespace MOProject.Utilities
{
    public interface IDbInitializer
    {
        void Initialize();
        Task InitializeAsync();
    }
}
