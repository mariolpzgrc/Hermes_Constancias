using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;
using System.Globalization;
using Hermes2018.ViewModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Hermes2018.Helpers;
using QRCoder;

namespace Hermes2018.Services
{
    public class HerramientaService: IHerramientaService
    {
        private readonly CultureInfo _cultureEs;

        public HerramientaService()
        {
            _cultureEs = new CultureInfo("es-MX");
        }

        public string Encriptar(string texto)
        {
            var fechaActual = DateTime.Now;
            texto = string.Format("{0}#{1}", texto, fechaActual.ToBinary().ToString());
            //--https://www.qualityinfosolutions.com/metodos-para-encriptar-y-desencriptar/
            try
            {
                string key = string.Format("{0}{1}#{2}{3}", "%H$Aq5gD#EnO&FmpeR3Sr2VPoMvG@Ty@fE*9dMh&LS4krWfem", fechaActual.Day, fechaActual.Month, fechaActual.Year); //llave para encriptar datos
                byte[] keyArray;
                byte[] arregloCifrar = UTF8Encoding.UTF8.GetBytes(texto);

                //Se utilizan las clases de encriptación MD5
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();

                //Algoritmo TripleDES
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
                {
                    Key = keyArray,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] ArrayResultado = cTransform.TransformFinalBlock(arregloCifrar, 0, arregloCifrar.Length);
                tdes.Clear();

                //se regresa el resultado en forma de una cadena
                texto = Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
            }
            catch (Exception)
            {
                texto = string.Empty;
            }

            return texto;
        }
        public string Desencriptar(string textoEncriptado)
        {
            var fechaActual = DateTime.Now;
            try
            {
                string key = string.Format("{0}{1}#{2}{3}", "%H$Aq5gD#EnO&FmpeR3Sr2VPoMvG@Ty@fE*9dMh&LS4krWfem", fechaActual.Day, fechaActual.Month, fechaActual.Year); //llave para desencriptar datos
                byte[] keyArray;
                byte[] arrayDescifrar = Convert.FromBase64String(textoEncriptado);
                string[] separar;

                //algoritmo MD5
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
                {
                    Key = keyArray,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(arrayDescifrar, 0, arrayDescifrar.Length);
                tdes.Clear();
                textoEncriptado = UTF8Encoding.UTF8.GetString(resultArray);

                separar = textoEncriptado.Split('#');
                //textoEncriptado = string.Format("{0}_{1}", separar[0], DateTime.FromBinary(long.Parse(separar[1])).ToString("dd/MM/yyyy H:m:ss", _cultureEs));
                textoEncriptado = separar[0];
            }
            catch (Exception)
            {
                textoEncriptado = string.Empty;
            }

            return textoEncriptado;

            //var _cultureEs = new System.Globalization.CultureInfo("es-MX");
            //var fechaActual = DateTime.Now.ToBinary().ToString();
            //var fechaObtenida = DateTime.FromBinary(long.Parse(fechaActual));

            //var _cultureEs = new System.Globalization.CultureInfo("es-MX");
            //var fechaActual = DateTime.Now.ToBinary().ToString();
            //var valor0 = string.Format("{0}#{1}", "lusimon", fechaActual.ToString());
        }
        
        public TokenApiViewModel ConstruirToken(string usuario, int minutos)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, this.Encriptar(string.Format("{0}={1}", ConstKeyApp.KeyApp, usuario))),
            };
            //--
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConstApiMasterKey.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //--
            var expiracion = DateTime.UtcNow.AddMinutes(minutos);
            //--
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:44373/",
                audience: "https://localhost:44373/",
                claims: claims,
                expires: expiracion,
                signingCredentials: creds);
            //--
            var regreso = new TokenApiViewModel() {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiracion
            };
            //--
            return regreso;
        }
    }
}
