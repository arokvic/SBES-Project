using Manager;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CLSServer
{
    public class Services : IServices
    {

        static internal List<CriticalLevel> criticalLevels = new List<CriticalLevel>();
        static internal List<CriticalLevel> criticalLevelsHigh = new List<CriticalLevel>();


        public void Ping(string message, byte[] sign)
        {
            string clienName = Formatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);


            string clientNameSign = clienName + "_sign";
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,
                StoreLocation.LocalMachine, clientNameSign);

            /// Verify signature using SHA1 hash algorithm
            DigitalSignature.Verify(message, HashAlgorithm.SHA1, sign, certificate);

            string[] words = message.Split(',');

            CriticalLevel critical = new CriticalLevel { User = words[1], DatumIVreme = DateTime.ParseExact(words[2], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Type = words[0] };
            if (critical.Type.Equals("FAILED"))
            {
                criticalLevels.Add(critical);
                criticalLevelsHigh.Add(critical);
            }

            do
            {
                try
                {
                    if (HighLevelCheck(critical))
                    {
                        Console.WriteLine("\nHIGH LEVEL ALERT\nUser => " + critical.User + " access denied");
                        break;
                    }
                    if (MediumLevelCheck(critical))
                    {
                        Console.WriteLine("\nMEDIUM LEVEL ALERT\nUser => " + critical.User + " access denied");
                        break;
                    }
                    if (LowLevelCheck(critical))
                    {
                        Console.WriteLine("\nLOW LEVEL ALERT\nUser => " + critical.User + " access denied");
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            } while (false);

            try
            {
                using (StreamWriter sw = File.AppendText("logging.txt"))
                {
                    sw.WriteLine(words[0] + " " + "User: " + words[1] + " at " + words[2].ToString() + " " + words[3]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);

            }
        }

        public bool LowLevelCheck(CriticalLevel cl)
        {
            if (cl.Type.Equals("FAILED"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MediumLevelCheck(CriticalLevel cl)
        {
            int mediumAlarm = 0;
            System.TimeSpan diff1;
            for (int i = 0; i < criticalLevels.Count; i++)
            {
                if (cl.User.Equals(criticalLevels[i].User))
                {
                    diff1 = cl.DatumIVreme.Subtract(criticalLevels[i].DatumIVreme);
                    if (diff1.Seconds <= TimeSpan.FromSeconds(15).Seconds && (diff1.Seconds != 0))
                    {
                        mediumAlarm++;
                        //criticalLevels.Add(cl);

                        if (mediumAlarm == 3)
                        {
                            foreach (CriticalLevel c in criticalLevels.ToList())
                            {
                                if (c.User.Equals(cl.User))
                                    criticalLevels.Remove(c);
                            }
                            mediumAlarm = 0;
                            return true;
                        }

                    }
                }
            }
            mediumAlarm = 0;
            return false;
        }

        public bool HighLevelCheck(CriticalLevel cl)
        {
            int highAlarm = 0;
            System.TimeSpan diff1;
            for (int i = 0; i < criticalLevelsHigh.Count; i++)
            {
                if (cl.User.Equals(criticalLevelsHigh[i].User))
                {
                    diff1 = cl.DatumIVreme.Subtract(criticalLevelsHigh[i].DatumIVreme);
                    if (diff1.Seconds <= TimeSpan.FromSeconds(15).Seconds && (diff1.Seconds != 0))
                    {
                        highAlarm++;
                        //criticalLevels.Add(cl);

                        if (highAlarm == 4)
                        {
                            foreach (CriticalLevel c in criticalLevelsHigh.ToList())
                            {
                                if (c.User.Equals(cl.User))
                                    criticalLevelsHigh.Remove(c);
                            }
                            highAlarm = 0;
                            return true;
                        }

                    }
                }
            }
            highAlarm = 0;
            return false;
        }

    }
}
