$(document).ready(function () {
    $("#editPref").click(DataPref());

    function DataPref() {
        $("#actionPref").attr("formaction").replace("/Matching/UpdateProfil");
    }
});