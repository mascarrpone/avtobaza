using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using avtobaza.Model;

namespace HashPasswords
{
    public class Helper
    {
        public static avtobazaEntities1 getContext()
        {
            return new avtobazaEntities1();
        }
        public void CreateUsers(avtobaza.Model.Users users)
        {
            using (var context = new avtobazaEntities1())
            {
                context.Users.Add(users);
                context.SaveChanges();
            }
        }

        public void UpdateUsers(avtobaza.Model.Users users)
        {
            using (var context = new avtobazaEntities1())
            {
                context.Entry(users).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void RemoveUsers(int IDUsera)
        {
            using (var context = new avtobazaEntities1())
            {
                var user = context.Users.Find(IDUsera);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
            }
        }
    }
}
