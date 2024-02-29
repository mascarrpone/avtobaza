
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace HashPasswords
{
    public class Helper
    {
        private static avtobazaEntities s_avtobazaEntities;
        public static avtobazaEntities getContext()
        {
            if (s_avtobazaEntities == null)
            {
                s_avtobazaEntities = new avtobazaEntities();
            }
            return s_avtobazaEntities;
        }

        public void CreateUsers(Model.Users users)
        {
            s_avtobazaEntities.User.Add(users);
            s_avtobazaEntities.SaveChanges();
        }

        public void UpdateUsers(Model.Users users)
        {
            s_avtobazaEntities.Entry(users).State = EntityState.Modified;
            s_avtobazaEntities.SaveChanges();
        }

        public void RemoveUsers(int IDUsera)
        {
            var user = s_avtobazaEntities.User.Find(IDUsera);
            s_avtobazaEntities.User.Remove(user);
            s_avtobazaEntities.SaveChanges();
        }

        public void FiltrUsers()
        {
            var users = s_avtobazaEntities.User.Where(x => x.Login.StartsWith("M"));
        }

        public void SortUsers()
        {
            var users = s_avtobazaEntities.User.OrderBy(x => x.Login);
        }

    }
}
