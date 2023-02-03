using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using web_api_managemen_user.Controllers;

namespace web_api_managemen_user.Class
{
    
    public class Validation
    {
        
        [Required]
        public string kode_verifikasi { set; get; }
        [Required]
        public string serial { set; get; }
        
        public int app_id { set; get; }
    }

    public class ImagePost
    {
        [Required]
        public IFormFile foto_depan { get; set; }
        [Required]
        public IFormFile foto_belakang { get; set; }
    }

    public class DeveloperValidator : AbstractValidator<Validation>
    {
        public DeveloperValidator()
        {
            RuleFor(p => p.kode_verifikasi).NotEmpty().WithMessage("Data Tidak Boleh Kosong");
        }


    }
   
    public class ReformatValidationProblemAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is BadRequestObjectResult badRequestObjectResult)

                if (badRequestObjectResult.Value is ValidationProblemDetails data)
                {
                    ValidationAPIResult message_validation = new ValidationAPIResult();
                    try
                    {
                        
                        message_validation.status = false;
                        message_validation.message = data.Title;
                        message_validation.data = data.Errors;

                        string nss = context.ModelState.First().Key;
                        ModelError[] ern = context.ModelState[nss].Errors.ToArray();
                        string ndd = ern.First().ErrorMessage;

                        if (ndd.Contains("The JSON value could not"))
                        {
                            message_validation.message = context.ModelState.First().Key + " data type does not match";
                            context.Result = new BadRequestObjectResult(message_validation);
                        }
                        else
                            context.Result = new BadRequestObjectResult(message_validation);
                    }
                    catch
                    {
                        context.Result = new BadRequestObjectResult(message_validation);
                    }
                    


                }

            base.OnResultExecuting(context);
        }
    }
}
