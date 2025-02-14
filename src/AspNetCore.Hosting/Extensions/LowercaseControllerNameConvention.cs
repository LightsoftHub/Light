using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Light.AspNetCore.Hosting.Extensions
{
    public class LowercaseControllerNameConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerName = controller.ControllerName;

            if (controllerName != null)
            {
                controller.ControllerName = controllerName.ConvertName();
            }
        }
    }
}
