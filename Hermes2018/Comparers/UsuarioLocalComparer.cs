using Hermes2018.Helpers;
using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Comparers
{
    public class UsuarioLocalComparer : IEqualityComparer<UsuarioLocalJsonModel>
    {
        public bool Equals(UsuarioLocalJsonModel x, UsuarioLocalJsonModel y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.HER_Titular == y.HER_Titular && x.HER_Tipo == ConstRol.Rol7T && y.HER_Tipo == ConstRol.Rol8T)
                return true;
            else
                return false;
        }

        public int GetHashCode(UsuarioLocalJsonModel obj)
        {
            //Comprueba que los valores a comparar sea iguales
            return obj.HER_Titular.GetHashCode();
        }
    }
}
