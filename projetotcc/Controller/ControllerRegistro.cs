﻿using Npgsql;
using projetotcc.Database;
using projetotcc.Model;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public static async ValueTask<(bool, TimeSpan?)> VerificarUltimaAcao(long id_funcionario)
        {
            string sql = @"SELECT acao, hora 
                   FROM registro 
                   WHERE id = @id_funcionario AND data = @data 
                   ORDER BY id_registro DESC LIMIT 1";

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
                                TimeSpan hora = reader.GetTimeSpan(1);

                                return (ultimaAcao == "entrada", hora);
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

            return (false, null);
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
                            return "Erro: Código do funcionário inválido.";
                        }

                        if (!await VerificarExistencia(codigo))
                        {
                            return "Usuário não existe!";
                        }

                        // Obtém a última ação e a hora correspondente
                        var (ultimaAcaoFoiEntrada, horaUltimaAcao) = await VerificarUltimaAcao(codigo);
                        string acao = ultimaAcaoFoiEntrada ? "saida" : "entrada";

                        commInsert.Parameters.AddWithValue("@hora", horaMinutos);
                        commInsert.Parameters.AddWithValue("@data", dataAtual);
                        commInsert.Parameters.AddWithValue("@id", codigo);
                        commInsert.Parameters.AddWithValue("@acao", acao);

                        await commInsert.ExecuteNonQueryAsync();

                        return "Registro criado com sucesso.";
                    }
                    catch (Exception ex)
                    {
                        return $"Erro ao criar registro: {ex.Message}";
                    }
                    finally
                    {
                        await conn.CloseAsync();
                    }
                }
            }
        }

        public static async ValueTask<DataTable> PesquisaRegistro(ModelRegistro mRegistro, string estado = null)
        {
            DataTable dataTable = new DataTable();
            string sql = @"SELECT r.hora, r.data, f.id_funcionario, r.acao
                            FROM registro r
                            LEFT JOIN funcionario f ON r.id = f.id
                            WHERE r.data >= @dataInicio AND r.data <= @dataFim
                            "; 

            // Adicionando condições opcionais
            if (mRegistro.Id != 0)
            {
                sql += " AND r.id = @id";
            }
            if (!string.IsNullOrEmpty(mRegistro.Acao))
            {
                sql += " AND r.acao = @acao";
            }
            if (!string.IsNullOrEmpty(estado))
            {
                sql += " AND f.status = @estado";
            }

            ConnectionDatabase con = new ConnectionDatabase();

            try
            {
                using (NpgsqlConnection conn = con.connectionDB())
                {

                    using (NpgsqlCommand commSearch = new NpgsqlCommand(sql, conn))
                    {
                        // Adicionando parâmetros obrigatórios
                        commSearch.Parameters.AddWithValue("@dataInicio", mRegistro.DataInicio);
                        commSearch.Parameters.AddWithValue("@dataFim", mRegistro.DataFim);

                        // Adicionando parâmetros opcionais
                        if (mRegistro.Id != 0)
                        {
                            commSearch.Parameters.AddWithValue("@id", mRegistro.Id);
                        }
                        if (!string.IsNullOrEmpty(mRegistro.Acao))
                        {
                            commSearch.Parameters.AddWithValue("@acao", mRegistro.Acao);
                        }
                        if (!string.IsNullOrEmpty(estado))
                        {
                            commSearch.Parameters.AddWithValue("@estado", estado);
                        }

                        // Preenchendo o DataTable com os dados retornados pela consulta
                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(commSearch))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Erro na consulta ao banco de dados: {ex.Message}");
            }

            return dataTable;
        }
        public static async ValueTask<DataTable> PesquisaRegistroHoje()
        {
            DataTable dataTable = new DataTable();
            DateTime hoje = DateTime.Now.Date;
            string sql = @"
            SELECT r.hora, r.data, f.nome, f.id_funcionario, r.acao 
            FROM registro r
            JOIN funcionario f ON r.id = f.id
            WHERE r.data = @dataHoje
            ORDER BY r.id_registro DESC";

            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand commSearch = new NpgsqlCommand(sql, conn))
                {
                    commSearch.Parameters.AddWithValue("@dataHoje", hoje);

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
            string sqlDelete = "DELETE FROM registro WHERE id = @id";
            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                try
                {
                    using (NpgsqlCommand commDelete = new NpgsqlCommand(sqlDelete, conn))
                    {
                        commDelete.Parameters.AddWithValue("@id", codigo);
                        int rowsAffectedDelete = await commDelete.ExecuteNonQueryAsync();

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
