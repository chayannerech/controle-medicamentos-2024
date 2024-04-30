using ControleMedicamentos.ConsoleApp.Compartilhado;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using Controlepacientes.ConsoleApp.ModuloPaciente;
namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    internal class TelaCadastroRequisicao : TelaBase
    {
        public TelaCadastroRequisicao(TelaCadastroPaciente telaPaciente, TelaCadastroMedicamento telaMedicamento, RepositorioBase repositorio, string nome, string NOME)
        {
            this.telaPaciente = telaPaciente;
            this.telaMedicamento = telaMedicamento;
            this.repositorio = repositorio;
            tipoEntidade = nome;
            tipoEntidadePlural = NOME;
        }

        protected override void CadastrarItem(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("REQUISIÇÕES");
            Console.WriteLine("\t\t  Cadastrar\n------------------------------------------------");

            string paciente = RecebeString(" Informe o nome do paciente: ");
            int jaExiste = telaPaciente.repositorio.EsteItemExiste(paciente);
            if (jaExiste == -1) ItemNaoCadastrado(ref sair, ref repetir);
            else
            {
                string medicamento = RecebeString(" Informe o medicamento: ");
                jaExiste = telaMedicamento.repositorio.EsteItemExiste(medicamento);
                if (jaExiste == -1) ItemNaoCadastrado(ref sair, ref repetir);
                else
                {
                    int posologia = RecebeInt(" Informe a posologia: ");
                    DateTime dataValidade = RecebeData(" Informe a data de validade: ");
                    var novaRequisicao = new Requisicao(medicamento, paciente, posologia, dataValidade, repositorio.contador + 1);

                    repositorio.Cadastrar(novaRequisicao);
                    RealizadoComSucesso("cadastrada");
                }
            }
        }
        public void DarBaixaRequisicao(ref bool sair, ref bool repetir)
        {
            CabecalhoEntidade("REQUISIÇÕES");
            Console.WriteLine("\t\t   Dar Baixa");

            if (repositorio.NaoExistemItens()) AindaNaoExistemItens(ref sair, ref repetir);
            else
            {
                CabecalhoVisualizar();
                for (int i = 0; i < repositorio.entidade.Length; i++) if (repositorio.entidade[i] != null) ListaItensParaVisualizar(i);
                int indexDarBaixa = repositorio.EsteItemExiste(RecebeString("\n Informe o ID que deseja dar baixa: "));

                if (indexDarBaixa == -1) ItemNaoCadastrado(ref sair, ref repetir);
                else IdValidoParaMovimentacao(indexDarBaixa);
            }
        }

        #region Métodos Auxiliares
        #region Visualizar
        protected override void CabecalhoVisualizar() => Notificação(ConsoleColor.Blue, "\n--------------------------------------------------------------------\n ID\t| Medicamento\t| Paciente\t| Posologia\t| Validade\n--------------------------------------------------------------------\n");
        protected override void ListaItensParaVisualizar(int i) => Console.Write($" {repositorio.entidade[i].id}\t| {repositorio.entidade[i].medicamento}\t\t| {repositorio.entidade[i].paciente}\t\t| {repositorio.entidade[i].posologia}\t\t| {repositorio.entidade[i].dataValidade.ToString("d")}\n--------------------------------------------------------------------\n");
        #endregion
        #region Editar
        protected override void MenuEdicao(ref bool sair, ref bool repetir, TelaBase telaMedicamento, TelaBase telaPaciente, int indexEditar)
        {
            CabecalhoEntidade("REQUISIÇÕES");
            var objetoAuxiliar = repositorio.entidade[indexEditar]; bool editado = true;
            VisualizarParaEdicao(objetoAuxiliar);

            string opcao = RecebeString(" Ótimo! O que deseja Editar?\n 1. medicamento\n 2. paciente\n 3. posologia\n 4. data de validade\n\n R. para retornar\n S. para sair\n\n Digite: ");
            switch (opcao)
            {
                case "1":
                    objetoAuxiliar.medicamento = RecebeString(" Informe o novo medicamento: ");
                    int jaExiste = telaMedicamento.repositorio.EsteItemExiste(objetoAuxiliar.medicamento);
                    if (jaExiste == -1) ItemNaoCadastrado(ref sair, ref repetir);
                    break;
                case "2":
                    objetoAuxiliar.paciente = RecebeString(" Informe o novo paciente: ");
                    jaExiste = telaPaciente.repositorio.EsteItemExiste(objetoAuxiliar.paciente);
                    if (jaExiste == -1) ItemNaoCadastrado(ref sair, ref repetir);
                    break;
                case "3": objetoAuxiliar.posologia = RecebeInt("\n Informe o novo endereço: "); break;
                case "4": objetoAuxiliar.dataValidade = RecebeData("\n Informe o novo cartão SUS: "); break;
                case "R": repetir = true; break;
                case "S": sair = true; break;
                default: OpcaoInvalida(ref repetir); break;
            }
            if (opcao == "") repetir = true;
            if (!sair && !repetir)
            {
                var editarRequisicao = new Requisicao(objetoAuxiliar.medicamento, objetoAuxiliar.paciente, objetoAuxiliar.posologia, objetoAuxiliar.dataValidade, 2);
                repositorio.Editar(editarRequisicao, indexEditar);
                RealizadoComSucesso("editada");
            }
        }
        protected override void VisualizarParaEdicao(EntidadeBase objetoAuxiliar)
        {
            Console.WriteLine("\t\t    Editar");
            CabecalhoVisualizar();
            Console.Write($" {objetoAuxiliar.id}\t| {objetoAuxiliar.medicamento}\t\t| {objetoAuxiliar.paciente}\t\t| {objetoAuxiliar.posologia}\t\t| {objetoAuxiliar.dataValidade.ToString("d")}\n--------------------------------------------------------------------\n\n");
        }
        #endregion
        #region DarBaixa
        public void IdValidoParaMovimentacao(int indexDarBaixa)
        {
            repositorio.DarBaixa(telaMedicamento.repositorio.entidade, indexDarBaixa);
            if (telaMedicamento.repositorio.QuantidadeEstaCritica(indexDarBaixa)) Notificação(ConsoleColor.Red, "\n         ATENÇÃO! Nível de estoque baixo!\n");
            RealizadoComSucesso("movimentada");
        }
        #endregion
        #endregion
    }
}
