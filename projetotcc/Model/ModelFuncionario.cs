
namespace projetotcc.Model
{
    public class ModelFuncionario
    {
        private long _id;
        private long _id_funcionario;
        private string _nome;
        private string _cpf;

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

        public string Cpf
        {
            get { return _cpf; }
            set { _cpf = value; }
        }


    }
}
