using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Cliente
{
    class Conexion
    {
        MySqlConnection conexionBD;
        string cadenaConexion;

        public Conexion()
        {
            //cadenaConexion = ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString;
            cadenaConexion = "server=localhost;port=3306;user id=root;password=password;database=testeocolas"; 
            conexionBD = new MySqlConnection(cadenaConexion);
        }


        public Conexion(string server, string port, string user, string pass, string db)
        {
            string cadenaConexion = "server=" + server + ";port=" + port + ";user id=" + user + ";password=" + pass + ";database=" + db + ";";
            conexionBD = new MySqlConnection(cadenaConexion);
        }

        public DataTable QuerySelect()
        {
            string Query = "SELECT Id, Nombre from colacomun WHERE Bloqueado = 0";

            DataTable dt = new DataTable();
            try
            {
                conexionBD.Open();
                MySqlCommand cmd = new MySqlCommand(Query, conexionBD);
                MySqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("ERROR " + ex.ToString());
            }
            conexionBD.Close();
            return dt;
        }

        public void QueryInsert(string nombre) 
        {
            MySqlCommand cmd = new MySqlCommand(
               "INSERT INTO colacomun " +
               "(Nombre) " +
               "VALUES (@Nombre); "
               );
            cmd.Parameters.AddWithValue("@Nombre", nombre);

            string datos = "";
            try
            {
                conexionBD.Open();
                cmd.Connection = conexionBD;
                datos = cmd.ExecuteNonQuery().ToString();
            }
            catch (MySqlException ex)
            {
                datos = "ERROR " + ex.ToString();
                MessageBox.Show(datos);
            }
            conexionBD.Close();
        }

        public void QueryUpdate(string id)
        {
            MySqlCommand cmd = new MySqlCommand(
               "UPDATE colacomun " +
               "SET Bloqueado = 1 " +
               "WHERE Id = @id"
               );

            cmd.Parameters.AddWithValue("@id", id);

            string datos = "";
            try
            {
                conexionBD.Open();
                cmd.Connection = conexionBD;
                datos = cmd.ExecuteNonQuery().ToString();
            }
            catch (MySqlException ex)
            {
                datos = "ERROR " + ex.ToString();
                MessageBox.Show(datos);
            }

            conexionBD.Close();
        }

    }
}
