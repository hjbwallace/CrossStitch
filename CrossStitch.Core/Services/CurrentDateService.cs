using CrossStitch.Core.Interfaces;
using System;

namespace CrossStitch.Core.Services
{
    public class CurrentDateService : ICurrentDateService
    {
        public DateTime Now() => DateTime.Now;
    }
}