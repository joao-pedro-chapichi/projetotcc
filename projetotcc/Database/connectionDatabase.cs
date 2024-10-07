using System;
using Npgsql;

namespace projetotcc.Database
{
    class ConnectionDatabase
    {
        // Conexão com o banco de dados
        // Declaração dos parametros para conexao

#if DEBUG
        string connectionString = "User Id = postgres.cwuebsaemzssxrzjlamw; Password=!Senha1234???;Server=aws-0-sa-east-1.pooler.supabase.com;Port=5432;Database=postgres;";
        
#else
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=pontofacil_db";
#endif
          
        private NpgsqlConnection connection = null;

        // Metodo de conexao
        // Conexão bem feita
        public NpgsqlConnection connectionDB()
        {
            try
            {
                connection = new NpgsqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Conexão aberta com sucesso!");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar: {ex.Message}");
                return null;
            }
        }
    }
}
