using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DoctorLRweb
{
    public interface IAuth
    {
        string Authentication(string identifier, string password, string role);
    }
}
