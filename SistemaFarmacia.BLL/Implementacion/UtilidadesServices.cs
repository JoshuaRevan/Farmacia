using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaFarmacia.BLL.Interfaces;
using System.Security.Cryptography;


namespace SistemaFarmacia.BLL.Implementacion
{
    public class UtilidadesServices : IUtilidadesService
    {
        private const int Iteraciones = 10000;
        private const int LongitudSalt = 16;
        private const int LongitudHash = 32;
        public string GenerarClave()
        {
            const string caratereres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; //cadena de caracteres Permitidos 
            StringBuilder resultado = new StringBuilder(); //Contruir la clave 
            Random random = new Random(); //Generación de Números Aleatorios

            for (int i = 0; i < 8; i++)
            {
                resultado.Append(caratereres[random.Next(caratereres.Length)]); //genera un índice aleatorio entre 0 y caracteres.Length - 1.
            }

            return resultado.ToString();
        }

        public string EncriptarClave(string contrasena)
        {
            // 1. Generar salt aleatorio
            byte[] salt = new byte[LongitudSalt];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // 2. Derivar la clave
            var pbkdf2 = new Rfc2898DeriveBytes(contrasena, salt, Iteraciones, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(LongitudHash);

            // 3. Retornar formato combinado: iteraciones.salt.hash (en Base64)
            return $"{Iteraciones}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool VerificarClave(string ContrasenaIngresada, string hashAlmacenado)
        {
            try
            {
                var partes = hashAlmacenado.Split('.');
                if (partes.Length != 3) return false;

                int iteraciones = int.Parse(partes[0]);
                byte[] salt = Convert.FromBase64String(partes[1]);
                byte[] hashOriginal = Convert.FromBase64String(partes[2]);

                var pbkdf2 = new Rfc2898DeriveBytes(ContrasenaIngresada, salt, iteraciones, HashAlgorithmName.SHA256);
                byte[] hashNuevo = pbkdf2.GetBytes(LongitudHash);

                return hashNuevo.SequenceEqual(hashOriginal);
            }
            catch 
            { 
                return false; 
            }

        }
    }
}
