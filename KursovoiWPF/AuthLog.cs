using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;
using BCrypt.Net;
using System.Windows;
using Org.BouncyCastle.Crypto.Generators;

namespace KursovoiWPF
{
    internal class AuthLog
    {
        public static SmartStoreEntities1 DataEntities { get; set; } = new SmartStoreEntities1();
        public ObservableCollection<Profiles> ListProfiles { get; set; } = new ObservableCollection<Profiles>();

        public void Register(Profiles profile)
        {
            var prof =new Profiles(){
                Login = profile.Login,
                Email = profile.Email,
                Password = profile.Password
            };

            var queryProf = (from profil in DataEntities.Profiles where profil.Login.Contains(prof.Login) select profil).ToList();

            ListProfiles = new ObservableCollection<Profiles>(queryProf);

            if(ListProfiles.Count < 1)
            {
                try
                {
                    DataEntities.Profiles.Add(prof);
                    DataEntities.SaveChanges();

                    var queryProfile = (from prof1 in DataEntities.Profiles where prof1.Email == prof.Email where prof1.Password == prof.Password select prof1).ToList();
                    var prof_ = queryProfile.ToList()[0];

                    DataEntities.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(
                    "Ошибка регистрации" + ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Логин занят!");
            }
        }

        public int Login (string login, string password)
        {
            var queryProf = (from profile in DataEntities.Profiles where profile.Login.Contains(login) select profile).ToList();

            ListProfiles = new ObservableCollection<Profiles>(queryProf);
            
            if (BCrypt.Net.BCrypt.Verify(password, ListProfiles.Last().Password))
                return ListProfiles.Last().ID_Profiles;
            else return -1;
        }
    }
}
