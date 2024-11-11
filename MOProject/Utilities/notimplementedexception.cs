
namespace MOProject.Utilities
{
    [Serializable]
    internal class notimplementedexception : Exception
    {
        public notimplementedexception()
        {
        }

        public notimplementedexception(string? message) : base(message)
        {
        }

        public notimplementedexception(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}