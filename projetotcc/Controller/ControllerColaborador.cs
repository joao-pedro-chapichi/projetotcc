using Npgsql; // Importa o namespace para interação com o PostgreSQL.
using projetotcc.Database; // Importa a camada de acesso ao banco de dados do projeto.
using projetotcc.Model; // Importa a camada de modelos do projeto.
using System; // Importa classes base do .NET Framework.
using System.Data; // Importa classes para manipulação de dados.
using System.Threading.Tasks; // Importa classes para programação assíncrona.
using System.Windows.Forms; // Importa classes para a criação de interfaces de usuário no Windows Forms.

namespace projetotcc.Controller
{
    // Define a classe estática ControllerColaborador para gerenciar colaboradores.
    public static class ControllerColaborador
    {
        // Método assíncrono para cadastrar um novo funcionário.
        public static async Task cadastrarFuncionario(ModelFuncionario modelFunc)
        {
            // Verifica se já existe um funcionário com o mesmo ID.
            if (await VerificarExistenciaId(modelFunc.Id_funcionario))
            {
                MessageBox.Show("Código já cadastrado!"); // Exibe mensagem de erro se o ID já estiver cadastrado.
                return; // Encerra a execução do método.
            }

            // SQL para inserir um novo funcionário.
            string sql = "INSERT INTO funcionario(id_funcionario, nome) VALUES(@id_funcionario, @nome)";

            try
            {
                // Cria uma conexão com o banco de dados.
                using (var connection = new ConnectionDatabase().connectionDB())
                // Cria o comando SQL para ser executado.
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    // Adiciona os parâmetros ao comando.
                    cmd.Parameters.AddWithValue("nome", modelFunc.Nome);
                    cmd.Parameters.AddWithValue("id_funcionario", modelFunc.Id_funcionario);

                    // Executa o comando de forma assíncrona.
                    await cmd.ExecuteNonQueryAsync();

                    // Exibe mensagem de sucesso.
                    MessageBox.Show("Funcionário cadastrado com sucesso!", "SUCESSO!");
                }
            }
            catch (Exception ex)
            {
                // Exibe mensagem de erro em caso de exceção.
                MessageBox.Show("Erro ao cadastrar funcionário! Erro: " + ex.Message, "ERRO!");
            }
        }

        // Método assíncrono para buscar funcionários por nome e/ou ID.
        public static async ValueTask<DataTable> buscasFuncionarios(string nome, string idFuncionario)
        {
            var dataTable = new DataTable(); // Cria um DataTable para armazenar os resultados da consulta.

            // SQL básico para selecionar os funcionários.
            string sqlDeBuscar = "SELECT id_funcionario, nome FROM funcionario WHERE 1=1";

            // Adiciona filtros à consulta com base nos parâmetros fornecidos.
            if (!string.IsNullOrEmpty(nome))
                sqlDeBuscar += " AND nome LIKE @nome";
            if (!string.IsNullOrEmpty(idFuncionario))
                sqlDeBuscar += " AND CAST(id_funcionario AS TEXT) LIKE @id_funcionario";

            try
            {
                // Cria uma conexão com o banco de dados.
                using (var connection = new ConnectionDatabase().connectionDB())
                // Cria o comando SQL para ser executado.
                using (var comm = new NpgsqlCommand(sqlDeBuscar, connection))
                // Cria um adaptador para preencher o DataTable com os resultados.
                using (var adapter = new NpgsqlDataAdapter(comm))
                {
                    // Adiciona os parâmetros ao comando.
                    if (!string.IsNullOrEmpty(nome))
                        comm.Parameters.AddWithValue("@nome", "%" + nome + "%");
                    if (!string.IsNullOrEmpty(idFuncionario))
                        comm.Parameters.AddWithValue("@id_funcionario", "%" + idFuncionario + "%");

                    // Preenche o DataTable com os resultados da consulta de forma assíncrona.
                    await Task.Run(() => adapter.Fill(dataTable));
                }
            }
            catch (NpgsqlException ex)
            {
                // Exibe mensagem de erro em caso de exceção.
                MessageBox.Show("Erro ao pesquisar funcionários: " + ex.Message);
            }

            return dataTable; // Retorna o DataTable com os resultados.
        }

        // Método assíncrono para excluir um funcionário pelo ID.
        public static async ValueTask<string> ExcluirFuncionario(int codigo)
        {
            // SQL para excluir o funcionário.
            string sqlDelete = "DELETE FROM funcionario WHERE id_funcionario = @id_funcionario";

            try
            {
                // Cria uma conexão com o banco de dados.
                using (var connection = new ConnectionDatabase().connectionDB())
                // Cria o comando SQL para ser executado.
                using (var commDelete = new NpgsqlCommand(sqlDelete, connection))
                {
                    // Adiciona o parâmetro ao comando.
                    commDelete.Parameters.AddWithValue("@id_funcionario", codigo);

                    // Executa o comando de forma assíncrona e obtém o número de linhas afetadas.
                    int rowsAffectedDelete = await commDelete.ExecuteNonQueryAsync();

                    // Retorna mensagem de sucesso ou erro com base no resultado.
                    return rowsAffectedDelete > 0 ? "Colaborador excluído com sucesso!" : "Colaborador não encontrado para exclusão.";
                }
            }
            catch (Exception ex)
            {
                // Retorna a mensagem de erro em caso de exceção.
                return $"Erro: {ex.Message}";
            }
        }

