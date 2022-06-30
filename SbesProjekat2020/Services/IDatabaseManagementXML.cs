using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [ServiceContract]
    public interface IDatabaseManagementXML
    {
        [OperationContract]
        void WriteInXml(string path, string user, string currentTime, string message);
    }
}
