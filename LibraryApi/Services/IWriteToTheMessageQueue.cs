using LibraryApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public interface IWriteToTheMessageQueue
    {
        Task Write(Reservation reservation);
    }
}
