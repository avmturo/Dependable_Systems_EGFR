var loadedClinicians = null;

/*
 *  OnLoad function, initialises page content 
 */
$(document).ready(function () {
    loadedClinicians = {};
    $("form.delete-clinician").submit(deleteClinician);
    $("table#clinician-credentials .hcp-id").each(addClinicianToLoaded);
    $("#clinician-search").on("input", searchClinician);
});

function addClinicianToLoaded(index, element) {
    loadedClinicians[$(element).html()] = element;
    //loadedClinicians.push(element.html());
}

function removeClinicianFromLoaded(clinicianId) {
    //var arrayIndex = loadedClinicians.indexOf(clinicianId);
    //if (arrayIndex > -1) {
    //    loadedClinicians.splice(arrayIndex, 1);
    //}
    var remove = loadedClinicians[clinicianId];
    delete loadedClinicians[clinicianId];
    return remove;
}

function searchClinician() {
    var searchTerm = $("#clinician-search").val();
    var searchResults = {};
    var searchRemove = {};


    for (var key in loadedClinicians) {
        var value = loadedClinicians[key];
        if (key.indexOf(searchTerm) !== -1) {
            searchResults[key] = value;
        } else {
            searchRemove[key] = value;
        }
    }
    //console.log("Finished Searching");
    //console.log("Search Results for" + searchTerm);
    //console.log(searchResults);

    //console.log("To Remove: ");
    //console.log(searchRemove);
    onSearchClinician(searchResults, searchRemove);
}

// Attempts to delete the clinician via AJAX Post
function deleteClinician() {
    if ($(this).valid()) {
        var postData = $(this).data("hcp-id");
        var row = $(this).closest("tr");

        $.post("/Admin/DeleteClinicianCredentials", { HCPId: postData },
            function (data, textStatus, jqXHR) {
                onDeleteClinician(data, textStatus, jqXHR, row, postData);
            });
    }

    // Testing without actually deleting
    // onDeleteClinician("<div data-success='True'></div>", null, null, $(this), null);
    return false;
}

// Displays the result of deleting the clinician in the appropriate DOM element.
function onDeleteClinician(resultData, textStatus, jqXHR, formElement, clinicianId) {
    // Display the result
    $("#delete-message").html(resultData);

    // If the result was successful, delete the row from the element
    if ($(resultData).data("success") === "True") {
        removeClinicianFromLoaded(clinicianId);
        $(formElement).remove();
    }
}

function onSearchClinician(searchResults, searchRemove) {
    //var currentResultIndex = 0;
    //// Iterate through all the clinician rows and compare them against the Id's to show
    //$("table.clinician-credentials .hcp-id").each(function (index, element) {
    //    if ($(element).html() == searchResults[currentResultIndex]) {
    //        // Remove any hiding that may have occurred
    //        $(element).removeAttr("style");
    //        // Increment the search result to compare with
    //        currentResultIndex++;
    //    } else {
    //        $(element).css("display", "none");
    //    }
    //});

    for (var key in searchResults) {
        var value = searchResults[key];
        $(value).parent().removeAttr("style");
    }
    for (var key in searchRemove) {
        var value = searchRemove[key];
        $(value).parent().css("display", "none");
    }
}