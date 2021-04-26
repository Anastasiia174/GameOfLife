using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GameOfLife.Infrastructure;

namespace GameOfLife.Services
{
    public class GameGameLogger : IGameLogger
    {
        public void LogInfo(string message)
        {
            var logMessage = new LogEventMessage(new GameLog()
            {
                Event = message,
                EventDateTime = DateTime.Now
            });

            Messenger.Default.Send(logMessage);
        }

        public void LogInfo(string message, Bitmap playground)
        {
            var logMessage = new LogEventMessage(new GameLog()
            {
                Event = message,
                EventDateTime = DateTime.Now,
                Playground = playground
            });

            Messenger.Default.Send(logMessage);
        }
    }
}
