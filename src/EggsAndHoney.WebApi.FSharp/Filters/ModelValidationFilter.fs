namespace EggsAndHoney.WebApi.FSharp.Filters

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Filters

type ValidateModelAttribute() =
    inherit ActionFilterAttribute()
    override this.OnActionExecuting context =
        if not context.ModelState.IsValid then 
            context.Result <- (new BadRequestObjectResult(context.ModelState) :> IActionResult)
