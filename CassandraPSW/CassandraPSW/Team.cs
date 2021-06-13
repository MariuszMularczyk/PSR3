using Cassandra.Data.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CassandraPSW
{

    class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Loses { get; set; }

    }
}
