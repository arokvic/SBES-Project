﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [ServiceContract]
    public interface IServices
    {
        [OperationContract]
        void Ping(string message, byte[] sign);
    }
}
