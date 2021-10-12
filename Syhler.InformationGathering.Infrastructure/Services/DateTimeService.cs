using System;
using Syhler.InformationGathering.Application.Common.Interface;

namespace Syhler.InformationGathering.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}