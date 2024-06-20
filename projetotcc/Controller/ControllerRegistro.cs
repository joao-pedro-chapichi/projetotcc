using Npgsql;
using projetotcc.Database;
using projetotcc.Model;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;

namespace projetotcc.Controller
{
    public static class ControllerRegistro
    {
        public static async ValueTask<long> BuscarCodigoPorCodigoDeBarras(long codigoDeBarras)
        {
            long idUsuario = 0;

            string sql = "SELECT id FROM funcionario WHERE id_funcionario = @id_funcionario";
            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@id_funcionario", codigoDeBarras);

                        using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                idUsuario = reader.GetInt64(0);
                            }
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine("Erro ao buscar o código do funcionário: " + ex.Message);
                    }
                    finally
                    {
                        await conn.CloseAsync();
                    }
                }
            }

            return idUsuario;
        }
        public static async ValueTask<bool> VerificarExistencia(long codigofuncionario)
        {
            bool retorno = false;

            string sql = "SELECT COUNT(*) FROM funcionario WHERE id = @id";
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
        public static async ValueTask<bool> VerificarExistenciaId(long codigofuncionario)
        {
            bool retorno = false;

            string sql = "SELECT COUNT(*) FROM funcionario WHERE id = @id";
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
        public static async ValueTask<bool> VerificarUltimaAcao(long id_funcionario)
        {
            string sql = "SELECT acao FROM registro WHERE id = @id_funcionario AND data = @data ORDER BY id_registro DESC LIMIT 1";

            DateTime data = DateTime.Now.Date;
            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@id_funcionario", id_funcionario);
                        cmd.Parameters.AddWithValue("@data", data);

                        using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                string ultimaAcao = reader.GetString(0);
                                return ultimaAcao == "entrada";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao verificar última ação: " + ex.Message);
                    }
                    finally
                    {
                        await conn.CloseAsync();
                    }
                }
            }

            return false;
        }
        public static async ValueTask<string> BuscarNomeFuncionario(long codigofuncionario)
        {
            string nomeFuncionario = null;

            string sql = "SELECT nome FROM funcionario WHERE id = @id";
            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@id", codigofuncionario);

                        using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                nomeFuncionario = reader.GetString(0);
                            }
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine("Erro ao buscar o nome do funcionário: " + ex.Message);
                    }
                    finally
                    {
                        await conn.CloseAsync();
                    }
                }
            }

            return nomeFuncionario;
        }
        public static async ValueTask<string> CriarRegistro(ModelFuncionario mFunc)
        {
            string sqlInsert = "INSERT INTO registro(hora, data, id, acao) VALUES(@hora, @data, @id, @acao)";
            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand commInsert = new NpgsqlCommand(sqlInsert, conn))
                {
                    try
                    {
                        TimeSpan horaAtual = DateTime.Now.TimeOfDay;
                        TimeSpan horaMinutos = new TimeSpan(horaAtual.Hours, horaAtual.Minutes, 0);
                        DateTime dataAtual = DateTime.Now.Date;

                        long codigo = mFunc.ID;

                        if (codigo == 0)
                        {
                            return "Erro";
                        }

                        if (!await VerificarExistencia(codigo))
                        {
                            return "Usuário não existe!";
                        }

                        bool ultimaAcao = await VerificarUltimaAcao(codigo);
                        string acao = ultimaAcao ? "saida" : "entrada";

                        commInsert.Parameters.AddWithValue("@hora", horaMinutos);
                        commInsert.Parameters.AddWithValue("@data", dataAtual);
                        commInsert.Parameters.AddWithValue("@id", codigo);
                        commInsert.Parameters.AddWithValue("@acao", acao);

                        await commInsert.ExecuteNonQueryAsync();

                        return "Finalizado";
                    }
                    catch (Exception ex)
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
        public static async ValueTask<DataTable> PesquisaRegistro(ModelRegistro mRegistro)
        {
            DataTable dataTable = new DataTable();

            string sql = "SELECT hora, data, id, acao FROM registro WHERE id = @id AND acao = @acao AND data >= @dataInicio AND data <= @dataFim";
            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {

                using (NpgsqlCommand commSearch = new NpgsqlCommand(sql, conn))
                {
                    // Passar os valores das propriedades específicas do objeto mRegistro
                    commSearch.Parameters.AddWithValue("@id", mRegistro.Id);
                    commSearch.Parameters.AddWithValue("@acao", mRegistro.Acao);
                    commSearch.Parameters.AddWithValue("@dataInicio", mRegistro.DataInicio);
                    commSearch.Parameters.AddWithValue("@dataFim", mRegistro.DataFim);

                    try
                    {
                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(commSearch))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        // Ou faça o tratamento apropriado para a exceção
                    }
                    finally
                    {
                        await conn.CloseAsync();
                    }
                }
            }

            return dataTable;
        }

        public async static ValueTask<string> ExcluirRegistros(int codigo)
        {
            string sqlDelete = "DELETE FROM registro WHERE id = @id"; // Declara a consulta SQL de exclusão

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
                        commDelete.Parameters.AddWithValue("@id", codigo);
                        int rowsAffectedDelete = await commDelete.ExecuteNonQueryAsync(); // Executa a exclusão de forma assíncrona

                        // Verifica se alguma linha foi afetada para determinar o sucesso da operação
                        if (rowsAffectedDelete > 0)
                        {
                            return "Registros excluídos com sucesso!";
                        }
                        else
                        {
                            return "Registros não encontrado para exclusão.";
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


    }
}
