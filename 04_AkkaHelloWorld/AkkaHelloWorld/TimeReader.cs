using System;
using Akka.Actor;

namespace AkkaHelloWorld
{
    public class TimeReader : ReceiveActor
    {
        private Random random;

        public TimeReader()
        {
            random = new Random();

            Become(TruthTelling);
        }


        #region behaviors
        private void TruthTelling()
        {
            Receive<string>(s => TellCorrectTime(s));
        }

        private void Lying()
        {
            Receive<string>(s => TellAnyTime(s));
        }
        #endregion

        #region message handlers
        private void TellCorrectTime(string _)
        {
            Console.WriteLine("Current time is: {0:hh:mm} -- really", DateTime.Now);
            if (random.Next(100) < 50)
                Become(Lying);
        }

        private void TellAnyTime(string _)
        {
            Console.WriteLine("Current time is: 11:92) -- do you believe it?");
            Become(TruthTelling);
        }
        #endregion
    }
}

