using Npgsql;
using projetotcc.Database;
using projetotcc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetotcc.Controller
{
    public class ControllerColaborador
    {
        public void cadastrarFuncionario(ModelFuncionario modelFunc)
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

    }
}
