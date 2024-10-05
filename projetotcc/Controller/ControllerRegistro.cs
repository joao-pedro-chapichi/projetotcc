using Npgsql;
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
        public static async ValueTask<bool> VerificarExistencia(long codigofuncionario, string status)
        {
            bool retorno = false;
            string sql = $"SELECT COUNT(*) FROM funcionario WHERE id = @id AND status = @status";
            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand commCheckCodigo = new NpgsqlCommand(sql, conn))
                {
                    try
                    {
                        commCheckCodigo.Parameters.AddWithValue("@id", codigofuncionario);
                        commCheckCodigo.Parameters.AddWithValue("@status", status);

                        int countCodigo = Convert.ToInt32(await commCheckCodigo.ExecuteScalarAsync());
                        
                        if(countCodigo > 0)
                        {
                            retorno = true;
                        }
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
            // Atualize a instrução SQL para incluir sum_horas, se necessário
            string sqlInsert = "INSERT INTO registro(hora, data, id, acao, sum_horas) VALUES(@hora, @data, @id, @acao, @sum_horas)";
            ConnectionDatabase con = new ConnectionDatabase();

            using (NpgsqlConnection conn = con.connectionDB())
            {
                using (NpgsqlCommand commInsert = new NpgsqlCommand(sqlInsert, conn))
                {
                    try
                    {
                        // Criando um TimeSpan com horas e minutos apenas
                        TimeSpan horaAtual = DateTime.Now.TimeOfDay;
                        TimeSpan horaMinutos = new TimeSpan(horaAtual.Hours, horaAtual.Minutes, 0); // Zera os segundos
                        TimeSpan? tempoTrabalhado = null; // Definido como null por padrão
                        DateTime dataAtual = DateTime.Now.Date;
                        long codigo = mFunc.ID;

                        if (codigo == 0)
                        {
                            return "Erro: Código do funcionário inválido.";
                        }

                        if (!await VerificarExistencia(codigo, "ATIVO"))
                        {
                            return "Usuário não existe!";
                        }

                        // Obtém a última ação e a hora correspondente
                        var (ultimaAcaoFoiEntrada, horaUltimaAcao) = await VerificarUltimaAcao(codigo);
                        string acao = ultimaAcaoFoiEntrada ? "saida" : "entrada";

                        // Se a última ação foi "entrada" e estamos registrando "saída", calcula o tempo trabalhado
                        if (acao == "saida" && horaUltimaAcao.HasValue)
                        {
                            tempoTrabalhado = horaMinutos - horaUltimaAcao.Value; // Subtrai considerando só horas e minutos
                            //MessageBox.Show($"Tempo Trabalhado: {tempoTrabalhado.Value.ToString(@"hh\:mm")}");
                        }

                        commInsert.Parameters.AddWithValue("@hora", horaMinutos);
                        commInsert.Parameters.AddWithValue("@data", dataAtual);
                        commInsert.Parameters.AddWithValue("@id", codigo);
                        commInsert.Parameters.AddWithValue("@acao", acao);

                        // Se o tempoTrabalhado foi calculado, adiciona ao comando; caso contrário, insere DBNull
                        commInsert.Parameters.AddWithValue("@sum_horas", tempoTrabalhado.HasValue ? (object)tempoTrabalhado.Value : DBNull.Value);

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
                            commSearch.Parameters.AddWithValue("@estado", estado.ToUpper());
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

            if (dataTable.Rows.Count > 0)
            {
                if (dataTable.Columns.Contains("data"))
                    dataTable.Columns["data"].ColumnName = "DATA";
                if (dataTable.Columns.Contains("hora")) 
                    dataTable.Columns["hora"].ColumnName = "HORA";
                if (dataTable.Columns.Contains("acao")) 
                    dataTable.Columns["acao"].ColumnName = "AÇÃO";
                if (dataTable.Columns.Contains("id_funcionario")) 
                    dataTable.Columns["id_funcionario"].ColumnName = "CÓDIGO";
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
