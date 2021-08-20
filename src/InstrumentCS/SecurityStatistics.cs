using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Instrument
{
    public class SecurityStatistics
    {
        public SecurityStatistics(Security security)
        {
            // PROPERTIES //
            Security = security;
            Volume = 0;
            Last = 0;
            Low = long.MaxValue;
            High = long.MinValue;
        }

        public void SetLast(long last)
        {
            Last = last;
        }

        public bool TrySetHigh(long potentialHigh)
        {
            if (potentialHigh > High)
            {
                High = potentialHigh;
                return true;
            }
            return false;
        }

        public bool TrySetLow(long potentialLow)
        {
            if (potentialLow < Low)
            {
                Low = potentialLow;
                return true;
            }
            return false;
        }

        public void AddVolume(uint quantity)
        {
            Volume += quantity; 
        }

        // PROPERTIES //
        public uint Volume { get; private set; }
        public long Last { get; private set; }
        public long Low { get; private set; }
        public long High { get; private set; }
        public Security Security { get; private set; }
    }
}
