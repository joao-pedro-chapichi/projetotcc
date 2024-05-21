using projetotcc.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace projetotcc.Controller
{
    static public class ControllerAll
    {
        
        public static bool VerificarExistencia(string tabela, string campo, string dado)
        {
            string sqlCheckNome = $"SELECT COUNT(*) FROM {tabela} WHERE {campo} = @{campo}";

            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                try
                {
                    using (NpgsqlCommand commCheckNome = new NpgsqlCommand(sqlCheckNome, conn))
                    {
                        commCheckNome.Parameters.AddWithValue($"@{campo}", dado);
                        int countNome = Convert.ToInt32(commCheckNome.ExecuteScalar());
                        if (countNome > 0)
                        {
                            return true; // Nome já existe
                        }
                    }

                    return false; // Nenhum registro encontrado com o nome ou código fornecidos
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao verificar existência: " + ex.Message);
                    return false; // Retorna false em caso de erro para evitar inserção indevida
                }
            }
        }

        public static string Cadastrar(string tabela, object[] campos, object[] dados)
        {
            string sqlInsert = "INSERT INTO " + tabela + " (";

            int numElementos = campos.Length;
            for (int i = 0; i < numElementos; i++)
            {
                sqlInsert += " " + campos[i] + ",";
            }
            sqlInsert = sqlInsert.TrimEnd(',') + ") VALUES (";

            for (int i = 0; i < numElementos; i++)
            {
                sqlInsert += " @" + campos[i] + ",";
            }
            sqlInsert = sqlInsert.TrimEnd(',') + ")";

            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand commInsert = new NpgsqlCommand(sqlInsert, conn))
                {
                    try
                    {
                        for (int i = 0; i < numElementos; i++)
                        {
                            commInsert.Parameters.AddWithValue("@" + campos[i], dados[i]);
                        }

                        commInsert.ExecuteNonQuery();

                        return "Cadastrado com sucesso!";
                    }
                    catch (Exception ex)
                    {
                        return "Erro ao cadastrar: " + ex.Message;
                    }
                }
            }
        }

        public static string Excluir(string tabela, string campo, string dado)
        {
            // Verificar se já existe alguém cadastrado com o mesmo nome ou código

            string sqlDelete = $"DELETE FROM {tabela} WHERE {campo} = @{campo}";

            ConnectionDatabase con = new ConnectionDatabase();
            NpgsqlConnection conn = con.connectionDB();

            try
            {
                // Exclua o funcionário da tabela 'funcionario'
                NpgsqlCommand commDelete = new NpgsqlCommand(sqlDelete, conn);
                commDelete.Parameters.AddWithValue($"@{campo}", dado);
                int rowsAffectedDelete = commDelete.ExecuteNonQuery();

                if (rowsAffectedDelete > 0)
                {
                    return "Excluído com sucesso!";
                }
                else
                {
                    return "Não encontrado para exclusão.";
                }
            }
            catch (Exception ex)
            {
                return "Erro ao excluir" + ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataTable Listar(string tabela, object campos, string campo, object valor)
        {
            DataTable dataTable = new DataTable();

            // Construção da parte SELECT da query SQL
            string camposString = "";
            if (campos is string)
            {
                camposString = (string)campos;
            }
            else if (campos is string[])
            {
                camposString = string.Join(",", (string[])campos);
            }
            else
            {
                Console.WriteLine("O parâmetro 'campos' deve ser uma string ou um array de strings.");
            }

            // Query SQL para buscar funcionários cujo nome seja semelhante ao fornecido
            string sqlSearch = $"SELECT {camposString} FROM {tabela} WHERE {campo} ILIKE @{campo}";

            // Criação da conexão com o banco de dados
            ConnectionDatabase con = new ConnectionDatabase();
            NpgsqlConnection conn = con.connectionDB();

            // Criação e configuração do comando SQL
            NpgsqlCommand comm = new NpgsqlCommand(sqlSearch, conn);
            comm.Parameters.AddWithValue($"@{campo}", "%" + valor + "%");

            try
            {
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(comm);
                adapter.Fill(dataTable);
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Erro ao pesquisar funcionários: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dataTable;
        }

        public static string Alterar(string tabela, object[] campos, object[] dados, string campo, object valor)
        {

            string sqlInsert = $"UPDATE {tabela} SET ";

            int numElementos = campos.Length;
            for (int i = 0; i < numElementos; i++)
            {
                sqlInsert += $"{campos[i]} = @{campos[i]},";
            }
            sqlInsert = sqlInsert.TrimEnd(',') + $" WHERE {campo} = @{campo}";

            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand commInsert = new NpgsqlCommand(sqlInsert, conn))
                {
                    try
                    {
                        for (int i = 0; i < numElementos; i++)
                        {
                            commInsert.Parameters.AddWithValue($"@{campos[i]}", dados[i]);
                        }

                        commInsert.ExecuteNonQuery();

                        return "Cadastrado com sucesso!";
                    }
                    catch (Exception ex)
                    {
                        return "Erro ao cadastrar: " + ex.Message;
                    }
                }
            }
        }

        public static object[] CriarArray(object objc)
        {
            PropertyInfo[] propriedades = objc.GetType().GetProperties();
            object[] valores = new object[propriedades.Length];

            for (int i = 0; i < propriedades.Length; i++)
            {
                valores[i] = propriedades[i].GetValue(objc);
            }

            return valores;
        }
    }
}
