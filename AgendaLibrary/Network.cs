using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace AgendaLibrary
{

    public static class Network
    {
        private static readonly string _serviceAccountEmail = "177600524325-6sbfabk2ldv0ickbpjtvfjqbieagg289@developer.gserviceaccount.com";
        private static readonly string _privateKeyPassword = "notasecret";
        internal static readonly string _agendaId = "roosterify@gmail.com";
        private static readonly string ApplicationName = "Roosterify";
        private static Events _agenda;
        private static CalendarService _service;
        private static CalendarService _userService;
        private static Userinfoplus _userInfo;


        static Network()
        {
            _service = GoogleCalendar();
            GetUserService();
        }

        private static void GetUserService()
        {
            string ClientId = "177600524325-le2ar5m4qoffvdadptka06gkpc92eht6.apps.googleusercontent.com";
            string ClientSecret = "a4u6ROKjicMbegGAmYzJLlEP";

            UserCredential credential2 = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets { ClientId = ClientId, ClientSecret = ClientSecret },
                new[] { CalendarService.Scope.Calendar, Google.Apis.Oauth2.v2.Oauth2Service.Scope.UserinfoEmail},
                "user", 
                CancellationToken.None,
                new FileDataStore("Drive.Auth.Store")
            ).Result;

            var oauthSerivce = new Google.Apis.Oauth2.v2.Oauth2Service(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential2,
                    ApplicationName = ApplicationName,
                });

            _userInfo = oauthSerivce.Userinfo.Get().Execute();

            _userService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential2,
                ApplicationName = ApplicationName,
            });
        }

        private static CalendarService GoogleCalendar()
        {
            var certificate = new X509Certificate2(Properties.Resources.Roosterify_b75ac5863d29, _privateKeyPassword, X509KeyStorageFlags.Exportable);

            var credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(_serviceAccountEmail)
                {
                    Scopes = new[] { CalendarService.Scope.Calendar }
                }.FromCertificate(certificate));

            return new CalendarService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });
        }

        public static bool GetData()
        {
            _service = GoogleCalendar();
            _agenda = _service.Events.List(_agendaId).Execute();
            return true;
        }

        public static Events GetAgenda()
        {
            return _agenda;
        }

        public static CalendarService GetCalendar()
        {
            return _service;
        }

        public static CalendarService GetUserCalendarService()
        {
            return _userService;
        }

        public static Userinfoplus GetUserInfoService()
        {
            return _userInfo;
        }

    }
}
