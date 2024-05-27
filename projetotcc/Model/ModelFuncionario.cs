using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projetotcc.Model
{
    public class ModelFuncionario
    {

        private int _id_funcionario;
        private string _nome;


        public int Id_funcionario
        {
            get { return _id_funcionario; }
            set { _id_funcionario = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }


    }
}
