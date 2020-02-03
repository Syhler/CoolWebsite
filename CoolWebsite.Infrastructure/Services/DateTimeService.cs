using System;
using CoolWebsite.Application.Common.Interfaces;

namespace CoolWebsite.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}