using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogApi.Extension
{
    public static class ModelStateExtension
    {
         public static List<string> GetErros(this ModelStateDictionary model)
         {
             var result = new List<string>();
             foreach (var item in model.Values)
             {
                result.AddRange(item.Errors.Select(error => error.ErrorMessage));
             }

             return result;
         }
    }
}