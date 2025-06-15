using Lastik.Helpers.Logging;
using Lastik.Models.Cart.Entities;
using Lastik.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application = System.Windows.Application;

namespace Lastik.Models.Loggining
{
    public class UserSessionStore
    {
        private UserSession _userSession;
        private IApiHttpClient _httpClient;
        private ILoggingService _loggingService;
        private TouchAndMouseTracker _touchAndMouseTracker;
        private int _terminalId;
        private int _inactivityTime;

        public UserSessionStore(
            IApiHttpClient apiHttpClient, 
            ILoggingService logger, 
            int terminalId,
            int inactivityTime)
        {
            _userSession = CreateUserSession();
            _httpClient = apiHttpClient;
            _loggingService = logger;
            _terminalId = terminalId;
        }

        public void InitTrackTouch()
        {
            if (Application.Current.MainWindow != null)
            {
                _touchAndMouseTracker = new TouchAndMouseTracker(Application.Current.MainWindow);
            }
        }

        public void AddAction(string? action, int actionObjectId = 0)
        {
            var userAction = new UserAction
            {
                ObjectName = action,
                DateAt = DateTime.Now
            };
            var touch = _touchAndMouseTracker.GetLastInputPosition();
            if (touch != null)
            {
                userAction.Coordinates = $"X:{touch.Value.X} Y:{touch.Value.Y}";
            }

            userAction.Response = "Success";
            _userSession.AllEvent.Add(userAction);
        }

        public async void SendAsync()
        {
            try
            {
                if (_userSession.AllEvent.Count == 0)
                    return;

                _userSession.StartAt = _userSession.AllEvent.FirstOrDefault()!.DateAt;
                _userSession.EndAt = DateTime.Now.AddSeconds(-_inactivityTime);
                _userSession.TerminalId = _terminalId;
                var result = (await _httpClient.SendUserSession(_userSession)).GetContent(_loggingService);
                _userSession = CreateUserSession();
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private UserSession CreateUserSession()
        {
            return new UserSession
            {
                TerminalId = _terminalId,
                AllEvent = []
            };
        }
    }
}
