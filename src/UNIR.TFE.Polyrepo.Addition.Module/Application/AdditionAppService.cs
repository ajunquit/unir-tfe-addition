namespace UNIR.TFE.Polyrepo.Addition.Module.Application
{
    public class AdditionAppService : IAdditionAppService
    {
        public string Key => "add";

        public decimal Execute(decimal a, decimal b) => a + b;
    }
}
