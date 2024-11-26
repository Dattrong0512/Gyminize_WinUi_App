using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gyminize.Core.Services;

namespace Gyminize.Contracts.Services;
public interface IApiServicesClient
{
    T Get<T>(string endpoint);
    T Post<T>(string endpoint, object data);
    T Put<T>(string endpoint, object data);
    bool Delete(string endpoint, object data);
}

