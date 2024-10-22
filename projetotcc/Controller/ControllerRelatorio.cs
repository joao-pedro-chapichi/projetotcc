using iTextSharp.text;
using Npgsql;
using projetotcc.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetotcc.Controller
{
    public static class ControllerRelatorio
    {
        public static async Task<DataTable> SomarHorasEDias(DateTime dataInicial, DateTime dataFinal)
        {
            // SQL para somar os valores das horas em um determinado período
            string sql = @"
                SELECT
                    f.nome,
                    COUNT(DISTINCT r.data) AS dias_presentes,
                    ROUND(SUM(EXTRACT(EPOCH FROM r.sum_horas)) / 3600, 2) AS horas_trabalhadas
                FROM
                    registro r
                JOIN
                    funcionario f ON r.id = f.id
                WHERE
                    r.data >= @datainicial 
                    AND r.data <= @datafinal
                GROUP BY
                    f.nome
                HAVING
                    SUM(EXTRACT(EPOCH FROM r.sum_horas)) > 0";

            try
            {
                // Criar um DataTable para armazenar os resultados
                DataTable dataTable = new DataTable();

                // Conectar ao banco de dados
                using (var connection = new ConnectionDatabase().connectionDB())
                // Criar o comando SQL para execução
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    // Adicionar os parâmetros da consulta
                    cmd.Parameters.AddWithValue("@datainicial", dataInicial);
                    cmd.Parameters.AddWithValue("@datafinal", dataFinal);

                    // Executar a consulta e preencher o DataTable com os resultados
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Carregar os dados no DataTable
                        dataTable.Load(reader);
                    }
                }

                dataTable.Columns["nome"].ColumnName = "NOME";
                dataTable.Columns["horas_trabalhadas"].ColumnName = "HORAS TRABALHADAS";
                dataTable.Columns["dias_presentes"].ColumnName = "DIAS PRESENTES";


                // Retornar o DataTable preenchido
                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao somar as horas e dias: " + ex.Message);
                return null; // Retorna null em caso de erro
            }
        }

        public static async Task<DataTable> GerarRelatorioDetalhado(DateTime dataInicial, DateTime dataFinal, string cpf)
        {
            // SQL para somar os valores das horas em um determinado período
            string sql = @"
                        SELECT
                        SUM(R.SUM_HORAS),
                              MIN(R.HORA),
                              MAX(R.HORA),
                              R.DATA,
                              F.NOME
                            FROM
                              REGISTRO R
                            INNER JOIN FUNCIONARIO F
                            ON F.ID = R.ID
                            WHERE
                              DATA BETWEEN @datainicial  AND @datafinal
                              AND F.CPF = @cpf
                            GROUP BY
                              R.DATA,
                              F.ID
                             ORDER BY R.DATA";

            try
            {
                // Criar um DataTable para armazenar os resultados
                DataTable dataTable = new DataTable();

                // Conectar ao banco de dados
                using (var connection = new ConnectionDatabase().connectionDB())
                // Criar o comando SQL para execução
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    // Adicionar os parâmetros da consulta
                    cmd.Parameters.AddWithValue("@datainicial", dataInicial);
                    cmd.Parameters.AddWithValue("@datafinal", dataFinal);
                    cmd.Parameters.AddWithValue("@cpf", cpf);


                    // Executar a consulta e preencher o DataTable com os resultados
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Carregar os dados no DataTable
                        dataTable.Load(reader);
                    }
                }

                dataTable.Columns["sum"].ColumnName = "TOTAL DE HORAS";
                dataTable.Columns["max"].ColumnName = "HORARIO FIM";
                dataTable.Columns["min"].ColumnName = "HORARIO INICIO";
                dataTable.Columns["data"].ColumnName = "DATA";
                dataTable.Columns["nome"].ColumnName = "NOME";


                // Retornar o DataTable preenchido
                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar relátorio detalhados: " + ex.Message);
                return null; // Retorna null em caso de erro
            }
        }





    }
}
