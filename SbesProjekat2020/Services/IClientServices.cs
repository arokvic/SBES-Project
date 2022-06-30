using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [ServiceContract]
    public interface IClientServices
    {
        [OperationContract]
        bool CreateNewFolder(string path);

        [OperationContract]
        bool DeleteFolder(string path);

        [OperationContract]
        bool CreateFile(string path, string tekst);

        [OperationContract]
        bool AppendFile(string path, string tekst);

        [OperationContract]
        bool DeleteFile(string path);

        [OperationContract]
        string Read(string path);

        [OperationContract]
        string ViewFiles();
    }
}
