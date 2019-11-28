namespace NoteMe.Client.Sql
{
    public interface INoteMeContextFactory
    {
        NoteMeContext CreateContext();
    }
    
    public class NoteMeContextFactory : INoteMeContextFactory
    {
        private readonly SqliteSettings _settings;

        public NoteMeContextFactory(SqliteSettings settings)
        {
            _settings = settings;
        }
        
        public NoteMeContext CreateContext()
        {
            return new NoteMeContext(_settings);
        }
    }
}