namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class Requisicao
    {
        public string medicamento {  get; set; }
        public string paciente { get; set; }
        public int posologia { get; private set; }
        public DateTime dataValidade { get; private set; }
    }
}
