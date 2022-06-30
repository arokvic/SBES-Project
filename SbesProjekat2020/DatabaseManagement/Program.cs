using Manager;
using Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseManagement
{
    public class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = "clsserver";

            string signCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name) + "_sign";


            string port = null;
            int result = 0;
            do
            {
                Console.WriteLine("\nUnesi port izmedju 9980 i 9998\n");
                port = Console.ReadLine();
                try
                {
                    result = Convert.ToInt32(port);
                }catch(OverflowException) {
                    Console.WriteLine("{0} is outside the range of the Int32 type.", port);
                }
                catch (FormatException)
                {
                    Console.WriteLine("The {0} value '{1}' is not in a recognizable format.",
                                      port.GetType().Name, port);
                }
            } while (result < 9980 || result > 9998);
           



            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/ClsServer"),
                                      new X509CertificateEndpointIdentity(srvCert));

            
            Console.WriteLine("Korisnik koji je pokrenuo servera :" + WindowsIdentity.GetCurrent().Name);
            
            Thread Server = new Thread(() => ServerThread(port));
            Server.Start();

            using (ClientProxy proxy = new ClientProxy(binding, address))
            {              
                
            }          
            Console.ReadLine();
        }

        static void ServerThread(string port)
        {

            NetTcpBinding binding2 = new NetTcpBinding();
            string address2 = "net.tcp://localhost:" + port + "/DBM";

            binding2.Security.Mode = SecurityMode.Transport;
            binding2.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding2.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host = new ServiceHost(typeof(ClientServices));
            host.AddServiceEndpoint(typeof(IClientServices), binding2, address2);
            // podesavamo da se koristi MyAuthorizationManager umesto ugradjenog
            host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            // podesavamo custom polisu, odnosno nas objekat principala
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            // TO DO : podesavanje AutidBehaviour-a
            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAudit);

            try
            {

                host.Open();
                Console.WriteLine("DatabaseManagement is started.\n Press enter to quit...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                host.Close();

            }


        }

    }
}
