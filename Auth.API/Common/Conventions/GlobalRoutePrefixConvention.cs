using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Auth.API.Common.Conventions
{
    public class GlobalRoutePrefixConvention: IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;

        public GlobalRoutePrefixConvention(string prefix)
        {
            _routePrefix = new AttributeRouteModel(new RouteAttribute(prefix));
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var matchedSelectors = controller.Selectors
                    .Where(x => x.AttributeRouteModel != null)
                    .ToList();

                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        selectorModel.AttributeRouteModel =
                            AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selectorModel.AttributeRouteModel);
                    }
                }
                else
                {
                    controller.Selectors.Add(new SelectorModel
                    {
                        AttributeRouteModel = _routePrefix
                    });
                }
            }
        }
    }
}
