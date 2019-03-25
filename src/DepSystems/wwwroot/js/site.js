function changeTab(viewTab, tabGroup, buttonClicked) {
    var tabs = document.getElementsByClassName(tabGroup);

    for (var i = 0; i < tabs.length; ++i) {
        if (tabs[i].classList.contains("tab-buttons")) {
            tabs[i].getElementsByClassName("btn-info")[0].classList.remove("btn-info");
        }
        else {
            tabs[i].style.display = "none";
        }
    }

    buttonClicked.classList.add("btn-info");
    document.getElementById(viewTab).style.display = "block";
}