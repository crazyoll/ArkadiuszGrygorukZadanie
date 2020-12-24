function onAdditionalData() {
    var tmp = $("#Moves").val();
    return { text: tmp };
}

function onChangeGrid(e) {
    var grid = $("#grid").data("kendoGrid");
    var selectedItem = grid.dataItem(grid.select()).Id;
    window.location.href = '/Home/Details/' + selectedItem;
}

function onSelectBar(e) {
    var dataItem = this.dataItem(e.item.index()).IMDbId;
    $.ajax({
        type: "POST",
        url: '/home/SetTempData',
        data: { id: dataItem },
    });
    window.location.href = '/Home/CreateFromApi/';
}


function onFilteringBar() {
    $('#Moves').data('kendoAutoComplete').dataSource.read();
    $('#Moves').data('kendoAutoComplete').refresh();
}