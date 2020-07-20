using LibraryApi.Services;
using System;


namespace LibraryApiIntegrationTests.Fakes
{
    public class FakeSystemTime : ISystemTime
    {
        public DateTime GetCurrent()
        {
            return new DateTime(1969, 4, 20, 23, 59, 59);
        }
    }
}
