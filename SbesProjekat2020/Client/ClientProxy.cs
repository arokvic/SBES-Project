using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientProxy : ChannelFactory<IClientServices>, IClientServices, IDisposable
    {
        IClientServices factory;

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }
        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public bool AppendFile(string path, string tekst)
        {
            try
            {
                if (factory.AppendFile(path, tekst))
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;

            }
        }

        public bool CreateFile(string path, string tekst)
        {
            try
            {
                if (factory.CreateFile(path, tekst))
                    return true;
                else
                    return false;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;

            }
        }

        public bool CreateNewFolder(string path)
        {
            try
            {
                if (factory.CreateNewFolder(path))
                    return true;
                else
                    return false;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;

            }
        }

        public bool DeleteFile(string path)
        {
            try
            {
                if (factory.DeleteFile(path))
                    return true;
                else
                    return false;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;

            }
        }

        public bool DeleteFolder(string path)
        {
            try
            {
                if (factory.DeleteFolder(path))
                    return true;
                else
                    return false;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;
            }
        }

        public string Read(string path)
        {
            string s = null;
            try
            {
                s = factory.Read(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            return s;
        }

        public string ViewFiles()
        {
            string s = null;
            try
            {
                s = factory.ViewFiles();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            return s;
        }
    }
}
