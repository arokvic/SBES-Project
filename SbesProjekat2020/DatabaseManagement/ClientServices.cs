using Manager;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseManagement
{
    public class ClientServices : IClientServices
    {    
        DatabaseXML xml = new DatabaseXML();
        //[PrincipalPermission(SecurityAction.Demand, Role = "ManageFile")]
        public bool AppendFile(string path, string tekst)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);


            if (Thread.CurrentPrincipal.IsInRole("ManageFile"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " successfully accessed to http://tempuri.org/IClientServices/AppendFile");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("SUCCESS", userName, DateTime.Now, "access to AppendFile method is granted."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailure(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, "Append File method need Manage File permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " failed to access http://tempuri.org/IClientServices/AppendFile. Reason: Append File method need Manage File permission,");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("FAILED", userName, DateTime.Now, "access to AppendFile method is denied."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return false;
            }

            IIdentity identity = Thread.CurrentPrincipal.Identity;

            Console.WriteLine("Tip autentifikacije : " + identity.AuthenticationType);

            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            Console.WriteLine("Ime klijenta koji je pozvao metodu : " + windowsIdentity.Name);
            Console.WriteLine("Jedinstveni identifikator : " + windowsIdentity.User);

            string pathString = System.IO.Path.Combine("DatabaseManagement", path);


            try
            {

                using (StreamWriter sw = File.AppendText(pathString + ".txt"))
                {
                    sw.WriteLine(tekst);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return false;
            }

        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "ManageFile")]

        public bool CreateFile(string path, string tekst)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);


            if (Thread.CurrentPrincipal.IsInRole("ManageFile"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " successfully accessed to http://tempuri.org/IClientServices/CreateFile");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("SUCCESS", userName, DateTime.Now, "access to CreateFile method is granted."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailure(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, "Create method need Manage File permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " failed to access http://tempuri.org/IClientServices/CreateFile. Reason: CreateFile method need Manage File permission,");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("FAILED", userName, DateTime.Now, "access to CreateFile method is denied."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return false;
            }


            IIdentity identity = Thread.CurrentPrincipal.Identity;

            Console.WriteLine("Tip autentifikacije : " + identity.AuthenticationType);

            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            Console.WriteLine("Ime klijenta koji je pozvao metodu : " + windowsIdentity.Name);
            Console.WriteLine("Jedinstveni identifikator : " + windowsIdentity.User);

            string pathString = System.IO.Path.Combine("DatabaseManagement", path);


            try
            {
                if (!System.IO.File.Exists(pathString))
                {
                    System.IO.File.Create(pathString + ".txt").Close();
                    using (StreamWriter sw = new StreamWriter(pathString + ".txt"))
                    {
                        sw.WriteLine(tekst);
                    }

                    return true;
                }
                else
                {
                    Console.WriteLine("File \"{0}\" already exists.", path);
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return false;

            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "ManageFolder")]
        public bool CreateNewFolder(string path)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("ManageFolder"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " successfully accessed to http://tempuri.org/IClientServices/CreateNewFolder");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("SUCCESS", userName, DateTime.Now, "access to CreateNewFolder method is granted."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailure(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, "CreateNewFolder method need Manage Folder permission");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " failed to access http://tempuri.org/IClientServices/CreateNewFolder. Reason: CreateNewFolder method need Manage Folder permission,");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("FAILED", userName, DateTime.Now, "access to CreateNewFolder method is denied."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return false;
            }
            IIdentity identity = Thread.CurrentPrincipal.Identity;

            Console.WriteLine("Tip autentifikacije : " + identity.AuthenticationType);

            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            Console.WriteLine("Ime klijenta koji je pozvao metodu : " + windowsIdentity.Name);
            Console.WriteLine("Jedinstveni identifikator : " + windowsIdentity.User);
            string pathString = System.IO.Path.Combine("DatabaseManagement", path);



            try
            {
                System.IO.Directory.CreateDirectory(pathString);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return false;
            }
        }
        // [PrincipalPermission(SecurityAction.Demand, Role = "ManageFile")]
        public bool DeleteFile(string path)
        {

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);


            if (Thread.CurrentPrincipal.IsInRole("ManageFile"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " successfully accessed to http://tempuri.org/IClientServices/DeleteFile");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("SUCCESS", userName, DateTime.Now, "access to DeleteFile method is granted."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailure(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, "DeleteFile method need Manage File permission");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " failed to access http://tempuri.org/IClientServices/DeleteFile. Reason: DeleteFile method need Manage File permission,");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("FAILED", userName, DateTime.Now, "access to DeleteFile method is denied."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return false;
            }
            IIdentity identity = Thread.CurrentPrincipal.Identity;

            Console.WriteLine("Tip autentifikacije : " + identity.AuthenticationType);

            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            Console.WriteLine("Ime klijenta koji je pozvao metodu : " + windowsIdentity.Name);
            Console.WriteLine("Jedinstveni identifikator : " + windowsIdentity.User);
            string pathString = System.IO.Path.Combine("DatabaseManagement", path);

            try
            {
                System.IO.File.Delete(pathString + ".txt");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return false;
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "ManageFolder")]

        public bool DeleteFolder(string path)
        {


            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("ManageFolder"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " successfully accessed to http://tempuri.org/IClientServices/DeleteFolder");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("SUCCESS", userName, DateTime.Now, "access to DeleteFolder method is granted."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailure(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, "DeleteFolder method need Manage Folder permission.");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " failed to access http://tempuri.org/IClientServices/DeleteFolder. Reason: DeleteFolder method need Manage Folder permission,");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("FAILED", userName, DateTime.Now, "access to DeleteFolder method is failed."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return false;
            }
            IIdentity identity = Thread.CurrentPrincipal.Identity;

            Console.WriteLine("Tip autentifikacije : " + identity.AuthenticationType);

            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            Console.WriteLine("Ime klijenta koji je pozvao metodu : " + windowsIdentity.Name);
            Console.WriteLine("Jedinstveni identifikator : " + windowsIdentity.User);

            string pathString = System.IO.Path.Combine("DatabaseManagement", path);

            try
            {
                System.IO.Directory.Delete(pathString,true);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return false;
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Read")]
        public string Read(string path)
        {

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Read"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " successfully accessed to http://tempuri.org/IClientServices/Read");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("SUCCESS", userName, DateTime.Now, "access to Read method is granted."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailure(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, "Read method need Reader permission.");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " failed to access http://tempuri.org/IClientServices/Read. Reason: Read method need Reader permission,");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("FAILED", userName, DateTime.Now, "access to Read method is failed."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return $"Korisinik nema prava prisutpa";
            }

            IIdentity identity = Thread.CurrentPrincipal.Identity;

            Console.WriteLine("Tip autentifikacije : " + identity.AuthenticationType);

            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            Console.WriteLine("Ime klijenta koji je pozvao metodu : " + windowsIdentity.Name);
            Console.WriteLine("Jedinstveni identifikator : " + windowsIdentity.User);
            string pathString = System.IO.Path.Combine("DatabaseManagement", path);

            string sztr = null;
            try
            {
                using (StreamReader sr = new StreamReader(pathString + ".txt"))
                {
                    sztr = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
            return sztr;
        }

        public string ViewFiles()
        {

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Read"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " successfully accessed to http://tempuri.org/IClientServices/ViewFiles");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("SUCCESS", userName, DateTime.Now, "access to ViewFiles method is granted."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailure(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, "ViewFiles method need Reader permission.");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    xml.WriteInXml("logging.xml", userName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), $"User " + userName + " failed to access http://tempuri.org/IClientServices/ViewFiles. Reason: ViewFiles method need Reader permission,");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    Thread Base = new Thread(() => BaseThread("FAILED", userName, DateTime.Now, "access to ViewFiles method is failed."));
                    Base.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return $"Korisinik nema prava prisutpa";
            }

            IIdentity identity = Thread.CurrentPrincipal.Identity;

            Console.WriteLine("Tip autentifikacije : " + identity.AuthenticationType);

            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            Console.WriteLine("Ime klijenta koji je pozvao metodu : " + windowsIdentity.Name);
            Console.WriteLine("Jedinstveni identifikator : " + windowsIdentity.User);

            DirectoryInfo d = new DirectoryInfo("DatabaseManagement");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files
            string str = "Txt fajlovi: ";
            foreach (FileInfo file in Files)
            {
                str = str + file.Name + ", ";
            }

            string[] dirs = Directory.GetDirectories("DatabaseManagement");
            string dir = "\nFolderi: ";
            foreach(string s in dirs)
            {
                dir = dir + s + ", ";
            }

            

            return str + dir;
        }

        static void BaseThread(string state, string username, DateTime dateTime, string method)
        {
            string srvCertCN = "clsserver";
            string signCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name) + "_sign";
            X509Certificate2 certificateSign = CertManager.GetCertificateFromStorage(StoreName.My,
                                    StoreLocation.LocalMachine, signCertCN);

            string message = state + "," + username + "," + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "," + method;

            byte[] signature = DigitalSignature.Create(message, HashAlgorithm.SHA1, certificateSign);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/ClsServer"),
                                      new X509CertificateEndpointIdentity(srvCert));
            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                proxy.Ping(message, signature);
            }

        }

       
    }
}
