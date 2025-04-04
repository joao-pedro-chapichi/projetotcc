﻿using Npgsql; // Importa o namespace para interação com o PostgreSQL.
using projetotcc.Database; // Importa a camada de acesso ao banco de dados do projeto.
using projetotcc.Model; // Importa a camada de modelos do projeto.
using System; // Importa classes base do .NET Framework.
using System.Data; // Importa classes para manipulação de dados.
using System.Linq;
using System.Threading.Tasks; // Importa classes para programação assíncrona.
using System.Windows.Forms; // Importa classes para a criação de interfaces de usuário no Windows Forms.
using projetotcc.Utils;

namespace projetotcc.Controller
{
    // Define a classe estática ControllerColaborador para gerenciar colaboradores.
    public static class ControllerColaborador
    {
        // Método assíncrono para cadastrar um novo funcionário.
        public static async Task cadastrarFuncionario(ModelFuncionario modelFunc)
        {
            // Verifica se já existe um funcionário com o mesmo ID.
            if (await VerificarExistenciaId(modelFunc.Id_funcionario, "id_funcionario"))
            {
                MessageBox.Show("Código já cadastrado!");
                return;
            }

            if (await VerificarExistenciaId(modelFunc.Cpf, "cpf"))
            {
                MessageBox.Show("CPF já cadastrado!");
                return;
            }

            // Validação do CPF
            if (string.IsNullOrWhiteSpace(modelFunc.Cpf))
            {
                MessageBox.Show("O CPF não pode ser vazio!", "ERRO!");
                return;
            }

            // Verifica se o CPF contém apenas números e tem 11 dígitos
            if (!modelFunc.Cpf.All(char.IsDigit) || modelFunc.Cpf.Length != 11)
            {
                MessageBox.Show("O CPF deve conter exatamente 11 números!", "ERRO!");
                return;
            }

            // Verifica se o CPF é válido
            if (!UtilsClasse.ValidarCpf (modelFunc.Cpf))
            {
                MessageBox.Show("O CPF informado é inválido!", "ERRO!");
                return;
            }


            // SQL para inserir um novo funcionário.
            string sql = "INSERT INTO funcionario(id_funcionario, nome, status, cpf) VALUES(@id_funcionario, @nome, @status, @cpf)";

            try
            {
                using (var connection = new ConnectionDatabase().connectionDB())
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("nome", modelFunc.Nome);
                    cmd.Parameters.AddWithValue("id_funcionario", modelFunc.Id_funcionario);
                    cmd.Parameters.AddWithValue("status", "ATIVO");
                    cmd.Parameters.AddWithValue("cpf", modelFunc.Cpf);

                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show("Funcionário cadastrado com sucesso!", "SUCESSO!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar funcionário! Erro: " + ex.Message, "ERRO!");
            }
        }

        // Método assíncrono para buscar funcionários por nome e/ou ID.
        public static async ValueTask<DataTable> buscasFuncionarios(string nome, string idFuncionario, string cpf, string estado = null)
        {
            var dataTable = new DataTable(); // Cria um DataTable para armazenar os resultados da consulta.

            // SQL básico para selecionar os funcionários.
            string sqlDeBuscar = "SELECT id_funcionario, nome, cpf, status FROM funcionario WHERE 1=1";

            // Adiciona filtros à consulta com base nos parâmetros fornecidos.
            if (!string.IsNullOrEmpty(nome))
                sqlDeBuscar += " AND nome LIKE @nome";
            if (!string.IsNullOrEmpty(idFuncionario))
                sqlDeBuscar += " AND CAST(id_funcionario AS TEXT) LIKE @id_funcionario";
            if (!string.IsNullOrEmpty(cpf))
                sqlDeBuscar += " AND cpf LIKE @cpf";
            if (!string.IsNullOrEmpty(estado))
                sqlDeBuscar += " AND status = @status";

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
                        comm.Parameters.AddWithValue("@nome", nome + "%");
                    if (!string.IsNullOrEmpty(idFuncionario))
                        comm.Parameters.AddWithValue("@id_funcionario", idFuncionario + "%");
                    if (!string.IsNullOrEmpty(cpf))
                        comm.Parameters.AddWithValue("@cpf", cpf + "%");
                    if (!string.IsNullOrEmpty(estado))
                        comm.Parameters.AddWithValue("@status", estado.ToUpper());

                    // Preenche o DataTable com os resultados da consulta de forma assíncrona.
                    await Task.Run(() => adapter.Fill(dataTable));
                }
            }
            catch (NpgsqlException ex)
            {
                // Exibe mensagem de erro em caso de exceção.
                MessageBox.Show("Erro ao pesquisar funcionários: " + ex.Message);
            }

            if (dataTable.Rows.Count > 0)
            {
                dataTable.Columns["id_funcionario"].ColumnName = "CODIGO";
                dataTable.Columns["nome"].ColumnName = "NOME";
                dataTable.Columns["cpf"].ColumnName = "CPF";
                dataTable.Columns["status"].ColumnName = "STATUS";
            }

