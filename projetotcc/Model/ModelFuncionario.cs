using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projetotcc.Model
{
    public class ModelFuncionario
    {
        private long _id;
        private long _id_funcionario;
        private string _nome;

        public long ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public long Id_funcionario
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