        // Método assíncrono para alterar os dados de um funcionário.
        public static async ValueTask<string> AlterarDados(ModelFuncionario modelFunc, int id)
        {
            // SQL para atualizar os dados do funcionário.
            string sqlUpdate = "UPDATE funcionario SET id_funcionario = @id_funcionario, nome = @nome WHERE id = @id";

            try
            {
                // Cria uma conexão com o banco de dados.
                using (var connection = new ConnectionDatabase().connectionDB())
                // Cria o comando SQL para ser executado.
                using (var commUpdate = new NpgsqlCommand(sqlUpdate, connection))
                {
                    // Adiciona os parâmetros ao comando.
                    commUpdate.Parameters.AddWithValue("@id_funcionario", modelFunc.Id_funcionario);
                    commUpdate.Parameters.AddWithValue("@nome", modelFunc.Nome);
                    commUpdate.Parameters.AddWithValue("@id", id);

                    // Executa o comando de forma assíncrona e obtém o número de linhas afetadas.
                    int rowsAffected = await commUpdate.ExecuteNonQueryAsync();

                    // Retorna mensagem de sucesso ou erro com base no resultado.
                    return rowsAffected > 0 ? "Dados do funcionário atualizados com sucesso!" : "Funcionário não encontrado para alteração.";
                }
            }
            catch (Exception ex)
            {
                // Retorna a mensagem de erro em caso de exceção.
                return "Erro ao alterar dados do Funcionário: " + ex.Message;
            }
        }

        // Método assíncrono para pesquisar o código de um funcionário pelo nome.
        public static async ValueTask<int> PesquisarCodigoPorNome(string nome)
        {
            // SQL para selecionar o código do funcionário pelo nome.
            string sqlSelect = "SELECT id FROM funcionario WHERE nome = @nome";

            try
            {
                // Cria uma conexão com o banco de dados.
                using (var connection = new ConnectionDatabase().connectionDB())
                // Cria o comando SQL para ser executado.
                using (var commSelect = new NpgsqlCommand(sqlSelect, connection))
                {
                    // Adiciona o parâmetro ao comando.
                    commSelect.Parameters.AddWithValue("@nome", nome);

                    // Executa o comando de forma assíncrona e obtém o resultado.
                    object result = await commSelect.ExecuteScalarAsync();

                    // Retorna o resultado convertido para int, ou -1 se o resultado for nulo.
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch (Exception ex)
            {
                // Exibe mensagem de erro em caso de exceção e retorna -1.
                Console.WriteLine("Erro ao pesquisar código do usuário por nome: " + ex.Message);
                return -1;
            }
        }

        public static async ValueTask<string> PesquisarNomePorCodigo(int id)
        {
            // SQL para selecionar o código do funcionário pelo nome.
            string sqlSelect = "SELECT nome FROM funcionario WHERE id = @id";

            try
            {
                // Cria uma conexão com o banco de dados.
                using (var connection = new ConnectionDatabase().connectionDB())
                // Cria o comando SQL para ser executado.
                using (var commSelect = new NpgsqlCommand(sqlSelect, connection))
                {
                    // Adiciona o parâmetro ao comando.
                    commSelect.Parameters.AddWithValue("@id", id);

                    // Executa o comando de forma assíncrona e obtém o resultado.
                    object result = await commSelect.ExecuteScalarAsync();


                    return result.ToString();
                }
            }
            catch (Exception ex)
            {
                // Exibe mensagem de erro em caso de exceção e retorna -1.
                Console.WriteLine("Erro ao pesquisar código do usuário por nome: " + ex.Message);
                return null;
            }
        }

        // Método assíncrono para pesquisar o código de barras de um funcionário pelo ID.
        public static async ValueTask<int> PesquisarCodigoDeBarrasPorCodigo(int id)
        {
            // SQL para selecionar o código de barras do funcionário pelo ID.
            string sqlSelect = "SELECT id_funcionario FROM funcionario WHERE id = @id";

            try
            {
                // Cria uma conexão com o banco de dados.
                using (var connection = new ConnectionDatabase().connectionDB())
                // Cria o comando SQL para ser executado.
                using (var commSelect = new NpgsqlCommand(sqlSelect, connection))
                {
                    // Adiciona o parâmetro ao comando.
                    commSelect.Parameters.AddWithValue("@id", id);

                    // Executa o comando de forma assíncrona e obtém o resultado.
                    object result = await commSelect.ExecuteScalarAsync();

                    // Retorna o resultado convertido para int, ou -1 se o resultado for nulo.
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch (Exception ex)
            {
                // Exibe mensagem de erro em caso de exceção e retorna -1.
                Console.WriteLine("Erro ao pesquisar código do usuário por id: " + ex.Message);
                return -1;
            }
        }

        // Método assíncrono para verificar se um ID de funcionário já existe no banco de dados.
        public static async ValueTask<bool> VerificarExistenciaId(long codigofuncionario)
        {
            // SQL para contar
            string sql = "SELECT COUNT(*) FROM funcionario WHERE id_funcionario = @id_funcionario";
            try
            {
                // Cria uma conexão com o banco de dados.
                using (var connection = new ConnectionDatabase().connectionDB())
                // Cria o comando SQL para ser executado.
                using (var commCheckCodigo = new NpgsqlCommand(sql, connection))
                {
                    // Adiciona o parâmetro ao comando.
                    commCheckCodigo.Parameters.AddWithValue("@id_funcionario", codigofuncionario);

                    // Executa o comando de forma assíncrona e obtém o número de funcionários com o ID fornecido.
                    int countCodigo = Convert.ToInt32(await commCheckCodigo.ExecuteScalarAsync());

                    // Retorna true se o ID já estiver cadastrado, caso contrário, retorna false.
                    return countCodigo > 0;
                }
            }
            catch (Exception ex)
            {
                // Exibe mensagem de erro em caso de exceção e retorna false.
                Console.WriteLine("Erro ao verificar existência: " + ex.Message);
                return false;
            }
        }
    }
}
