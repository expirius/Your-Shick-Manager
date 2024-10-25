namespace MFASeekerServer.Infrastructure
{
    public class DbInitializer
    {
        public static void Initialize(ToiletDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
