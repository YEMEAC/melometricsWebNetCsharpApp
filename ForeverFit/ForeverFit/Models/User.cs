using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ForeverFit.Models
{
    public class User
    {

        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }


        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }


        [Required]
        [Display(Name = "Genero")]
        public int Genero { get; set; }

        [Required]
        [Display(Name = "Peso")]
        public float Weight { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Frecuencia cardiaca máxima")]
        public float MaxHeartRate { get; set; }

        [Required]
        [Display(Name = "Frecuencia cardiaca en reposo")]
        public float RestingHeartRate { get; set; }



        public User IsValid(string _username, string _password)
        {
            using (var cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ForeverFitDB"].ConnectionString))


            {
                string _sql = @"SELECT * FROM [dbo].[User] " +
                       @"WHERE [Username] = @u AND [Password] = @p";
                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = _username;
                cmd.Parameters
                    .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                    .Value = Helpers.SHA1.Encode(_password);
                cn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    var u = mappingUser(reader); 
                    reader.Dispose();
                    cmd.Dispose();
                    return u;
                }
                else
                {
                    reader.Dispose();
                    cmd.Dispose();
                    return null;
                }
            }
        }
        
        internal int Persist()
        {

            using (var cm = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ForeverFitDB"].ConnectionString))
            {
                string _sqll = @"SELECT * FROM [dbo].[User] " +
                     @"WHERE [Username] = @u";

                var cmdd = new SqlCommand(_sqll, cm);
                cmdd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = this.UserName;

                cm.Open();
                var readerr = cmdd.ExecuteReader();
               

                if (readerr.HasRows)
                {
                    readerr.Dispose();
                    cmdd.Dispose();
                    return 2;
                }
                readerr.Dispose();
                cmdd.Dispose();
            }

            using (var cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ForeverFitDB"].ConnectionString))
            {

                string _sql = "INSERT INTO [dbo].[User] ([Username], [Password]," +
                    "[RegDate], [Genero], [BirthDate], [MaxHeartRate], [RestingHeartRate]," +
                 "[Weight]) VALUES (@un, @p,@rd, @g, @bd, @mhr, @rhr, @w)";

                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                    .Add(new SqlParameter("@un", SqlDbType.NVarChar))
                    .Value = this.UserName;
                cmd.Parameters
                    .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                    .Value = Helpers.SHA1.Encode(this.Password);
                cmd.Parameters
                   .Add(new SqlParameter("@rd", SqlDbType.Date))
                   .Value = DateTime.Now;
                cmd.Parameters
                    .Add(new SqlParameter("@g", SqlDbType.Bit))
                    .Value = this.Genero;
                cmd.Parameters
                   .Add(new SqlParameter("@bd", SqlDbType.Date))
                   .Value = this.BirthDate;
                cmd.Parameters
                    .Add(new SqlParameter("@mhr", SqlDbType.Float))
                    .Value = this.MaxHeartRate;
                cmd.Parameters
                   .Add(new SqlParameter("@rhr", SqlDbType.Float))
                   .Value = this.RestingHeartRate;
                cmd.Parameters
                   .Add(new SqlParameter("@w", SqlDbType.Float))
                   .Value = this.Weight;
                cn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.RecordsAffected == 1)
                {
                    reader.Dispose();
                    cmd.Dispose();
                    return 0;
                }
                reader.Dispose();
                cmd.Dispose();
                return 1;
            }
        }

        private User mappingUser(SqlDataReader reader)
        {
            User u = new User();
            while (reader.Read())
            {      
                u.Id = (int) reader["Id"];
                u.UserName = reader["UserName"].ToString();
                u.Password = reader["Password"].ToString();
                u.Genero = (bool)reader["Genero"] ? 1 : 0;
                u.BirthDate = (DateTime)reader["BirthDate"];
                u.MaxHeartRate = (float)(double) reader["MaxHeartRate"];
                u.RestingHeartRate = (float)(double) reader["RestingHeartRate"];
                u.Weight = (float)(double) reader["Weight"];
            }
            return u;
        }

        
    }
}