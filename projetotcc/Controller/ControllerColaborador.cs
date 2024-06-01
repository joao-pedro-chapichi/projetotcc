using Npgsql;
using projetotcc.Database;
using projetotcc.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetotcc.Controller
{
    public static class ControllerColaborador
    {
        public static void cadastrarFuncionario(ModelFuncionario modelFunc)
        {
            /* Foi declarada a classe ModelFuncionario como parametro do metodo cadastrarFuncionario
               pois será necessária para passar os valores da textBox ao metodo*/


            // Instanciando a classe 'ConnectionDatabase' e puxando o metodo de conexão
            ConnectionDatabase con = new ConnectionDatabase();
            NpgsqlConnection connection = con.connectionDB();

            try
            {
                /* Declarando a string do insert e criando um novo comando o comando para 
                   para ser executado no banco */
                string sql = "INSERT INTO funcionario(id_funcionario, nome) values(@id_funcionario, @nome)";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);

                // Passsando os parametros que deverao ser cadastrados no banco de dados
                cmd.Parameters.AddWithValue("nome", modelFunc.Nome);
                cmd.Parameters.AddWithValue("id_funcionario", modelFunc.Id_funcionario);
                cmd.ExecuteNonQuery();

                // Mensagem de sucesso após cadastrar o funcionário
                MessageBox.Show("Funcionário cadastrado com sucesso!", "SUCESSO!");
            } 
            catch (Exception ex)
            {
                // Retornando uma mensagem em caso de erros
                MessageBox.Show("Erro ao cadastrar funcionário! Erro: " + ex.Message, "ERRO!");
            }
            finally
            {
                // Verificando se a conexão está aberta e encerrando
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }


        }

        public async static ValueTask<DataTable> buscasFuncionarios(string nome, string idFuncionario)
        {
            DataTable dataTable = new DataTable();
            ConnectionDatabase con = new ConnectionDatabase();

            // Base da consulta SQL
            string sqlDeBuscar = "SELECT * FROM funcionario WHERE 1=1";

            // Verificar se parâmetros são fornecidos e adicionar filtros dinamicamente
            if (!string.IsNullOrEmpty(nome))
            {
                sqlDeBuscar += " AND nome LIKE @nome";
            }
            if (!string.IsNullOrEmpty(idFuncionario))
            {
                sqlDeBuscar += " AND CAST(id_funcionario AS TEXT) LIKE @id_funcionario";
            }

            using (NpgsqlConnection conn = con.connectionDB())
            {
                try
                {

                    using (NpgsqlCommand comm = new NpgsqlCommand(sqlDeBuscar, conn))
                    {
                        // Adicionar parâmetros dinamicamente
                        if (!string.IsNullOrEmpty(nome))
                        {
                            comm.Parameters.AddWithValue("@nome", "%" + nome + "%");
                        }
                        if (!string.IsNullOrEmpty(idFuncionario))
                        {
                            comm.Parameters.AddWithValue("@id_funcionario", "%" + idFuncionario + "%");
                        }

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(comm))
                        {
                            await Task.Run(() => adapter.Fill(dataTable)); // Preencher DataTable de forma assíncrona
                        }
                    }
                }
                catch (NpgsqlException ex)
                {
                    // Registra como mensagem
                    MessageBox.Show("Erro ao pesquisar funcionários: " + ex.Message);
                }
            }

            return dataTable;
        }

        public async static ValueTask<string> ExcluirFuncionario(int codigo)
        {
            string sqlDelete = "DELETE FROM funcionario WHERE id_funcionario = @id_funcionario";

            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                try
                {
                    using (NpgsqlCommand commDelete = new NpgsqlCommand(sqlDelete, conn))
                    {
                        commDelete.Parameters.AddWithValue("@id_funcionario", codigo);
                        int rowsAffectedDelete = await commDelete.ExecuteNonQueryAsync();

                        if (rowsAffectedDelete > 0)
                        {
                            return "Colaborador excluído com sucesso!";
                        }
                        else
                        {
                            return "Colaborador não encontrado para exclusão.";
                        }
                    }
                     

                }catch(Exception ex)
                {
                    return $"Erro: {ex.Message}";
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
        }
    }
}
