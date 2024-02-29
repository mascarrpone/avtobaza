using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

using ConsoleApp.Model;

namespace HashPasswords
{
    public class Helper
    {
        public static avtobazaEntities4 getContext()
        {
            return new avtobazaEntities4();
        }
        public void CreateUsers(ConsoleApp.Model.Users users)
        {
            using (var context = new avtobazaEntities4())
            {
                context.Users.Add(users);
                context.SaveChanges();
            }
        }

        public void UpdateUsers(ConsoleApp.Model.Users users)
        {
            using (var context = new avtobazaEntities4())
            {
                context.Entry(users).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void RemoveUsers(int IDUsera)
        {
            using (var context = new avtobazaEntities4())
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
