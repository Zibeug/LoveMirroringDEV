using mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Services
{
    class ConnectionsSingleton
    {
        private static ConnectionsSingleton _instance = null;
        private static List<ConnectionPC> _connectionList = null;
        private ConnectionsSingleton()
        {
            _connectionList = new List<ConnectionPC>();
        }
        static public List<ConnectionPC> GetConnectionList()
        {
            if (_instance == null)
            {
                _instance = new ConnectionsSingleton();
            }
            return _connectionList;
        }
    }
}
