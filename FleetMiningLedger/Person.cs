using System;
using System.Collections.Generic;
using System.Text;

namespace FleetMiningLedger
{
    struct Person
    {
        public string name;
        public string role;
        public TimeSpan time_joined;
        public TimeSpan? time_left;
        public Dictionary<string, int> mining_dictionary;

        public static TimeSpan? getPresence(Person p)
        {
            return p.time_left - p.time_joined;
        }
    }

    
}
