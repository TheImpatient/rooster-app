using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Oauth2.v2.Data;

namespace AgendaLibrary
{
    public class Agenda
    {
        private CalendarService _service;
        private CalendarService _userService;
        private Userinfoplus _userInfo;
        private Events _agenda;
        public Agenda()
        {
            _service = Network.GetCalendar();
            _userService = Network.GetUserCalendarService();
            _userInfo = Network.GetUserInfoService();
            _agenda = Network.GetAgenda();
        }

        public Boolean AddEvent()
        {
            var event1 = new Event()
            {
                Id = "362606de6aa34923815ffe0dc4616023",
                Summary = "CONTROLE AAN DE KANT !! DIT IS DE BAZEN POLITIE ",
                Creator = new Event.CreatorData(){DisplayName = "Roosterify", Email = "roosterify@gmail.com"},               
                Organizer = new Event.OrganizerData() { DisplayName = "Roosterify", Email = "roosterify@gmail.com" },
                Start = new EventDateTime() { DateTime = DateTime.Today},
                End = new EventDateTime() { DateTime = DateTime.Now.AddDays(1)},
                Description = "WHAT SEEMS TO BE THE OFFICER PROBLEM ?",
            };


            var attendeeList = new List<EventAttendee>();
            attendeeList.Add(new EventAttendee()
            {
                Email = _userInfo.Email,
                ResponseStatus = "accepted",
                Self = true,
                Organizer = false
            });

            EventAttendee ea = new EventAttendee();
            
            event1.Attendees = attendeeList;
            _service.Events.Insert(event1, Network._agendaId).Execute();

            return true;
        }

        public CalendarService GetUserCalendarService()
        {
            return _userService;
        }

        public Events GetUserAgenda()
        {
            return _userService.Events.List("primary").Execute();
        }

        public bool UpdateEvent(Les les)
        {
            try
            {
                var item = _agenda.Items.FirstOrDefault(x => x.Id.Equals(les.CalendarGuid));

                List<EventAttendee> lea = item.Attendees.ToList();

                if (lea.Any(x=>x.Email.Equals(_userInfo.Email)))
                {
                    lea.Add(new EventAttendee()
                    {
                        Email = _userInfo.Email,
                        ResponseStatus = "accepted",
                        Self = true,
                        Organizer = false
                    });
                }

                item.Attendees = lea;

                _service.Events.Update(item, Network._agendaId, les.CalendarGuid).Execute();
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

    }
}
