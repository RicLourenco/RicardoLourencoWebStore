﻿using Microsoft.AspNetCore.Mvc;
using RicardoLourencoWebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Helpers.Interfaces
{
    public interface IMailHelper
    {
        void SendMail(string to, string subject, string body);

        void SendInvoiceMail(string to, DeliveryViewModel model, FileStreamResult invoice);
    }
}
