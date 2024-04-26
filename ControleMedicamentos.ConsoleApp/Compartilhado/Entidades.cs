namespace ControleMedicamentos.ConsoleApp.Compartilhado
{
    internal class Entidades
    {
        public string nome { get; set; }
        public string descricao { get; set; }
        public string fornecedor { get; set; }
        public int quantidade { get; set; }
        public int quantidadeCritica { get; set; }
        public string cpf { get; set; }
        public string endereco { get; set; }
        public string cartaoSUS { get; set; }
        public string medicamento { get; set; }
        public string paciente { get; set; }
        public int posologia { get; set; }
        public DateTime dataValidade { get; set; }
        public int id { get; set; }
    }
}
