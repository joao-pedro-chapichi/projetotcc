using projetotcc.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace projetotcc.Controller
{
    // Classe controladora que agrupa métodos para operações CRUD no banco de dados
    static public class ControllerAll
    {
        // Método para verificar a existência de um registro em uma tabela com base em um campo e valor fornecidos
        public static bool VerificarExistencia(string tabela, string campo, string dado)
        {
            // Query SQL para contar registros que correspondem ao valor fornecido
            string sqlCheckNome = $"SELECT COUNT(*) FROM {tabela} WHERE {campo} = @{campo}";

            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                try
                {
                    using (NpgsqlCommand commCheckNome = new NpgsqlCommand(sqlCheckNome, conn))
                    {
                        // Adiciona o parâmetro à query
                        commCheckNome.Parameters.AddWithValue($"@{campo}", dado);
                        // Executa a query e converte o resultado para inteiro
                        int countNome = Convert.ToInt32(commCheckNome.ExecuteScalar());
                        if (countNome > 0)
                        {
                            return true; // Registro encontrado
                        }
                    }

                    return false; // Nenhum registro encontrado
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao verificar existência: " + ex.Message);
                    return false; // Retorna false em caso de erro para evitar inserção indevida
                }
            }
        }

        // Método para inserir um novo registro na tabela especificada
        public static string Cadastrar(string tabela, object classe)
        {
            // Obtém as propriedades do objeto fornecido
            PropertyInfo[] propriedades = classe.GetType().GetProperties();

            // Arrays para armazenar os valores e nomes dos campos
            object[] valores = new object[propriedades.Length];
            string[] campos = propriedades.Select(p => p.Name).ToArray();

            // Preenche os arrays com os valores das propriedades
            for (int i = 0; i < propriedades.Length; i++)
            {
                valores[i] = propriedades[i].GetValue(classe);
            }

            int numElementos = campos.Length;
            // Monta a query SQL para inserção
            string sqlInsert = $"INSERT INTO {tabela} ({string.Join(", ", campos)}) VALUES ({string.Join(", ", campos.Select(c => "@" + c))})";

            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand commInsert = new NpgsqlCommand(sqlInsert, conn))
                {
                    try
                    {
                        // Adiciona os parâmetros à query
                        for (int i = 0; i < numElementos; i++)
                        {
                            commInsert.Parameters.AddWithValue("@" + campos[i], valores[i]);
                        }

                        // Executa a query
                        commInsert.ExecuteNonQuery();

                        return "Cadastrado com Sucesso!";
                    }
                    catch (Exception ex)
                    {
                        return "Erro ao cadastrar: " + ex.Message;
                    }
                }
            }
        }

        // Método para excluir um registro de uma tabela com base em um campo e valor fornecidos
        public static string Excluir(string tabela, string campo, string dado)
        {
            // Query SQL para deletar o registro correspondente
            string sqlDelete = $"DELETE FROM {tabela} WHERE {campo} = @{campo}";

            ConnectionDatabase con = new ConnectionDatabase();
            NpgsqlConnection conn = con.connectionDB();

            try
            {
                // Criação do comando SQL para exclusão
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
                return "Erro ao excluir: " + ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        // Método para listar registros de uma tabela com base em um campo e valor fornecidos
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

            // Query SQL para buscar registros com base no valor fornecido
            string sqlSearch = $"SELECT {camposString} FROM {tabela} WHERE {campo} ILIKE @{campo}";

            // Criação da conexão com o banco de dados
            ConnectionDatabase con = new ConnectionDatabase();
            NpgsqlConnection conn = con.connectionDB();

            // Criação e configuração do comando SQL
            NpgsqlCommand comm = new NpgsqlCommand(sqlSearch, conn);
            comm.Parameters.AddWithValue($"@{campo}", "%" + valor + "%");

            try
            {
                // Preenche o DataTable com os dados retornados pela query
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(comm);
                adapter.Fill(dataTable);
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Erro ao listar registros: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dataTable;
        }

        // Método para atualizar um registro de uma tabela com base em um campo e valor fornecidos
        public static string Alterar(string tabela, object classe, string campo, object valor)
        {
            // Obtém as propriedades do objeto fornecido
            PropertyInfo[] propriedades = classe.GetType().GetProperties();

            // Arrays para armazenar os valores e nomes dos campos
            object[] valores = new object[propriedades.Length];
            string[] campos = propriedades.Select(p => p.Name).ToArray();

            // Preenche os arrays com os valores das propriedades
            for (int i = 0; i < propriedades.Length; i++)
            {
                valores[i] = propriedades[i].GetValue(classe);
            }

            // Monta a query SQL para atualização
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
                        // Adiciona os parâmetros à query
                        for (int i = 0; i < numElementos; i++)
                        {
                            commInsert.Parameters.AddWithValue($"@{campos[i]}", valores[i]);
                        }

                        // Executa a query
                        commInsert.ExecuteNonQuery();

                        return "Alterado com sucesso!";
                    }
                    catch (Exception ex)
                    {
                        return "Erro ao alterar: " + ex.Message;
                    }
                }
            }
        }
    }
}
