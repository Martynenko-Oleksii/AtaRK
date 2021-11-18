using AtaRK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK_Back.Services
{
    public interface IMailService
    {
        object SendApplication(ShopApplication application);

        object SendTechAnswer(TechMessage techMessage);
    }
}
