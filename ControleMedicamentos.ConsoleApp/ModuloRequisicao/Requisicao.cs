namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class Requisicao
    {
        public string medicamento {  get; set; }
        public string paciente { get; set; }
        public int posologia { get; set; }
        public DateTime dataValidade { get; set; }
        public int id { get; set; }

        public Requisicao(string medicamento, string paciente, int posologia, DateTime dataValidade, int id)
        {
            this.medicamento = medicamento;
            this.paciente = paciente;
            this.posologia = posologia;
            this.dataValidade = dataValidade;
            this.id = id;
        }
    }
}
