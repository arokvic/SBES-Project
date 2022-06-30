using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();

            string port = null;
            int result = 0;
            do
            {
                Console.WriteLine("\nUnesi port izmedju 9980 i 9998\n");
                port = Console.ReadLine();
                try
                {
                    result = Convert.ToInt32(port);
                }
                catch (OverflowException)
                {
                    Console.WriteLine("{0} is outside the range of the Int32 type.", port);
                }
                catch (FormatException)
                {
                    Console.WriteLine("The {0} value '{1}' is not in a recognizable format.",
                                      port.GetType().Name, port);
                }
            } while (result < 9980 || result > 9998);

            string address = "net.tcp://localhost:"+port+"/DBM";


            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            Console.WriteLine("Korisnik koji je pokrenuo klijenta je : " + WindowsIdentity.GetCurrent().Name);

            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address),
                EndpointIdentity.CreateUpnIdentity("clsclient"));


            using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
            {
                bool dalje = true;
                while (dalje)
                {
                    int choice = 0;

                    Console.WriteLine("\nMENI\n");
                    Console.WriteLine("\t1. Kreirajte folder\n");
                    Console.WriteLine("\t2. Kreirajte fajl\n");
                    Console.WriteLine("\t3. Obrisi folder\n");
                    Console.WriteLine("\t4. Obrisi fajl\n");
                    Console.WriteLine("\t5. Izmeni fajl\n");
                    Console.WriteLine("\t6. Citaj fajl\n");
                    Console.WriteLine("\t7. Pregledaj fajlove\n");
                    Console.WriteLine("\t8. EXIT\n");

                    do
                    {
                        try
                        {
                            choice = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    } while (choice == 0);

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("\tUnesi naziv foldera:\n");
                            string path = Console.ReadLine();
                            try
                            {
                                if (proxy.CreateNewFolder(path))
                                    Console.WriteLine("\tFolder sa nazivom '" + path + "' kreiran\n");
                                else
                                    Console.WriteLine("\tNeuspesno kreiran folder\n");
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\tERROR: " + e.Message + "\n");
                                break;
                            }
                        case 2:
                            Console.WriteLine("\tUnesi naziv fajla:\n");
                            string path2 = Console.ReadLine();
                            Console.WriteLine("\tUnesite tekst:\n");
                            string txt = Console.ReadLine();
                            try
                            {
                                if (proxy.CreateFile(path2, txt))
                                    Console.WriteLine("\tFajl sa nazivom '" + path2 + "' kreiran\n");
                                else
                                    Console.WriteLine("\tFajl neuspesno kreiran\n");
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\tERROR: " + e.Message + "\n");
                                break;
                            }
                        case 3:
                            Console.WriteLine("\tUnesi naziv foldera koji zelite obrisati:\n");
                            string path3 = Console.ReadLine();
                            try
                            {
                                if (proxy.DeleteFolder(path3))
                                    Console.WriteLine("\tFolder sa nazivom '" + path3 + "' obrisan\n");
                                else
                                    Console.WriteLine("\tFolder neuspesno obrisan\n");
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\tERROR: " + e.Message + "\n");
                                break;
                            }
                        case 4:
                            Console.WriteLine("\tUnesi naziv fajla koji zelite obrisati:\n");
                            string path4 = Console.ReadLine();
                            try
                            {
                                if (proxy.DeleteFile(path4))
                                    Console.WriteLine("\tFajl sa nazivom '" + path4 + "' obrisan\n");
                                else
                                    Console.WriteLine("\tFajl neuspesno obrisan\n");
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\tERROR: " + e.Message + "\n");
                                break;
                            }
                        case 5:
                            Console.WriteLine("\tUnesi naziv fajla koji zelite izmeniti:\n");
                            string path5 = Console.ReadLine();
                            Console.WriteLine("\tUnesi tekst koji zelite dodati\n");
                            string txt2 = Console.ReadLine();
                            try
                            {
                                if (proxy.AppendFile(path5, txt2))
                                    Console.WriteLine("\tFajl sa nazivom '" + path5 + "' izmenjen\n");
                                else
                                    Console.WriteLine("\tFajl neuspesno izmenjen\n");
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\tERROR: " + e.Message + "\n");
                                break;
                            }
                        case 6:
                            Console.WriteLine("\tUnesi naziv fajla koji zelite procitati:\n");
                            string path6 = Console.ReadLine();
                            try
                            {
                                Console.WriteLine("\tTEKST:\n");
                                Console.WriteLine(proxy.Read(path6) + "\n");
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\tERROR: " + e.Message + "\n");
                                break;
                            }
                        case 7:
                            try
                            {
                                Console.WriteLine("\tFajlovi:\n");
                                Console.WriteLine(proxy.ViewFiles() + "\n");
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\tERROR: " + e.Message + "\n");
                                break;
                            }
                        case 8:
                            dalje = false;
                            break;
                        default:
                            Console.WriteLine("\tNEVALIDAN UNOS\n");
                            break;
                    }
                }
            }
        }
    }
}
