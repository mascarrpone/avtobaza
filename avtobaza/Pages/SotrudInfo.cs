using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avtobaza.Pages
{
    public class SotrudInfo
    {
        public decimal IDUsera { get; set; }  
        public decimal IDSotrudnika { get; set; }
        public string UserRol { get; set; }
        public string UserFIO { get; set; }
        public decimal UserPhone { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public decimal? UserPassport { get; set; }
        public decimal? Prava { get; set; }
        public string Login { get; set; }
        public string Parol { get; set; }
       
    }

}
