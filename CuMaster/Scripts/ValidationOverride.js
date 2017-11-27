
$.validator.unobtrusive.options =
{
            errorPlacement: function (label, element)
            {
                // Add Bootstrap classes to newly added elements
                label.parents('.form-group').addClass('has-error');
                label.addClass('text-danger');
            },

            success: function (label)
            {
                // Remove error class from <div class="form-group">, but don't worry about
                // validation error messages as the plugin is going to remove it anyway
                label.parents('.form-group').removeClass('has-error');
            }
}

jQuery.validator.setDefaults(
{
    highlight: function (element, errorClass, validClass)
    {
        if (element.type === 'radio')
        {
            this.findByName(element.name).addClass(errorClass).removeClass(validClass);
        } else
        {
            $(element).addClass(errorClass).removeClass(validClass);
            $(element).closest('.control-group').removeClass('success').addClass('error');
        }
    },
    unhighlight: function (element, errorClass, validClass)
    {
        if (element.type === 'radio')
        {
            this.findByName(element.name).removeClass(errorClass).addClass(validClass);
        } else
        {
            $(element).removeClass(errorClass).addClass(validClass);
            $(element).closest('.control-group').removeClass('error').addClass('success');
        }
    }
});

$(document).ready(function()
    {
        $("span.field-validation-valid, span.field-validation-error").addClass('help-inline');
        $("div.control-group").has("span.field-validation-error").addClass('error');
        $("div.validation-summary-errors").has("li:visible").addClass("alert alert-block alert-error");
    });