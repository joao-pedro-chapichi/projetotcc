using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace projetotcc.Database
{
    internal class connectionDatabase
    {
        // Conexão com o banco de dados
        // Declaração dos parametros para conexao
        string connectionString = "Host=postgres;Port=5432;Username=postgres;Password=123456;Database=pontofacil_db";
        NpgsqlConnection connection = null;

        // Metodo de conexao
        //Conexão bem feita
        public NpgsqlConnection connectionDB()
        {
            try
            {
                connection = new NpgsqlConnection(connectionString);
                connection.Open();
                return connection;
            } 
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
