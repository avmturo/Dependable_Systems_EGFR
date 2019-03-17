var loadedClinicians = null;

/*
 *  OnLoad function, initialises page content 
 */
$(document).ready(function () {
    loadedClinicians = {};
    $("form.delete-clinician").submit(deleteClinician);
    $("form.edit-clinician").submit(editClinician);
    $("table#clinician-credentials .hcp-id").each(addClinicianToLoaded);
    $("#clinician-search").on("input", searchClinician);
    setupClinicianToggles();
});

function setupClinicianToggles() {
    var tableRows = $("tr");
    for (var i = 1; i < tableRows.length; i++) {
        // Show
        if (i % 2 != 0) {
            $(tableRows[i]).find("button").click(
                {
                    hideElement: tableRows[i],
                    showElement: tableRows[i + 1]
                }, toggle);
        }
        // Edit
        else {
            $(tableRows[i]).find("button").click(
                {
                    hideElement: tableRows[i],
                    showElement: tableRows[i -   1]
                }, toggle);

            $(tableRows[i]).css("display", "none");
        }
    }
}

function toggle(event) {
    console.log("toggling");
    console.log(event.data.hideElement);
    $(event.data.hideElement).css("display", "none");
    $(event.data.showElement).removeAttr("style");
    return false;
}

function hideEditClinicianForm(element) {
    // Hide the table row with the edit form
    $(element).closest("td").css("display", "none");
    // Show the non-editable version
}

function showEditClinicianForm(element) {
    var closest = $(element).closest("tr"); 
    $(closest).css("display", "none");
    var hcpId = $(closest).data("hcp-id");

    console.log(hcpId);
    var cells = $("table").find("[data-hcp-id='" + hcpId + "']");
    $(cells).each((index, ele) => {
        console.log(element);
        console.log(ele);
        if (ele != element) {
            $(ele).css("display", "block");
        }
    });
}

function addClinicianToLoaded(index, element) {
    loadedClinicians[$(element).html()] = element;
    //loadedClinicians.push(element.html());
}

function updatedLoadedClinicians(oldId, newId) {
    var element = loadedClinicians[oldId];
    delete loadedClinicians[oldId];
    loadedClinicians[newId] = element;
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

function editClinician() {
    if ($(this).valid()) {
        var currentHcp = $(this).data("hcp-id");
        var formArray = $(this).serializeArray();
        var formData = {};

        $(formArray).each((i, field) => {
            formData[field.name] = field.value;
        });
        var newHcp = formData["HCPId"];
        var newPassword = formData["ClinicianPassword"];
        
        $.post("/Admin/EditClinicianCredentials", { previousHCPId: currentHcp, HCPId: newHcp, password: newPassword },
            function (data, textStatus, jqXHR) {
                $("#delete-message").html(data);
                onEditClinician(data, textStatus, jqXHR, currentHcp, newHcp);
            });
    }
    // Prevent form submit (dont want to reload the page)
    return false;
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

function onEditClinician(resultData, textStatus, jqXHR, oldId, newId) {
    
    // update visible password
    if ($(resultData).data("success") === "True") {
        // update loaded clinician
        updatedLoadedClinicians(oldId, newId);
        // update delete form hcp-id
        $("[data-hcp-id='" + oldId + "']").each((index, element) => {
            $(element).data("hcp-id", newId);
        });

        $(".hcp-id").each((index, element) => {
            if ($(element).html() == oldId) {
                $(element).html(newId);
            }
        })


    }
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