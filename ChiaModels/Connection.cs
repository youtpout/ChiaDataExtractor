using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaModels
{
    public class Connection
    {
        public int bytes_read { get; set; }
        public int bytes_written { get; set; }
        public double creation_time { get; set; }
        public double last_message_time { get; set; }
        public int local_port { get; set; }
        public string node_id { get; set; }
        public string peer_host { get; set; }
        public int peer_port { get; set; }
        public int peer_server_port { get; set; }
        public int type { get; set; }
    }

    public class RootConnection
    {
        public List<Connection> connections { get; set; }
        public bool success { get; set; }
    }

}
