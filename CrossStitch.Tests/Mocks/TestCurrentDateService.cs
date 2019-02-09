using CrossStitch.Core.Interfaces;
using System;

namespace CrossStitch.Tests.Mocks
{
    public class TestCurrentDateService : ICurrentDateService
    {
        private DateTime _now;

        public TestCurrentDateService()
        {
            _now = DateTime.Now;
        }

        public DateTime Now() => _now;

        public void TimeTravel(int numberOfDays)
        {
            _now = _now.AddDays(numberOfDays);
        }
    }
}