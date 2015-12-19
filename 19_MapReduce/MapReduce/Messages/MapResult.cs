using System;
using System.Collections.Generic;
using System.Linq;

namespace MapReduce.Messages
{
    public class MapResult
    {
        public List<LanguageCount> Counts { get; set; }

        public MapResult()
        {
            Counts = new List<LanguageCount>();
        }

        public override string ToString()
        {
            return string.Format("[MapResult: {0}]", 
                String.Join(", ", 
                    Counts.Select(c => String.Format("{0}:{1}", c.Language, c.Count))
                )
            );
        }
    }
}
