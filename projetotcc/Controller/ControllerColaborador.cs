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
        public static async void cadastrarFuncionario(ModelFuncionario modelFunc)
        {
            bool test = await VerificarExistenciaId(modelFunc.Id_funcionario);
            /* Foi declarada a classe ModelFuncionario como parametro do metodo cadastrarFuncionario
               pois será necessária para passar os valores da textBox ao metodo*/
            if (!test) 
            {
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
            else
            {
                MessageBox.Show("Código já cadastrado!");
                return;
            }

        }
        // Declaração de um método assíncrono que retorna um DataTable com os funcionários filtrados pelo nome e/ou id
        public async static ValueTask<DataTable> buscasFuncionarios(string nome, string idFuncionario)
        {
            DataTable dataTable = new DataTable(); // Cria um novo DataTable para armazenar os resultados da consulta
            ConnectionDatabase con = new ConnectionDatabase(); // Instancia a classe de conexão com o banco de dados

            // Declara a consulta SQL básica
            string sqlDeBuscar = "SELECT id_funcionario, nome FROM funcionario WHERE 1=1";

            // Adiciona filtros dinamicamente à consulta SQL
            if (!string.IsNullOrEmpty(nome))
            {
                sqlDeBuscar += " AND nome LIKE @nome";
            }
            if (!string.IsNullOrEmpty(idFuncionario))
            {
                sqlDeBuscar += " AND CAST(id_funcionario AS TEXT) LIKE @id_funcionario";
            }

            // Abre uma conexão com o banco de dados
            using (NpgsqlConnection conn = con.connectionDB())
            {
                try
                {
                    // Cria um comando SQL com a consulta e a conexão
                    using (NpgsqlCommand comm = new NpgsqlCommand(sqlDeBuscar, conn))
                    {
                        // Adiciona parâmetros ao comando dinamicamente
                        if (!string.IsNullOrEmpty(nome))
                        {
                            comm.Parameters.AddWithValue("@nome", "%" + nome + "%");
                        }
                        if (!string.IsNullOrEmpty(idFuncionario))
                        {
                            comm.Parameters.AddWithValue("@id_funcionario", "%" + idFuncionario + "%");
                        }

                        // Utiliza um adaptador para preencher o DataTable com os resultados da consulta de forma assíncrona
                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(comm))
                        {
                            await Task.Run(() => adapter.Fill(dataTable)); // Preenche o DataTable
                        }
                    }
                }
                catch (NpgsqlException ex)
                {
                    // Exibe uma mensagem de erro se ocorrer uma exceção
                    MessageBox.Show("Erro ao pesquisar funcionários: " + ex.Message);
                }
            }

            return dataTable; // Retorna o DataTable com os resultados
        }

        // Declaração de um método assíncrono que exclui um funcionário pelo seu código
        public async static ValueTask<string> ExcluirFuncionario(int codigo)
        {
            string sqlDelete = "DELETE FROM funcionario WHERE id_funcionario = @id_funcionario"; // Declara a consulta SQL de exclusão

            ConnectionDatabase con = new ConnectionDatabase(); // Instancia a classe de conexão com o banco de dados

            // Abre uma conexão com o banco de dados
            using (NpgsqlConnection conn = con.connectionDB())
            {
                try
                {
                    // Cria um comando SQL para executar a exclusão
                    using (NpgsqlCommand commDelete = new NpgsqlCommand(sqlDelete, conn))
                    {
                        // Adiciona o parâmetro necessário ao comando
                        commDelete.Parameters.AddWithValue("@id_funcionario", codigo);
                        int rowsAffectedDelete = await commDelete.ExecuteNonQueryAsync(); // Executa a exclusão de forma assíncrona

                        // Verifica se alguma linha foi afetada para determinar o sucesso da operação
                        if (rowsAffectedDelete > 0)
                        {
                            return "Colaborador excluído com sucesso!";
                        }
                        else
                        {
                            return "Colaborador não encontrado para exclusão.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    return $"Erro: {ex.Message}"; // Retorna a mensagem de erro em caso de exceção
                }
                finally
                {
                    await conn.CloseAsync(); // Fecha a conexão com o banco de dados
                }
            }
        }

        // Declaração de um método assíncrono que altera os dados de um funcionário
        public async static ValueTask<string> AlterarDados(ModelFuncionario modelFunc, int id)
        {
            string sqlUpdate = "UPDATE funcionario SET id_funcionario = @id_funcionario, nome = @nome WHERE id = @id"; // Declara a consulta SQL de atualização

            ConnectionDatabase con = new ConnectionDatabase(); // Instancia a classe de conexão com o banco de dados

            // Abre uma conexão com o banco de dados
            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand commUpdate = new NpgsqlCommand(sqlUpdate, conn))
                {
                    try
                    {
                        // Adiciona os parâmetros necessários ao comando
                        commUpdate.Parameters.AddWithValue("@id_funcionario", modelFunc.Id_funcionario);
                        commUpdate.Parameters.AddWithValue("@nome", modelFunc.Nome);
                        commUpdate.Parameters.AddWithValue("@id", id);

                        int rowsAffected = await commUpdate.ExecuteNonQueryAsync(); // Executa a atualização de forma assíncrona

                        // Verifica se alguma linha foi afetada para determinar o sucesso da operação
                        if (rowsAffected > 0)
                        {
                            return "Dados do funcionário atualizados com sucesso!";
                        }
                        else
                        {
                            return "Funcionário não encontrado para alteração.";
                        }
                    }
                    catch (Exception ex)
                    {
                        return "Erro ao alterar dados do Funcionário: " + ex.Message; // Retorna a mensagem de erro em caso de exceção
                    }
                    finally
                    {
                        await conn.CloseAsync(); // Fecha a conexão com o banco de dados
                    }
                }
            }
        }

        // Declaração de um método assíncrono que pesquisa o código de um funcionário pelo nome
        public async static ValueTask<int> PesquisarCodigoPorNome(string nome)
        {
            string sqlSelect = "SELECT id FROM funcionario WHERE nome = @nome"; // Declara a consulta SQL de seleção

            ConnectionDatabase con = new ConnectionDatabase(); // Instancia a classe de conexão com o banco de dados

            // Abre uma conexão com o banco de dados
            using (NpgsqlConnection conn = con.connectionDB())
            using (NpgsqlCommand commSelect = new NpgsqlCommand(sqlSelect, conn))
                try
                {
                    commSelect.Parameters.AddWithValue("@nome", nome); // Adiciona o parâmetro necessário ao comando
                    object result = await commSelect.ExecuteScalarAsync(); // Executa a consulta de forma assíncrona e obtém o resultado

                    if (result != null)
                    {
                        return Convert.ToInt32(result); // Converte o resultado para int e retorna
                    }
                    else
                    {
                        return -1; // Retorna -1 se o nome não for encontrado
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao pesquisar código do usuário por nome: " + ex.Message); // Exibe uma mensagem de erro em caso de exceção
                    return -1; // Retorna -1 em caso de erro
                }
                finally
                {
                    await conn.CloseAsync(); // Fecha a conexão com o banco de dados
                }
        }

        public async static ValueTask<int> PesquisarCodigoDeBarrasPorCodigo(int id)
        {
            string sqlSelect = "SELECT id_funcionario FROM funcionario WHERE id = @id"; // Declara a consulta SQL de seleção

            ConnectionDatabase con = new ConnectionDatabase(); // Instancia a classe de conexão com o banco de dados

            // Abre uma conexão com o banco de dados
            using (NpgsqlConnection conn = con.connectionDB())
            using (NpgsqlCommand commSelect = new NpgsqlCommand(sqlSelect, conn))
                try
                {
                    commSelect.Parameters.AddWithValue("@id", id); // Adiciona o parâmetro necessário ao comando
                    object result = await commSelect.ExecuteScalarAsync(); // Executa a consulta de forma assíncrona e obtém o resultado

                    if (result != null)
                    {
                        return Convert.ToInt32(result); // Converte o resultado para int e retorna
                    }
                    else
                    {
                        return -1; // Retorna -1 se o nome não for encontrado
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao pesquisar código do usuário por id: " + ex.Message); // Exibe uma mensagem de erro em caso de exceção
                    return -1; // Retorna -1 em caso de erro
                }
                finally
                {
                    await conn.CloseAsync(); // Fecha a conexão com o banco de dados
                }
        }
        public static async ValueTask<bool> VerificarExistenciaId(long codigofuncionario)
        {
            bool retorno = false;

            string sql = "SELECT COUNT(*) FROM funcionario WHERE id_funcionario = @id_funcionario";
            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand commCheckCodigo = new NpgsqlCommand(sql, conn))
                {
                    try
                    {
                        commCheckCodigo.Parameters.AddWithValue("@id", codigofuncionario);

                        int countCodigo = Convert.ToInt32(await commCheckCodigo.ExecuteScalarAsync());
                        retorno = countCodigo > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao verificar existência: " + ex.Message);
                    }
                    finally
                    {
                        await conn.CloseAsync();
                    }
                }
            }

            return retorno;
        }

    }
}
