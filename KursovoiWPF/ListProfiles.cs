using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovoiWPF
{
    public class ListProfiles : ObservableCollection<Profiles>
    {
        public ListProfiles() 
        {
        }
    }
}
