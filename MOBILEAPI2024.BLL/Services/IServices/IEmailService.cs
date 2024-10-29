using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.BLL.Services.IServices
{
    public interface IEmailService
    {
        public void SendEmail(string aRecipientName, string aRecipientAddress, string aSubject, string aText);
    }
}
