namespace MFASeekerServer.Infrastructure
{
    public class DbInitializer
    {
        public static void Initialize(SeekerDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
