using System;
using System.Linq;
using Akka.Actor;
using SudokuSolver.Actors;
using SudokuSolver.Messages;
using System.Collections.Generic;

namespace SudokuSolver
{
    public class StatisticsCollector : SudokuActor
    {
        private Dictionary<string, int> statistics;

        public StatisticsCollector(IActorRef printer)
            :base(printer)
        {
            statistics = new Dictionary<string, int>();

            SetReceiveTimeout(TimeSpan.FromSeconds(1));

            Receive<SetDigit>(s => UpdateStatistics(s));

            Receive<ReceiveTimeout>(_ =>
                {
                    var row = 38;
                    statistics.Keys.ToList().ForEach(key =>
                        printer.Tell(new PrintLine(row++, 0, String.Format("{0}: {1}", key, statistics[key])))
                    );
                }
            );
        }

        private void UpdateStatistics(object message)
        {
            var type = message.GetType().Name;

            if (statistics.ContainsKey(type))
            {
                statistics[type]++;
            }
            else
            {
                statistics.Add(type, 1);
            }
        }
    }
}

