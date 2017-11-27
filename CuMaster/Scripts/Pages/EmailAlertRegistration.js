$(document).ready(function ()
{
    $("#btnThreshold").on('click', function ()
    {
        $("#alertOptionThreshold").collapse('show');
        $("#alertOptionTime").collapse('hide');

    });

    $("#btnTime").on('click', function ()
    {
        $("#alertOptionThreshold").collapse('hide');
        $("#alertOptionTime").collapse('show');

    });

    $("#btnThreshold").click();

    $("#txtEmail").keyup(function ()
    {
        if (!$(this)[0].checkValidity() || $(this).val() == "" || !$(this).valid())
        {
            $("#btnDeleteAlerts").addClass('disabled');
        }
        else
        {
            $("#btnDeleteAlerts").removeClass('disabled');
        }
    });

    $("#btnDeleteAlerts").click(function ()
    {
        deleteAlerts();
    });

    $("#btnSaveAlert").click(function ()
    {
        saveAlert();
    });

    $.validator.unobtrusive.parse($("#emailAlertForm"));
    
});

function CreateDataViewObject()
{
    var obj = new Object();
    obj.Email = $("#txtEmail").val();
    obj.CurrencyFrom = $("#emailFromCurrency option:selected").val();
    obj.CurrencyTo = $("#emailToCurrency option:selected").val();
    obj.PercentageChange = $("#txtThreshold").val();
    obj.TimeToSend = $("#txtTimeToSend").val();

    return obj;
}

function saveAlert()
{
    if ($("#emailAlertForm").valid())
    {
        var data = CreateDataViewObject();

        ajaxCallPost("/CuMaster/EmailAlertRegistration", "SaveAlert", data, onAlertSave);
    }

}

function deleteAlerts()
{
    if ($("#txtEmail").valid())
    {
        if (confirm("Are you sure you wish to delete all email alerts associated with this email?"))
        {
            var data = new Object();
            data.Email = $("#txtEmail").val();

            ajaxCallPost("/CuMaster/EmailAlertRegistration", "DeleteAlerts", data, onDeleteComplete);
        }

    }
}

function onAlertSave(response)
{
    if (response.StatusKey == "ERROR")
    {
        showEmailUpdateFailedMessage();
    }
    else if(response.StatusKey == "SUCCESS")
    {

        showEmailUpdateSuccessMessage();

    }
}

function onDeleteComplete(response)
{
    if (response.StatusKey == "ERROR")
    {
        showEmailUpdateFailedMessage();
    }
    else if (response.StatusKey == "SUCCESS")
    {

        showEmailDeleteSuccessMessage();

    }
}

function showEmailUpdateSuccessMessage()
{
    //show success and hide after period of time
    $("#emailAlertSuccess").removeClass('hidden').removeClass('hide');
    var timeoutID = window.setTimeout(function ()
    {
        $("#emailAlertSuccess").addClass('hidden').addClass('hide');
        $("#btnCloseAlert").click();
    }, 2000);
}

function showEmailDeleteSuccessMessage()
{
    $("#emailAlertDeleteSuccess").removeClass('hidden').removeClass('hide');
    var timeoutID = window.setTimeout(function ()
    {
        $("#emailAlertDeleteSuccess").addClass('hidden').addClass('hide');
    }, 8000);
}

function showEmailUpdateFailedMessage()
{
    //show success and hide after period of time
    $("#emailAlertFailed").removeClass('hidden').removeClass('hide');
    var timeoutID = window.setTimeout(function ()
    {
        $("#emailAlertFailed").addClass('hidden').addClass('hide');
    }, 8000);
}

$("#modalEmailAlert").on('hidden.bs.modal', function ()
{
    $("#txtEmail").val('');
    $("#txtThreshold").val('');
    $("#txtTimeToSend").val('');
    clearValidation($("#emailAlertForm"));
});

