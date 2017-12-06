var _checkedBox = '<center><span class="glyphicon glyphicon-check" style="font-size:20px;"/></center>';
var _uncheckedBox = '<center><span class="glyphicon glyphicon-unchecked" style="font-size:20px;"/></center>';
var _selectedEntries = new Array();

$(document).ready(function ()
{
    bindAccordianClicksTracker();
    initTrackerTable();
});

function bindAccordianClicksTracker()
{
    $("#trackerCollapse").on('click', function ()
    {
        toggleCollapsePane("trackerCollapse", "trackerCollapsePane", "trackerCollapseToggle");
    });
}

function initTrackerTable()
{
    
    $("#trackerTable").DataTable(
        {
            columns: 
            [
                //{ data: 'EntryID', hidden: true, sortable: false},
                { data: 'EntryName', name:'EntryName', title: 'Entry'},
                { data: 'AmountFrom', name: 'AmountFrom', title: 'Converted Amount', width: "100px"},
                { data: 'CurrencyFrom', name: 'CurrencyFrom', title: 'Currency From', width: "150px"},
                { data: 'AmountTo', name:'AmountTo', title: 'New Amount', width: "100px" },
                { data: 'CurrencyTo', name: 'CurrencyTo', title: 'Currency To', width: "150px" },
                { data: 'LastUpdated', name: 'LastUpdated', title: 'Last Updated', width: "100px" },
                { data: null, name: 'UpdateRate', title: 'Update?', width: '20px' },
                {data: null, title: 'Delete', width: '20px'}
                
            ],
            autoWidth: false,
            deferRender: true,
            processing: true,
            serverSide: true,
            order: [[0, 'desc']],
            createdRow: function (row, data, index)
            {
                if (data.AutoUpdate) // Also, need to make cursor turn into hand when hovered over row...
                {
                    $('td', row).eq(6).html(_checkedBox);
                }
                else
                {
                    $('td', row).eq(6).html(_uncheckedBox);
                }

                $('td', row).eq(7).html("<button class='btn btn-primary deleteButton' onclick='deleteEntry(" + data.EntryID + ");'><span class='glyphicon glyphicon-trash'></span></button>");

                $('td', row).eq(5).html(convertToLocalTime(data.LastUpdateString));
            },
            fnServerData: function (sSource, aoData, fnCallback)
            {
                var sendData = createDataTableParams(aoData);

                $.ajax
                    ({
                        dataType: "json",
                        type: "POST",
                        data: sendData,
                        url: siteRoot + "/ConversionTracker/GetTrackerEntries",
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

    $("#trackerTable_length").addClass("text-left");
    initOnRowClick();

}

function refeshTrackerDataTable()
{
    $("#trackerTable").DataTable().ajax.reload();
}

function initOnRowClick()
{
    $('#trackerTable tbody').on('click', 'td', function ()
    {
        if ($(this)[0].cellIndex == 6)
        {
            var table = $("#trackerTable").DataTable();
            var row = table.rows(table.cell(this).index().row);
            var data = row.data()[0];
            var id = data.EntryID;

            //send back to server
            var obj = new Object();
            obj.entryID = data.EntryID;
            obj.rowID = table.cell(this).index().row;
            ajaxCallPost("ConversionTracker", "SaveAutoUpdateChange", obj, showChangeSuccessMessage);
        }
        else if($(this)[0].cellIndex < 6)
        {
            $(this).parent().toggleClass('selected');

            if ($(this).parent().hasClass('selected'))
            {
                _selectedEntries.push(data.EntryID);
            }
            else
            {
                _selectedEntries.pop(data.EntryID);
            }

            //update badge in button above (remove if not selected, add if so....)
        }
    });
}

function deleteEntry(entryID)
{
    var obj = new Object();
    obj.entryID = entryID;

    obj.rowID = 0; //not needed

    ajaxCallPost("ConversionTracker", "DeleteEntry", obj, showChangeSuccessMessage);
}

function deleteAllEntries()
{
    ajaxCallPost("ConversionTracker", "DeleteAllEntries", "{}", showChangeSuccessMessage);
}

function compareSelectedEntries()
{
    //do this another time....if time is found...
}

function showChangeSuccessMessage(response)
{
    if (response.StatusKey == "ERROR")
    {
        showUpdateFailedMessage();
    }
    else
    {
        //var table = $("#trackerTable").DataTable();
        //var row = table.rows(response.RowID);
        //var data = row.data()[0];

        //data.AutoUpdate = !data.AutoUpdate;
        //if (data.AutoUpdate)
        //{
        //    $('td', row).eq(6).html(_checkedBox);
        //}
        //else
        //{
        //    $('td', row).eq(6).html(_uncheckedBox);
        //}
        //table.fnUpdate(data, rowID, undefined, false);

        $("#trackerTable").DataTable().ajax.reload(); //refresh table on this??

        showUpdateSuccessMessage();

    }
}

function showUpdateSuccessMessage()
{
    //show success and hide after period of time
    $("#trackerUpdateSuccess").removeClass('hidden').removeClass('hide');
    var timeoutID = window.setTimeout(function ()
    {
        $("#trackerUpdateSuccess").addClass('hidden').addClass('hide');
    }, 10000);
}

function showUpdateFailedMessage()
{
    //show success and hide after period of time
    $("#trackerUpdateFailed").removeClass('hidden').removeClass('hide');
    var timeoutID = window.setTimeout(function ()
    {
        $("#trackerUpdateFailed").addClass('hidden').addClass('hide');
    }, 10000);
}
