using Hermes2018.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class BaseUsuarioOracleViewModel
    {
        public List<string> Encabezado { get; set; }
        public List<string> Tipos { get; set; }
        public List<UsuarioOracleViewModel> Contenido { get; set; }
    }
    public class UsuarioOracleViewModel
    {
        public int NOPER { get; set; }
        public string NOMBRE { get; set; }
        public string SEXO { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public int PIDM { get; set; }
        public int IPE { get; set; }
        public string EDO_CIV { get; set; }
        public string NACIONALIDAD { get; set; }
        public string NIVEL { get; set; }
        public string CALLE { get; set; }
        public string NUMEXT { get; set; }
        public string NUMINT { get; set; }
        public string COLONIA { get; set; }
        //public int CP { get; set; }
        public string CP { get; set; }
        public string CIUDAD { get; set; }
        public string MUNICIPIO { get; set; }
        public string ESTADO { get; set; }
        public string TEL { get; set; }
        public int NTPE { get; set; }
        public string TIPO_PER { get; set; }
        public int NPUE { get; set; }
        public string DPUE { get; set; }
        public int NCAT { get; set; }
        public string CATEGORIA { get; set; }
        public int NCON { get; set; }
        public string TIPO_CONT { get; set; }
        public DateTime INGRESO { get; set; }
        public int ANT { get; set; }
        public int NDEP { get; set; }
        public string DDEP { get; set; }
        public string DCDEP { get; set; }
        public string TDEP { get; set; }
        public int NSUBP { get; set; }
        public string DSUBP { get; set; }
        public string DCSSUBP { get; set; }
        public int NAREA { get; set; }
        public string ISSBP { get; set; }
        public string CORREO { get; set; }
        public string STATUS { get; set; }
        public string SNI { get; set; }
        public string LADA { get; set; }
        public string TELEF_DEP { get; set; }
        public string EXTEN { get; set; }
        public string D_DIREC1 { get; set; }
        public string D_DIR2 { get; set; }
        public string EDIF { get; set; }
        public string PISO { get; set; }
        public string D_CIUDAD { get; set; }
        public string D_CP { get; set; }
        public int NZON { get; set; }
        public string DZON { get; set; }
        public int NSUBZ { get; set; }
        public string SUBREGION { get; set; }
        public string PPROMEP { get; set; }
        public DateTime FNAC { get; set; }
        public string LNAC { get; set; }
        public string TURNO { get; set; }
        public string APAT { get; set; }
        public string AMAT { get; set; }
        public string NOMB { get; set; }
        public string CVELOGIN { get; set; }
        public int NIVC { get; set; }
        public int NPERI { get; set; }
        public DateTime FALT { get; set; }
        public DateTime FBAJ { get; set; }
        public string URL { get; set; }
        public string IURL { get; set; }
        public int NCUR { get; set; }
        public int NMOT { get; set; }
        public string IACTREAL { get; set; }
        public int NNIV { get; set; }
        public int NPROF { get; set; }
        public int NOTORGA { get; set; }
        public int MyProperty { get; set; }
        public Single HRS { get; set; }
        public int NDPAG { get; set; }
        public string DDEPAG { get; set; }
        public int NSUBPAG { get; set; }
        public string DSUBPAG { get; set; }
    }
    public class InfoUsuarioGeneralOracleViewModel
    {
        public int PIDM { get; set; }
        public int NOPER { get; set; }
        public string STATUS { get; set; }
        public string CVELOGIN { get; set; }

        public string NOMBRE { get; set; }
        public string SEXO { get; set; }
        
        public string APAT { get; set; }
        public string AMAT { get; set; }
        public string NOMB { get; set; }

        public string CORREO { get; set; }

        public string LADA { get; set; }
        public string TELEF_DEP { get; set; }

        public string D_DIREC1 { get; set; }
        public string D_CP { get; set; }
        public string D_CIUDAD { get; set; }
        public string ESTADO { get; set; }

        public int NTPE { get; set; }
        public string TIPO_PER { get; set; } 

        public int NPUE { get; set; }
        public string DPUE { get; set; }

        public int NCAT { get; set; }
        public string CATEGORIA { get; set; }
        
        public int NDEP { get; set; }
        public string DDEP { get; set; }

        public int NZON { get; set; }
        public string DZON { get; set; }
    }
    public class InfoUsuarioOracleViewModel 
    {
        [Display(Name = "Usuario")]
        [HiddenInput]
        public string Usuario { get; set; }

        [Display(Name = "Nombre completo")]
        [HiddenInput]
        public string Nombre { get; set; }

        [Display(Name = "Correo")]
        [HiddenInput]
        public string Correo { get; set; }

        [Display(Name = "Área")]
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "required")]
        public string AreaId { get; set; }

        [HiddenInput]
        public string  Area { get; set; }

        [HiddenInput]
        public string Puesto { get; set; }

        public List<InfoAreaViewModel> Areas { get; set; }
    }
}
