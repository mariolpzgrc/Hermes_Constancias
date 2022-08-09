using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class ConstanciaViewModel
    {
        [BindProperty]
        [HiddenInput]
        public string sNumPer { get; set; }

        [BindProperty]
        [HiddenInput] public string sNombre { get; set; }
        public string dFIngreso { get; set; }
        [BindProperty]
        [HiddenInput]
        public string sNumDep { get; set; }
        [BindProperty]
        [HiddenInput]
        public string sDescDep { get; set; }

        [BindProperty]
        [HiddenInput] 
        public int sRegion { get; set; }
        [BindProperty]
        [HiddenInput]
        public string sDesRegion { get; set; }
        [BindProperty]
        [HiddenInput] 
        public int sTPE { get; set; }

        [BindProperty]
        [HiddenInput]
        public string sDesTPE { get; set; }

        [BindProperty]
        [HiddenInput] public int NumCont { get; set; }
        public string sDesCont { get; set; }

        [BindProperty]
        [HiddenInput] 
        public int sNumPuesto { get; set; }

        [BindProperty]
        [HiddenInput] 
        public string sDescPuesto { get; set; }
        
        [BindProperty]
        [HiddenInput]
        public int sNumCat { get; set; }

        [BindProperty]
        [HiddenInput] 
        public string sDescCat { get; set; }
        
        [BindProperty]
        [HiddenInput]
        public string sSuelPrest { get; set; }

        [BindProperty]
        [HiddenInput]
        public string sParentesco { get; set; }

        [BindProperty]
        [HiddenInput]
        public string sNomDepen { get; set; }

        [BindProperty]
        [HiddenInput]
        public string sDirecPers { get; set; }

        [BindProperty]
        [HiddenInput]
        public string sNPeri { get; set; }

        [BindProperty]
        [HiddenInput]
        public string sHrs { get; set; }
    }
}
