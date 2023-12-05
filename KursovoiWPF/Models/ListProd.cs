using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;
using KursovoiWPF;
using System.Security.Principal;

namespace KursovoiWPF.Model
{
    public class ListProd : ObservableCollection<Product>
    {
        public ListProd()
        {
            DbSet<Product> accs = PageMain.DataEntities.Product;
            var queryAcc = (from acc in accs
                            select acc);
            foreach (Product accc in queryAcc)
            {
                this.Add(accc);
            }
        }
    }
}