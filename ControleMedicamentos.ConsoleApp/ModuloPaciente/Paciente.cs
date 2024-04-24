namespace ControleMedicamentos.ConsoleApp.ModuloPaciente
{
    internal class Paciente
    {
        public string nome { get; set; }
        public string cpf { get; set; }
        public string endereco { get; set; }
        public string cartaoSUS { get; set; }
        //public string requisicoes { get; set; }

        public Paciente(string nome, string cpf, string endereco, string cartaoSUS)
        {
            this.nome = nome;
            this.cpf = cpf;
            this.endereco = endereco;
            this.cartaoSUS = cartaoSUS;
        }
    }
}
