using System;
using System.Collections.Generic;
using System.Text;

namespace TreasureMap.Domain
{
    public class Treasure
    {
        public Position Position { get; init; }
        public int Count { get; set; }
    }
}
