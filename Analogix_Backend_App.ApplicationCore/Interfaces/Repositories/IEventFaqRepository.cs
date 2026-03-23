using Analogix_Backend_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.ApplicationCore.Interfaces.Repositories
{
    public interface IEventFaqRepository
    {
        EventFaq Create ( EventFaq data);
        EventFaq Update ( EventFaq data );
        void Delete ( EventFaq data );
        EventFaq? GetById ( long id ); // Get a specific FAQ by its ID
        List<EventFaq> GetByEventId ( long eventId ); // Get all FAQs for a specific event
    }
}
