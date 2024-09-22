namespace MFASeeker.Services
{
    interface IToiletService
    {
        // CRUD
        void AddToilet(); // C
        void GetToiletById(); // R 
        void GetToilets();
        void RemoveToilet(); // D
        void UpdateToilet(); // U

    }
    class ToiletService : IToiletService
    {
        public void AddToilet()
        {
            throw new NotImplementedException();
        }

        public void GetToiletById()
        {
            throw new NotImplementedException();
        }

        public void GetToilets()
        {
            throw new NotImplementedException();
        }

        public void RemoveToilet()
        {
            throw new NotImplementedException();
        }

        public void UpdateToilet()
        {
            throw new NotImplementedException();
        }
    }
}
