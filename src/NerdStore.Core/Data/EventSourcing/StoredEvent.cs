namespace NerdStore.Core.Data.EventSourcing
{
    public class StoredEvent
    {
        public Guid Id { get; private set; }
        public string Tipo { get; private set; }
        public DateTime DataOcorrencia { get; private set; }

        // Dados é a serialização Json do event completo, para pode ser deserializado posteriormente
        public string Dados { get; private set; }

        public StoredEvent(Guid id, string tipo, DateTime dataOcorrencia, string dados)
        {
            Id = id;
            Tipo = tipo;
            DataOcorrencia = dataOcorrencia;
            Dados = dados;
        }
    }
}