            return dataTable; // Retorna o DataTable com os resultados.
        }

        // Método assíncrono para excluir um funcionário pelo ID.
        public static async ValueTask<string> InativarFuncionario(int codigo)
        {
            object status;
            string novoStatus = "";
            string sqlSearch = "SELECT status FROM funcionario WHERE id_funcionario = @id_funcionario";
            string sqlUpdate = "UPDATE funcionario SET status = @novoStatus WHERE id_funcionario = @id_funcionario";

            try
            {
                // Cria uma conexão com o banco de dados.
                using (var connection = new ConnectionDatabase().connectionDB())
                {

                    // Busca o status do funcionário
                    using (var commSearch = new NpgsqlCommand(sqlSearch, connection))
                    {
                        // Adiciona o parâmetro ao comando.
                        commSearch.Parameters.AddWithValue("@id_funcionario", codigo);

                        // Executa o comando de forma assíncrona e obtém o status.
                        status = await commSearch.ExecuteScalarAsync();
                    }

                    if (status != null)
                    {
                        // Define o novo status com base no valor atual.
                        if (status.ToString().ToUpper() == "ATIVO")
                        {
                            novoStatus = "INATIVO";
                        }
                        else
                        {
                            novoStatus = "ATIVO";
                        }

                        // Atualiza o status do funcionário
                        using (var commUpdate = new NpgsqlCommand(sqlUpdate, connection))
                        {
                            // Adiciona os parâmetros ao comando.
                            commUpdate.Parameters.AddWithValue("@id_funcionario", codigo);
                            commUpdate.Parameters.AddWithValue("@novoStatus", novoStatus);

                            // Executa o comando de forma assíncrona e obtém o número de linhas afetadas.
                            int rowsAffectedUpdate = await commUpdate.ExecuteNonQueryAsync();

                            // Retorna mensagem de sucesso ou erro com base no resultado.
                            return rowsAffectedUpdate > 0 ? "Status do colaborador alterado com sucesso!" : "Erro ao atualizar o status do colaborador.";
                        }
                    }
                    else
                    {
                        return "Colaborador não encontrado.";
                    }
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
            string sqlUpdate = "UPDATE funcionario SET id_funcionario = @id_funcionario, nome = @nome, cpf = @cpf WHERE id = @id";

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
                    commUpdate.Parameters.AddWithValue("@cpf", modelFunc.Cpf);
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
            string sqlSelect = "SELECT id FROM funcionario WHERE nome LIKE @nome";

            try
            {
                // Cria uma conexão com o banco de dados.
                using (var connection = new ConnectionDatabase().connectionDB())
                // Cria o comando SQL para ser executado.
                using (var commSelect = new NpgsqlCommand(sqlSelect, connection))
                {
                    // Adiciona o parâmetro ao comando.
                    commSelect.Parameters.AddWithValue("@nome", "%" + nome + "%");

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
        public static async ValueTask<bool> VerificarExistenciaId(object campo, string tipo_busca)
        {
            // SQL para contar
            string sql = $"SELECT COUNT(*) FROM funcionario WHERE {tipo_busca} = @{tipo_busca}";
            try
            {
                // Cria uma conexão com o banco de dados.
                using (var connection = new ConnectionDatabase().connectionDB())
                // Cria o comando SQL para ser executado.
                using (var commCheckCodigo = new NpgsqlCommand(sql, connection))
                {
                    // Adiciona o parâmetro ao comando.
                    commCheckCodigo.Parameters.AddWithValue($"@{tipo_busca}", campo);

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

        public static bool verificarExistenciaParaCarteirinha(int codigo, out string nome)
        {

            using (var connection = new ConnectionDatabase().connectionDB())
            {
                using (var cmd = new NpgsqlCommand("SELECT nome FROM funcionario WHERE id_funcionario = @id_funcionario", connection))
                {
                    cmd.Parameters.AddWithValue("id_funcionario", codigo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nome = reader.GetString(0);
                            return true; 
                        }
                    }
                }
            }

            nome = null;
            return false;
        }

        //public static bool verificarExistenciaParaRelatorioDetalhadoCPF(string cpf, out string nome, out string idFuncionario)
        //{
        //    using (var connection = new ConnectionDatabase().connectionDB())
        //    {
        //        using (var cmd = new NpgsqlCommand("SELECT id_funcionario, nome FROM funcionario WHERE cpf = @cpf", connection))
        //        {
        //            cmd.Parameters.AddWithValue("cpf", cpf);

        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    idFuncionario = reader.GetString(0);
        //                    nome = reader.GetString(1);
        //                    return true;

        //                }
        //            }
        //        }
        //    }

        //    nome = null;
        //    idFuncionario = null; 
        //    return false;
        //}

        public static bool verificarExistenciaParaRelatorioDetalhadoCPF(string cpf, out string nome, out int idFuncionario)
        {
            using (var connection = new ConnectionDatabase().connectionDB())
            {
                using (var cmd = new NpgsqlCommand("SELECT id_funcionario, nome FROM funcionario WHERE cpf = @cpf", connection))
                {
                    cmd.Parameters.AddWithValue("cpf", cpf);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idFuncionario = reader.GetInt32(0);
                            nome = reader.GetString(1); 
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Nenhum funcionário encontrado com esse CPF."); 
                        }
                    }
                }
            }

            nome = null;
            idFuncionario = 0; 
            return false;
        }

        public static async Task<int> ObterUltimoCodigoFuncionario()
        {
            await Task.Delay(100);

            int ultimoCodigo = 0;

            using (var connection = new ConnectionDatabase().connectionDB())
            {
                //await connection.OpenAsync();
                using (var command = new NpgsqlCommand("SELECT MAX(id_funcionario) FROM funcionario", connection))
                {
                    var result = await command.ExecuteScalarAsync();
                    if (result != DBNull.Value)
                    {
                        ultimoCodigo = Convert.ToInt32(result);
                    }
                }
            }

            return Math.Max(ultimoCodigo, 1000);
        }
    }

    
}
