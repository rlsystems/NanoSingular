$(document).ready(function () {

    var url = new URL(window.location.href);
    var tab = url.searchParams.get("tab");    

    if(tab){
        setTab(tab); // global function
    }
});