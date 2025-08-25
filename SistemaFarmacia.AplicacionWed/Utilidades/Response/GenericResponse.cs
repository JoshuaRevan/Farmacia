namespace SistemaFarmacia.AplicacionWed.Utilidades.Response
{
    public class GenericResponse<TObjenct>
    {
        public bool Estado {  get; set; }

        public string? Mensaje { get; set; }

        public TObjenct? objeto { get; set; }

        public List<TObjenct>? ListaObjeto { get; set; }

    }
}
