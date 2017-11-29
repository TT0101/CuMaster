function toggleCollapsePane(barElement, paneElement, toggleElement)
{
    var panel = $('#' + paneElement);
    var toggle = $("#" + toggleElement);
    var bar = $("#" + barElement);

    if (panel.hasClass('show'))
    {
        panel.removeClass('show');
        toggle.removeClass('glyphicon-collapse-down').addClass('glyphicon-collapse-up');
        bar.addClass('bar-collapsed');
    }
    else
    {
        panel.addClass('show');
        toggle.addClass('glyphicon-collapse-down').removeClass('glyphicon-collapse-up');
        bar.removeClass('bar-collapsed');
    }
}

//note for the following siteRoot must be defined in the layout as embedded js to get the site name.  Anything beyond the root must be defined as a path (so, 'Home' but 'Shared/SubFolder/mycontroller')
function ajaxCallPost(controller, callFunction, data, onSuccess)
{
    $.ajax(
        {
            dataType: "json",
            type: "POST",
            data: data,
            url: siteRoot + "/" + controller + "/" + callFunction,
            success: onSuccess,
            failure: onError
        });

}

function ajaxCallGet(controller, callFunction, data, onSuccess)
{
    if (location === null)
        location = $(location).attr('pathname');

    $.ajax(
        {
            dataType: "json",
            type: "GET",
            data: data,
            url: siteRoot + "/" + controller + "/" + callFunction,
            success: onSuccess,
            failure: onError
        });

}

function onError(error)
{
    alert(error); //change this to alert box later...
    $("#errorAlert #errorMessage").text(message);
    $("#errorAlert").removeClass('hidden').show();
}

function hideError()
{
    $("#errorAlert").addClass('hidden').hide();
}


 //html 5 geolocation
function getLocationObject()
{
    var posObj = new object();

    if (navigator.geolocation)
    {
        posObj = navigator.geolocation.getCurrentPosition();

    } 
    return posObj;
}

        //function showPosition(position) {
        //    x.innerHTML = "Latitude: " + position.coords.latitude +
        //        "<br>Longitude: " + position.coords.longitude;
        //}

function ajaxCallFailed(obj, textStatus, errorThrown)
{
    var message = "";
    if (obj.responseText != undefined)
    {
        var match = obj.responseText.match(/<!DOCTYPE html/);
        if (match != null && match.length > 0)
        {
            var htmlDoc = $('<output>').append($.parseHTML(obj.responseText));
            var error = htmlDoc.find('title');
            message = error[0].text;
        }
        else
        {
            message = (obj.responseText != undefined) ? JSON.parse(obj.responseText).Message : "";
        }
        message = message + " " + (errorThrown != undefined || errorThrown != null) ? errorThrown : "";
        onError(message);
    }
}

function getQueryStringParam(paramName)
{
    var paramVal;
    var aKeyVal;
    var key;
    var val;

    var queryString = window.location.search.substr(1);
    var keyValues = queryString.split('&');
    for (var i = 0; i < keyValues.length; i++)
    {
        aKeyVal = keyValues[i].split('=');
        key = aKeyVal[0];
        val = aKeyVal[1];
        if (key.toUpperCase() == paramName.toUpperCase())
        {
            return val;
        }
    }
    return "";
}

function createDataTableParams(aoData)
{
    var p = new Object();
    p.draw = aoData[0].value;
    p.order = aoData[2].value;
    p.start = aoData[3].value;
    p.length = aoData[4].value;
    p.search = aoData[5].value;
    p.orderColDataName = aoData[1].value[aoData[2].value[0].column].data;
    p.cols = aoData[1].value;

    return p;
}

function clearValidation(formElement)
{
    //Internal $.validator is exposed through $(form).validate()
    var validator = $(formElement).validate();
    //Iterate through named elements inside of the form, and mark them as error free
    $('[name]', formElement).each(function () {
        validator.successList.push(this);//mark as error free
        validator.showErrors();//remove error messages if present
    });
    validator.resetForm();//remove error class on name elements and clear history
    validator.reset();//remove all error and success data
}
