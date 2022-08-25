using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Helpers
{
    public class ConstPagina
    {
        public const int Elementos = 20;
    }
    public class ConstRol
    {
        public const string Rol1T = "Administrador General";
        public const string Rol2T = "Administrador Xalapa";
        public const string Rol3T = "Administrador Veracruz";
        public const string Rol4T = "Administrador Orizaba-Córdoba";
        public const string Rol5T = "Administrador Poza Rica-Tuxpan";
        public const string Rol6T = "Administrador Coatzacoalcos-Minatitlán";
        public const string Rol7T = "Titular";
        public const string Rol8T = "Usuario";
        //public const string Rol9T = "Administrador"; //Administrador de área
        public const string Rol10T = "Administrador Constancias";

        public const int Rol1N = 1;
        public const int Rol2N = 2;
        public const int Rol3N = 3;
        public const int Rol4N = 4;
        public const int Rol5N = 5;
        public const int Rol6N = 6;
        public const int Rol7N = 7;
        public const int Rol8N = 8;
        //public const int Rol9N = 9;
        public const int Rol10N = 10;

        public static string[] RolAdminGral = new string[] { Rol1T };
        public static string[] RolAdminRegional = new string[] { Rol2T, Rol3T, Rol4T, Rol5T, Rol6T };
        public static string[] RolAdmin = new string[] { Rol1T, Rol2T, Rol3T, Rol4T, Rol5T, Rol6T };
        //public static string[] RolAdminArea = new string[] { Rol9T };

        //public static string[] RolUsuarioNormal = new string[] { Rol7T, Rol8T };
        public static string[] RolUsuario = new string[] { Rol7T, Rol8T };
    }
    public class ConstRegion
    {
        public const string Region1T = "Xalapa";
        public const string Region2T = "Veracruz";
        public const string Region3T = "Orizaba-Córdoba";
        public const string Region4T = "Poza Rica-Tuxpan";
        public const string Region5T = "Coatzacoalcos-Minatitlán";

        public const int Region1N = 1;
        public const int Region2N = 2;
        public const int Region3N = 3;
        public const int Region4N = 4;
        public const int Region5N = 5;

        public static int[] RegionesIds = new int[] { Region1N, Region2N, Region3N, Region4N, Region5N };
        public static int[] RegionesIdsSNXalapa = new int[] { Region2N, Region3N, Region4N, Region5N };
    }
    public class ConstArea
    {
        public const int IdA1 = 1;
        public const string NombreA1 = "Rectoría";
        public const string ClaveA1 = "11902";
        public const int DiasCompromisoA1 = 6;
        public const string DireccionA1 = "Lomas del Estadio s/n Edificio \"A\", planta baja, CP. 91000 Xalapa-Enríquez, Veracruz, México";
        public const string TelefonoA1 = "01(228)) 141 1053, 842  17 03";

        public const int IdA2 = 2;
        public const string NombreA2 = "Vicerrectoría Veracruz";
        public const string ClaveA2 = "22901";
        public const int DiasCompromisoA2 = 10;
        public const string DireccionA2 = "Calz Juan Pablo II S/N, Costa Verde, 94294 Veracruz, Ver.";
        public const string TelefonoA2 = "229 775 2000";

        public const int IdA3 = 3;
        public const string NombreA3 = "Vicerrectoría Orizaba-Córdoba";
        public const string ClaveA3 = "31901";
        public const int DiasCompromisoA3 = 10;
        public const string DireccionA3 = "Poniente 7 # 1383 Colonia Centro Orizaba, Veracruz, México";
        public const string TelefonoA3 = "+52 (272) 7263066, +52 (272) 7254596, +52 (272) 7289482, +52 (272) 7289483, +52 (272) 7264590, +52 (272) 7254596 Ext. 33100";

        public const int IdA4 = 4;
        public const string NombreA4 = "Vicerrectoría Poza Rica-Tuxpan";
        public const string ClaveA4 = "41901";
        public const int DiasCompromisoA4 = 10;
        public const string DireccionA4 = "Blvd. Adolfo Ruiz Cortines #306 (entre Justo Sierra y Cedro) Col. Obras Sociales. C.P. 93240";
        public const string TelefonoA4 = "01 782 82 41540 Ext. 41118";

        public const int IdA5 = 5;
        public const string NombreA5 = "Vicerrectoría Coatzacoalcos-Minatitlán";
        public const string ClaveA5 = "51901";
        public const int DiasCompromisoA5 = 10;
        public const string DireccionA5 = "Av. Chihuahua # 803 Col. Petrolera C.P. 96500 Coatzacoalcos, Veracruz";
        public const string TelefonoA5 = "+52 (921) 2115700 ext 55717";

        public static int[] AreasIds = new int[] { IdA1, IdA2, IdA3, IdA4, IdA5 };
        public static int[] AreasIdsSNXalapa = new int[] { IdA2, IdA3, IdA4, IdA5 };
    }
    public class ConstTipoAreaEnRegion
    {
        public const int TipoN1 = 1;
        public const string TipoT1 = "Principal";

        public const int TipoN2 = 2;
        public const string TipoT2 = "Secundaria";
    }
    public class ConstEstadoArea
    {
        public const int EstadoN1 = 1;
        public const string EstadoT1 = "Activa";

        public const int EstadoN2 = 2;
        public const string EstadoT2 = "En proceso de cambio";
        public const string EstadoParaUsurioT2 = "El área a la que pertenece, se encuentra en proceso de cambio";

        public const int EstadoN3 = 3;
        public const string EstadoT3 = "No Activa";
    }
    public class ConstVisibilidad
    {
        public const int VisibilidadN1 = 1;
        public const string VisibilidadT1 = "Público";

        public const int VisibilidadN2 = 2;
        public const string VisibilidadT2 = "Privado";
    }
    public class ConstTipoArea
    {
        public const int TipoN1 = 1;
        public const string TipoT1 = "Área actual";

        public const int TipoN2 = 2;
        public const string TipoT2 = "Subárea";
    }
    public class ConstEstadoAreaPorAgregar
    {
        public const int EstadoN1 = 1;
        public const string EstadoT1 = "Sin agregar";

        public const int EstadoN2 = 2;
        public const string EstadoT2 = "Agregada";
    }
    public class ConstMasterKey
    {
        public const string Key1 = "h#Me5s7St3m@_";
        public const string Key2 = "$H3rm3s#UV!"; //H3rmEs#2
    }
    public class ConstApiMasterKey
    {
        public const string Key = "6VmYu3hh#tW&yFhECpHo8M$HOIyQzEtgZ73N6E1Ijc&bh";
    }
    public class ConstApiUsuario
    {
        public const string Anonimo = "usuarioAnonimo";
    }
    public class ConstKeyApp
    {
        public const string KeyApp = "4ppH3RM3s";
    }
    public class ConstEstadoUsuario
    {
        public const string Estado1T = "Por loguearse";
        public const string Estado2T = "Con credenciales incorrectas";
        public const string Estado3T = "No registrado";
        public const string Estado4T = "No aprobado";
        public const string Estado5T = "Sin aceptar el aviso de privacidad";
        public const string Estado6T = "No está asignado a un área";
        public const string Estado7T = "Logueado";
        //--
        public const string Estado8T = "Cuenta activa como titular";
    }
    public class ConstTerminos
    {
        public const string TerminosSi = "Si";
        public const string TerminosNo = "No";
    }
    public class ConstAprobado
    {
        public const string AprobadoSi = "Si";
        public const string AprobadoNo = "No";

        public const int AprobadoSiN = 1;
        public const int AprobadoNoN = 2;
    }
    public class ConstDelegar
    {
        public const string TipoT1 = "Delegado Editor (Puede crear y enviar correspondencia)";
        public const string TipoT2 = "Delegado Revisor (Puede leer la correspondencia)";

        public const string TipoTS1 = "Editor";
        public const string TipoTS2 = "Revisor";

        public const int TipoN1 = 1;
        public const int TipoN2 = 2;
    }
    public class ConstTipoDocumento
    {
        public const string TipoDocumentoT1 = "Oficio";
        public const string TipoDocumentoT2 = "Memorándum";

        public const int TipoDocumentoN1 = 1;
        public const int TipoDocumentoN2 = 2;
    }
    public class ConstImportancia
    {
        public const string ImportanciaT1 = "Alta";
        public const string ImportanciaT2 = "Normal";

        public const int ImportanciaN1 = 1;
        public const int ImportanciaN2 = 2;
    }
    public class ConstEstadoDocumento
    {
        public const string EstadoDocumentoT1 = "En borrador";
        public const string EstadoDocumentoT2 = "En revision";

        public const int EstadoDocumentoN1 = 1;
        public const int EstadoDocumentoN2 = 2;
    }
    public class ConstEstadoRevision
    {
        //[Revisión Remitente]
        public const string EstadoRemitenteT1 = "Enviado a revisión";
        public const string EstadoRemitenteT2 = "Revisado";
        //--
        public const int EstadoRemitenteN1 = 1;
        public const int EstadoRemitenteN2 = 2;

        //[Revisión Destinatario]
        public const string EstadoDestinatarioT1 = "Solicitud de revisión";
        public const string EstadoDestinatarioT2 = "VoBo de revisión";
        //--
        public const int EstadoDestinatarioN1 = 3;
        public const int EstadoDestinatarioN2 = 4;
    }
    public class ConstVisualizacionRevision
    {
        //[Revisión Remitente]
        public const int VisualizacionRemitente = 1; //Edición completa

        //[Revisión Destinatario]
        public const int VisualizacionDestinatario = 2; //Edición limitada
    }
    public class ConstVisualizacionEnvio
    {
        //[Destinatario]
        public const int Recepcion = 1;

        //[Remitente]
        public const int Envio = 2;
    }
    public class ConstTipoEnvioRevision
    {
        public const string TipoEnvioRevisionT1 = "Enviado";
        public const string TipoEnvioRevisionT2 = "Recibido";

        public const int TipoEnvioRevisionN1 = 1;
        public const int TipoEnvioRevisionN2 = 2;
    }
    public class ConstEstadoEnvio
    {
        /* Atendido,Extemporáneo,Vencido,Completo,Parcialmente Completo,Incompleto,Sin Respuesta */
        public const string EstadoEnvioT1 = "En proceso";
        public const string EstadoEnvioT2 = "Atendido";
        public const string EstadoEnvioT3 = "Extemporáneo";
        public const string EstadoEnvioT4 = "Vencido";
        public const string EstadoEnvioT5 = "Contestado parcialmente"; //Incompleto
        public const string EstadoEnvioT6 = "Contestado completamente"; //Completo
        public const string EstadoEnvioT7 = "No requiere respuesta";
        public const string EstadoEnvioT8 = "Respuesta";

        public const int EstadoEnvioN1 = 1;
        public const int EstadoEnvioN2 = 2;
        public const int EstadoEnvioN3 = 3;
        public const int EstadoEnvioN4 = 4;
        public const int EstadoEnvioN5 = 5;
        public const int EstadoEnvioN6 = 6;
        public const int EstadoEnvioN7 = 7;
        public const int EstadoEnvioN8 = 8;

        //---
        public static string[] EstadoBandejaRecibidos = new string[] { EstadoEnvioT7, EstadoEnvioT1, EstadoEnvioT2, EstadoEnvioT3, EstadoEnvioT4 };
        public static string[] EstadoBandejaEnviados = new string[] { EstadoEnvioT7, EstadoEnvioT5, EstadoEnvioT6 };
        //--
        public static string[] EstadoBandejaRecibidosCompleto = new string[] { EstadoEnvioT7, EstadoEnvioT8, EstadoEnvioT1, EstadoEnvioT2, EstadoEnvioT3, EstadoEnvioT4 };
        public static string[] EstadoBandejaEnviadosCompleto = new string[] { EstadoEnvioT7, EstadoEnvioT8, EstadoEnvioT5, EstadoEnvioT6 };
    }
    public class ConstTipoRespuesta
    {
        public const string TipoRespuestaT1 = "Definitiva";
        public const string TipoRespuestaT2 = "Parcial";

        public const int TipoRespuestaN1 = 1;
        public const int TipoRespuestaN2 = 2;
    }

    public class ConstTipoEnvio
    {
        public const string TipoEnvioT1 = "Envío";
        public const string TipoEnvioT2 = "Turnar";
        public const string TipoEnvioT3 = "Respuesta parcial";
        public const string TipoEnvioT4 = "Respuesta definitiva";
        public const string TipoEnvioT5 = "Reenvio";

        public const int TipoEnvioN1 = 1;
        public const int TipoEnvioN2 = 2;
        public const int TipoEnvioN3 = 3;
        public const int TipoEnvioN4 = 4;
        public const int TipoEnvioN5 = 5;

        public static int[] TiposEnvios = new int[] { TipoEnvioN1, TipoEnvioN2, TipoEnvioN5 };
        public static int[] TiposRespuestas = new int[] { TipoEnvioN3, TipoEnvioN4 };
    }
    public class ConstTipoDestinatario
    {
        public const string TipoDestinatarioT1 = "Para";
        public const string TipoDestinatarioT2 = "Con Copia Para";

        public const int TipoDestinatarioN1 = 1;
        public const int TipoDestinatarioN2 = 2;
    }
    public class ConstTipoEmisor
    {
        public const string TipoEmisorT3 = "De";
        public const int TipoEmisorN3 = 3;
    }
    public class ConstTipoSubmit
    {
        public const string TipoSubmitT1 = "Guardar";
        public const string TipoSubmitT2 = "Enviar";

        public const int TipoSubmitN1 = 1;
        public const int TipoSubmitN2 = 2;
    }
    public class ConstTipoCategoria
    {
        public const string TipoCategoriaT1 = "Creada Por Defecto";
        public const int TipoCategoriaN1 = 1;

        public const string TipoCategoriaT2 = "Creada Por El Usuario";
        public const int TipoCategoriaN2 = 2;
    }
    public class ConstCategoria
    {
        public const string CategoriaT1 = "General";
        public const int CategoriaN1 = 1;
    }
    public class ConstRespuestaCategoria
    {
        public const string RespuestaCategoriaT1 = "Se registro la nueva categoría";
        public const int RespuestaCategoriaN1 = 1;

        public const string RespuestaCategoriaT2 = "El usuario no se encuentra";
        public const int RespuestaCategoriaN2 = 2;

        public const string RespuestaCategoriaT3 = "La categoría ya está registrada";
        public const int RespuestaCategoriaN3 = 3;

        public const string RespuestaCategoriaT4 = "La información no es correcta";
        public const int RespuestaCategoriaN4 = 4;
    }
    public class ConstTipoAnexo
    {
        public const string TipoAnexoT1 = "Anexo de documento/envio";
        public const int TipoAnexoN1 = 1;

        public const string TipoAnexoT2 = "Anexo de turnar";
        public const int TipoAnexoN2 = 2;

        public const string TipoAnexoT3 = "Anexo de la respuesta parcial";
        public const int TipoAnexoN3 = 3;

        public const string TipoAnexoT4 = "Anexo de la respuesta definitiva";
        public const int TipoAnexoN4 = 4;
    }
    public class ConstOrdenEnvio
    {
        public const int OrdenEnvioN1 = 1;
    }
    public class ConstTipoDeBaja
    {
        public const string TipoDeBajaT1 = "Baja por reasignación";
        public const int TipoDeBajaN1 = 1;

        public const string TipoDeBajaT2 = "Baja definitiva";
        public const int TipoDeBajaN2 = 2;
    }
    public class ConstTipoDeBajaArea
    {
        public const string TipoDeBajaT1 = "Baja por cambio";
        public const int TipoDeBajaN1 = 1;

        public const string TipoDeBajaT2 = "Baja definitiva";
        public const int TipoDeBajaN2 = 2;
    }
    public class ConstTipoHttp
    {
        public const string ConstTipoVistaHttpT1 = "GET";
        public const int ConstTipoVistaHttpN1 = 1;

        public const string ConstTipoVistaHttpT2 = "POST";
        public const int ConstTipoVistaHttpN2 = 2;
    }
    public class ConstHistorico
    {
        public const string ConstTipoT1 = "Histórico por persona";
        public const int ConstTipoN1 = 1;

        public const string ConstTipoT2 = "Histórico por área";
        public const int ConstTipoN2 = 2;
    }
    public class ConstBandejas
    {
        public const string ConstTipoT1 = "Recibidos";
        public const int ConstTipoN1 = 1;

        public const string ConstTipoT2 = "Enviados";
        public const int ConstTipoN2 = 2;

        public const string ConstTipoT3 = "Borradores";
        public const int ConstTipoN3 = 3;

        public const string ConstTipoT4 = "Revisión";
        public const int ConstTipoN4 = 4;

        public const string ConstTipoT5 = "Carpetas";
        public const int ConstTipoN5 = 5;
    }
    public class ConstNivelCarpeta
    {
        public const string NivelT1 = "Carpeta";
        public const int NivelN1 = 1;

        public const string NivelT2 = "Subcarpeta Nivel 2";
        public const int NivelN2 = 2;

        public const string NivelT3 = "Subcarpeta Nivel 3";
        public const int NivelN3 = 3;

        public const string NivelT4 = "Subcarpeta Nivel 4";
        public const int NivelN4 = 4;

        public const string NivelT5 = "Subcarpeta Nivel 5";
        public const int NivelN5 = 5;

        public const int MaxNivel = 5;
    }
    public class ConstColor
    {
        public const string ColorT1 = "Verde";
        public const string ColorC1 = "#607d8b";
    }
    public class ConstExtension
    {
        public const string ExtensionT1 = ".doc";
        public const string ExtensionT2 = ".xls";
        public const string ExtensionT3 = ".ppt";
        public const string ExtensionT4 = ".docx";
        public const string ExtensionT5 = ".xlsx";
        public const string ExtensionT6 = ".pptx";
        public const string ExtensionT7 = ".pdf";
        public const string ExtensionT8 = ".jpg";
        public const string ExtensionT9 = ".png";

    }
    public class ConstPlantillaCorreo
    {
        public const string NivelT1 = "Envio de documentos";
        public const string AsuntoT1 = "";
        public const int NivelN1 = 1;

        public const string NivelT2 = "Registro de usuarios";
        public const string AsuntoT2 = "Registro de usuarios"; //[HERMES] 
        public const int NivelN2 = 2;

        public const string NivelT3 = "Solicitud de ingreso al sistema (Titular)";
        public const string AsuntoT3 = "Solicitud de ingreso al sistema"; //[HERMES] 
        public const int NivelN3 = 3;

        public const string NivelT4 = "Solicitud de ingreso al sistema (Solicitante)";
        public const string AsuntoT4 = "Solicitud de ingreso al sistema"; //[HERMES] 
        public const int NivelN4 = 4;

        public const string NivelT5 = "Aceptar solicitud de ingreso al sistema";
        public const string AsuntoT5 = "Aceptación de la solicitud de ingreso al sistema"; //[HERMES] 
        public const int NivelN5 = 5;

        public const string NivelT6 = "Rechazar solicitud de ingreso al sistema";
        public const string AsuntoT6 = "Rechazo de la solicitud de ingreso al sistema"; //[HERMES] 
        public const int NivelN6 = 6;
    }
    public class ConstConfiguracionGeneral
    {
        public const string Propiedad1 = "HER_INSTITUCION";
        //public const string Propiedad2 = "HER_LOGO";
        //public const string Propiedad3 = "HER_PORTADA";
        public const string Propiedad4 = "HER_AVISO_PRIVACIDAD";
        //--
        public const string Propiedad5 = "HER_IP_DB";
        public const string Propiedad6 = "HER_DB_USUARIO";
        public const string Propiedad7 = "HER_DB_CONTRASEÑA";
        //--
        public const string Propiedad8 = "HER_IP_LDAP";
        //--
        public const string Propiedad9 = "HER_SERVIDOR_SMTP";
        public const string Propiedad10 = "HER_PUERTO_SMTP";
        public const string Propiedad11 = "HER_CORREO_NOTIFICADOR";
        public const string Propiedad12 = "HER_CONTRASEÑA_NOTIFICADOR";
        //--
        public const string Propiedad13 = "HER_AVISO_PERIODO_INHABIL";
        public const string Propiedad14 = "HER_AVISO_INICIO";
        public const string Propiedad15 = "HER_AVISO_FIN";
        //--
        //public const string Propiedad16 = "HER_COLORES";
        //public const string Propiedad17 = "HER_EXTENSIONES";

        public static string[] PropiedadesBD = new string[] { Propiedad5, Propiedad6, Propiedad7 };
        public static string[] PropiedadesCorreo = new string[] { Propiedad9, Propiedad10, Propiedad11, Propiedad12 };
        public static string[] PropiedadesAviso = new string[] { Propiedad13, Propiedad14, Propiedad15 };

        public static string[] PropiedadesIdentidad = new string[] { Propiedad1, Propiedad4 };
        public static string[] PropiedadesAcceso = new string[] { Propiedad5, Propiedad6, Propiedad7, Propiedad8, Propiedad9, Propiedad10, Propiedad11, Propiedad12 };
        public static string[] PropiedadesGeneral = new string[] { Propiedad13, Propiedad14, Propiedad15 };

        public const int Identidad = 1;
        public const int Acceso = 2;
        public const int General = 3;


        //**************************************************
        public const string Valor1 = "Universidad Veracruzana";
        //public const string Valor2 = "1";
        //public const string Valor3 = "2";
        public const string Valor4 = "<div><div><h5 class=\"text-center\">AVISO DE PRIVACIDAD</h5></div><div><div><h5 class=\"text-center\">AVISO DE PRIVACIDAD SIMPLIFICADO</h5><p><strong>Aviso de Privacidad simplificado del Formato de Registro en línea de Eventos.</strong></p><p>La <strong>Universidad Veracruzana</strong>, es el responsable del tratamiento de los datos personales que nos proporcione.</p><p>Los Datos Personales que recabamos de usted, los utilizaremos para las siguientes finalidades:</p><p>a) Asegurar su registro y posible contacto a través de los datos que nos proporcione;<br>b) Contar con información estadística de los registros.</p><p><strong>Datos Personales recabados</strong></p><p>Para la finalidad antes señalada, se solicitarán los siguientes datos personales: Nombre completo, correo electrónico, teléfono, grado de estudios, dirección particular (opcional), y/o RFC.</p><p>\"Se informa que no se recaban datos personales sensibles\".</p><p>Le informamos que sus datos personales no son compartidos con personas, empresas, organizaciones y autoridades distintas a la Universidad Veracruzana.</p><table class=\"table table-bordered\"><thead><tr><th>Destinatario de los Datos Personales</th><th>País(Opcional)</th><th>Finalidad</th></tr></thead><tbody><tr><td>Dependencia o entidad universitaria organizadora del evento.</td><td>México</td><td>Llevar el control de registros de participación al evento.</td></tr></tbody></table><p>Para mayor información acerca del tratamiento y de los derechos que puede hacer valer, usted puede acceder al aviso de privacidad del Formato de registro en línea de eventos, localizado en el portal de cada evento organizado por una entidad académica o dependencia universitaria.</p></div><hr><div><h5 class=\"text-center\">AVISO DE PRIVACIDAD INTEGRAL</h5><p><strong>Aviso de Privacidad integral del Formato de Registro en línea de Eventos.</strong></p><p>La <strong>Universidad Veracruzana</strong>, con domicilio en el Edificio “A” 5to. Piso de Rectoría, calle circuito Gonzalo Aguirre Beltrán S/N, Col. Zona Universitaria de la Ciudad de Xalapa, Veracruz, Código Postal 91000, es el responsable del tratamiento de los datos personales que nos proporcione, los cuales serán protegidos conforme a lo dispuesto por la Ley 316 de Protección de Datos Personales en Posesión de Sujetos Obligados para el Estado de Veracruz, y demás normatividad que resulte aplicable.</p><p><strong>Finalidades del tratamiento</strong></p><p>Los datos personales que recabamos de usted, los utilizaremos para las siguientes finalidades:</p><p>a) Asegurar su registro y posible contacto a través de los datos que nos proporcione;<br>b) Contar con información estadística de los registros.</p><p>De manera adicional, utilizaremos su información personal para las siguientes finalidades que no son necesarias, pero que nos permiten y facilitan brindarle una mejor atención: El organizador del evento tendrá la posibilidad de contactarlo para proporcionarle información relevante del evento y/o notificarle las fechas importantes previas al mismo.</p><p>En caso de que no desee que sus que sus datos personales sean tratados para las finalidades adicionales, esta plataforma le permitirá indicarlo o usted puede manifestarlo así al correo electrónico al organizador del evento, publicado en el registro en línea del evento.</p><p><strong>Datos Personales recabados</strong></p><p>Para las finalidades antes señaladas se solicitarán los siguientes datos personales: Nombre completo, correo electrónico, teléfono, grado de estudios, dirección particular (opcional), y/o RFC.</p><p>\"Se informa que no se recaban datos personales sensibles\".</p><p><strong>Fundamento Legal</strong></p><p>El fundamento para el tratamiento de datos personales y transferencia es (o son): la Ley 316 de Protección de Datos Personales en Posesión de Sujetos Obligados para el Estado de Veracruz, el Estatuto General de la Universidad Veracruzana Título VII, Capítulo III, Sección quinta, Artículo 261, fracciones IX y X.</p><p><strong>Transferencia de datos personales</strong></p><p>Le informamos que sus datos personales no son compartidos con personas, empresas, organizaciones y autoridades distintas a la Universidad Veracruzana.</p><table class=\"table table-bordered\"><thead><tr><th>Destinatario de los Datos Personales</th><th>País(Opcional)</th><th>Finalidad</th></tr></thead><tbody><tr><td>Dependencia o entidad universitaria organizadora del evento.</td><td>México</td><td>Llevar el control de registros de participación al evento.</td></tr></tbody></table><p><strong>Derechos ARCO</strong></p><p>Usted tiene derecho a conocer qué datos personales se tienen de usted, para qué se utilizan y las condiciones del uso que les damos (Acceso). Asimismo, es su derecho solicitar la corrección de su información personal en caso de que esté desactualizada, sea inexacta o incompleta (Rectificación); que la eliminemos de nuestros registros o bases de datos cuando considere que la misma no está siendo utilizada conforme a los principios, deberes y obligaciones previstas en la ley (Cancelación); así como oponerse al uso de sus datos personales para fines específicos (Oposición). Estos derechos se conocen como derechos ARCO.</p><p>Para el ejercicio de cualquiera de los derechos ARCO, usted podrá presentar solicitud por escrito ante la <strong>Coordinación de Transparencia</strong>, formato o medio electrónico <strong>datospersonales@uv.mx</strong> la que deberá contener:</p><ul><li><p>El nombre del titular y su domicilio o cualquier otro medio para recibir notificaciones;</p></li><li><p>Los documentos que acrediten la identidad del titular, y en su caso, la personalidad e identidad de su representante;</p></li><li><p>De ser posible, el área responsable que trata los datos personales;</p></li><li><p>La descripción clara y precisa de los datos personales respecto de los que se busca ejercer alguno de los derechos ARCO, salvo que se trate del derecho de acceso;</p></li><li><p>La descripción del derecho ARCO que se pretende ejercer, o bien, lo que solicita el titular, y</p></li><li><p>Cualquier otro elemento o documento que facilite la localización de los datos personales, en su caso.</p></li></ul><p>En caso de solicitar la rectificación, adicionalmente deberá indicar las modificaciones a realizarse y aportar la documentación oficial necesaria que sustente su petición. En el derecho de cancelación debe expresar las causas que motivan la eliminación. Y en el derecho de oposición debe señalar los motivos que justifican se finalice el tratamiento de los datos personales y el daño o perjuicio que le causaría, o bien, si la oposición es parcial, debe indicar las finalidades específicas con las que se no está de acuerdo, siempre que no sea un requisito obligatorio.</p><p>La <strong>Coordinación de Transparencia</strong> responderá en el domicilio o medio que el titular de los datos personales designe en su solicitud, en un plazo de 15 días hábiles, que puede ser ampliado por 10 días hábiles más previa notificación. La respuesta indicará si la solicitud de acceso, rectificación, cancelación u oposición es procedente y, en su caso, hará efectivo dentro de los 15 días hábiles siguientes a la fecha en que comunique la respuesta.</p><p><strong>Datos de la Coordinación de Transparencia</strong></p><p><strong>Domicilio:</strong> Calle Veracruz # 46 Depto. 5, Fracc. Pomona, C.P. 91040.<br><strong>Teléfono:</strong> (228) 841-59-20, 818-78-91<br><strong>Correo electrónico institucional:</strong> transparencia@uv.mx</p><p><strong>Cambios al Aviso de Privacidad</strong></p><p>En caso de realizar alguna modificación al Aviso de Privacidad, se le hará de su conocimiento a través del portal de registro de eventos.</p></div></div></div>";

        public const string Valor5 = "148.226.26.100";
        public const string Valor6 = "usrHermes2018";
        public const string Valor7 = "XPk2cNwVYZ6P";

        public const string Valor8 = "148.226.12.10:3268";

        public const string Valor9 = "smtp.office365.com";
        public const string Valor10 = "587";
        public const string Valor11 = "ddiaav@uv.mx";
        public const string Valor12 = "123456";

        public const string Valor13 = "<p>Por el momento no esta disponible</p>";
        public const string Valor14 = "29/08/2019";
        public const string Valor15 = "29/08/2019";

        //public const string Valor16 = "1";
        //public const string Valor17 = "1,2,3,4,5,6,7,8,9";

        //Tipos
        //1: Texto
        //2: Imagen
        //3: Editor
        //4: Colección
        //5: Fecha

        public const int TipoN1 = 1;
        public const string TipoT1 = "Texto";

        public const int TipoN3 = 3;
        public const string TipoT3 = "Editor";

        public const int TipoN5 = 5;
        public const string TipoT5 = "Fecha";

        //--
        public const int TipoImagenN1 = 1;
        public const string TipoImagenT1 = "Logo";

        public const int TipoImagenN2 = 2;
        public const string TipoImagenT2 = "Portada";
        //--
        public const int TipoColeccionN1 = 1;
        public const string TipoColeccionT1 = "Colores";

        public const int TipoColeccionN2 = 2;
        public const string TipoColeccionT2 = "Extensiones";
    }

    public class ConstCompromiso
    {
        public const string EstadoT1 = "Activo/Actual";
        public const string EstadoT2 = "No activo/Pasado";

        public const int EstadoN1 = 1;
        public const int EstadoN2 = 2;

        public const string TipoT1 = "Fecha propuesta";
        public const string TipoT2 = "Fecha modificada-Compromiso";

        public const int TipoN1 = 1;
        public const int TipoN2 = 2;

    }
    public class ConstProximosVencer
    {
        public const double Porcentaje = 0.4;
    }
    public class ConstAnexoRuta
    {
        public const string EstadoT1 = "Activo/Actual";
        public const string EstadoT2 = "No activo/Pasado";

        public const int EstadoN1 = 1;
        public const int EstadoN2 = 2;
    }

    public class ConstTramiteEstado
    {
        public const string EstadoT1 = "Activo";
        public const string EstadoT2 = "No activo";

        public const int EstadoN1 = 1;
        public const int EstadoN2 = 2;
    }

    public class ConstTramite
    {
        public const string TipoT1 = "General";
        public const int TipoN1 = 1;
    }

    public class ConstLoginConstancias
    {
        public const string UsernameGeneral = "ddiaav";
        public const string PasswordGeneral = "123456";
    }
}