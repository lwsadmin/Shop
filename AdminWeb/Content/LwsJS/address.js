function ProChange(Pro)
{
    $.post("/business/business/GetAddressSlect", { id: $(Pro).val(), type: 0 }, function (data)
    {
        if (data.succ)
        {
            $("#city").html(""); $("#city").html(data.str);
            CityChange($("#city"));
        }
    });
}

function CityChange(City)
{
    $.post("/business/business/GetAddressSlect", { id: $(City).val(), type: 1 }, function (data)
    {
        if (data.succ)
        {
            $("#dis").html(""); $("#dis").html(data.str);
        }
    });
}