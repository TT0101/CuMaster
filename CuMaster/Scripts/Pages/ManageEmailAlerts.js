$(document).ready(function ()
{
    initAlertTable();

});


function initAlertTable()
{

    $("#alertTable").DataTable(
        {
            columns:
            [
                { data: 'Email', name: 'Email', title: 'Email' },
                { data: 'CurrencyFrom', name: 'CurrencyFrom', title: 'Currency From'},
                { data: 'CurrencyTo', name: 'CurrencyTo', title: 'Currency To'},
                { data: 'PercentageChange', name: 'PercentageChange', title: 'Percentage Change', width: "50px" },
                { data: 'TimeToSendUTCStr', name: 'TimeToSend', title: 'Daily Time Sent', width: "50px" },
                { data: 'DateCreatedUTCStr', name: 'DateCreated', title: 'Alert Created On'},
                { data: 'LastSentUTCStr', name:'LastSent', title: 'Alert Last Sent'},
                { data: null, title: 'Edit', width: '20px', visible: false}, 
                { data: null, title: 'Delete', width: '20px' }

            ],
            autoWidth: false,
            deferRender: true,
            processing: true,
            serverSide: true,
            order: [[0, 'desc']],
            createdRow: function (row, data, index)
            {
                $('td', row).eq(7).html("<button class='btn btn-primary' data-id='" + data.AlertID + "' onclick='openEmailAlertModalForEdit(" + data.AlertID + ")'><span class='glyphicon glyphicon-pencil'></span></button>");
                $('td', row).eq(8).html("<button class='btn btn-primary deleteButton' onclick='deleteAlert(" + data.AlertID + ");'><span class='glyphicon glyphicon-trash'></span></button>");
                $('td', row).eq(5).html(convertToLocalTime(data.DateCreatedUTCStr));
                $('td', row).eq(6).html((data.LastSent == null) ? "Never" : convertToLocalTime(data.LastSentUTCStr));
                $('td', row).eq(4).html((data.TimeToSend == null || data.TimeToSend == 0) ? "Not Set" : convertToLocalTimeOnly(data.TimeToSendUTCStr));
                $('td', row).eq(3).html((data.PercentageChange == null) ? "Not Set" : data.PercentageChange);

            },
            fnServerData: function (sSource, aoData, fnCallback)
            {
                var sendData = createDataTableParams(aoData);

                $.ajax
                    ({
                        dataType: "json",
                        type: "POST",
                        data: sendData,
                        url: siteRoot + "/ManageEmailAlerts/GetUserEmailAlerts",
                        success: function (msg)
                        {
                            if (msg != undefined)
                            {
                                fnCallback(msg);
                            }
                        },
                        error: function (obj, textStatus, errorThrown)
                        {
                            fnCallback({ data: [], recordsTotal: 0, recordsFiltered: 0 });
                            ajaxCallFailed(obj, textStatus, errorThrown);
                        }
                    });
            }
        }
    );

    //initOnRowClick();

}

function refeshDataTable()
{
    $("#alertTable").dataTable().draw();
}

function openEmailAlertModalForEdit(id)
{
    //location.href = '@Url.Content("~/EmailAlertRegistration/LoadForAlertEmail/' + id + '");' //'@Url.Action("EmailAlertRegistration", "LoadForAlertEdit", new { alertID = ' + id + ' })';
    var data = new Object();
    data.alertID = id;
    ajaxCallPostAction("EmailAlertEdit", "LoadForAlertEdit", data, function (pv)
    {
        //$(pv).find(".modal-title").text("Edit Email Alert");
        $('#emailAlertEditContainer').html(pv);
        initOnEmailAlertEditLoad();
        //$("#modalEmailAlertEdit").modal('show');
        $("#modalEmailAlertEdit").modal('show');
    });
    
}

function deleteAlert(alertID)
{
    var obj = new Object();
    obj.entryID = alertID;

    obj.rowID = 0; //not needed

    ajaxCallPost("ManageEmailAlerts", "DeleteAlert", obj, onDelete);
}

function deleteAllAlerts()
{
    ajaxCallPost("ManageEmailAlerts", "DeleteAllAlerts", "{}", onDelete);
}

function onDelete(response)
{
    if (response.StatusKey == "ERROR")
    {
        showFailedMessage("manageFailed", function () { });
    }
    else
    {
        showSuccessMessage("manageSuccess", function () { });
        refeshDataTable();
    }
}


///modal

function initOnEmailAlertEditLoad()
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

    if ($("#txtTimeToSend").val() != null && $("#txtTimeToSend").val() != "" && $("#txtTimeToSend").val() != 0)
        $("#btnTime").click();
    else
        $("#btnThreshold").click();

    $("#btnSaveAlert").click(function ()
    {
        saveAlert();
    });

    $.validator.unobtrusive.parse($("#emailAlertForm"));
}

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

        ajaxCallPost("EmailAlertRegistration", "SaveAlert", data, onAlertSave);
    }

}

function onAlertSave(response)
{
    if (response.StatusKey == "ERROR")
    {
        showEmailUpdateFailedMessage();
    }
    else if (response.StatusKey == "SUCCESS")
    {
        showEmailUpdateSuccessMessage();
        if ($("#alertTable").length)
        {
            $("#alertTable").dataTable().draw();
        }
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

