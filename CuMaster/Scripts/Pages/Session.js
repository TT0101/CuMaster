var _location = null;
var _ip = null;

function tryGetGeolocation()
{
    var options =
        {
            enableHighAccuracy: false,
            maximumAge: 10,
            timeout: 5000
        };

    navigator.geolocation.getCurrentPosition(geolocationSuccessful, geolocationFailed, options);
}

function geolocationSuccessful(position)
{
    _location = position.coords;

    createSession();
}

function geolocationFailed()
{
    getIPAndCreateSession();
}

function getIPAndCreateSession()
{
    $.getJSON("https://api.ipify.org?format=jsonp&callback=?",
    function (json)
        {
            _ip = json.ip;
        }
    );

    createSession();
}

function createSession()
{
    
    var data = new Object();
    data.Coords = _location;
    data.IP = _ip;
    ajaxCallPost("Session", "CreateSession", data, function () { });
}