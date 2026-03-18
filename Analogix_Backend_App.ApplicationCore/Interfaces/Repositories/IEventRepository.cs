using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Services
{
    public interface IEventRepository
    {

        Event Create(Event data);
        Event Update(Event data);
        void Delete(Event data);
        Event? GetById(long id);
        Event? GetByIdWithSubscriptions(long id);
        List<Event> GetAll();

    }
}
