// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// JavaScript code related to user accounts

$(document).ready(function () {
    
    if ($('#aspValidation').is(':empty')) {
        $('#aspValidation').hide();
    } else {
        $('#aspValidation').show();
    }
});


var routeURL = location.protocol + "//" + location.host;

function showViewEditUsersModal(identifier) {
    //alert(identifier.getAttribute("id"));
    getUserDetailsById(identifier.getAttribute("id"));
    $("#viewEditUsersModal").modal("show");
}
function closeViewEditUsersModal() {
    disableModalEdit();
    $("#viewEditUsersModal").modal("hide");
}
function getUserDetailsById(pUserId) {
    const apiURL = routeURL + '/api/Users/details/' + pUserId;
    console.log("**LOG: API URL: ", apiURL);
    $.ajax({
        url: apiURL,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            console.log("**LOG: API Call Success");
            if (response.status == 1 && response.dataenum != undefined) {
                fillModalWithDataAndShow(response.dataenum);
            }
        },
        error: function (xhr) {
            console.log("**LOG: ## API Call ERROR## = ", xhr);
        }
    });
}

function approveUser() {
    tempUserId = $("#userId").attr('data-userId');
    const apiURL = routeURL + '/api/Users/approve/' + tempUserId;
    console.log("**LOG: API URL: ", apiURL);
    $.ajax({
        url: apiURL,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            console.log("**LOG: API Call Success");
            if (response.status == 1) {
                alert("User has been suceesfully approved");
                $("#btnApproveOnModal").hide();
            }
        },
        error: function (xhr) {
            console.log("**LOG: ## API Call ERROR## = ", xhr);
        }
    });
}

function fillModalWithDataAndShow(pUserDataObj) {
    $("#userId").attr('data-userId', pUserDataObj.id); // Setting the currently displayed user's id
    $("#modalInputFirstName").val(pUserDataObj.firstName);
    $("#modalInputLastName").val(pUserDataObj.lastName);
    $("#modalInputEmail").val(pUserDataObj.email);
    $("#modalInputCreationDetails").val(pUserDataObj.userCreationDate + " " + pUserDataObj.userCreationTime);
    $("#btnSaveChanges").hide();
    if (pUserDataObj.isAdminApproved) {
        $("#btnApproveOnModal").hide();
        $("#modalApproveLabel").text("User is Approved");
        $("#modalInputCheck").prop("checked", true);
    } else {
        $("#btnApproveOnModal").show();
        $("#modalApproveLabel").text("User not Approved");
        $("#modalInputCheck").prop("checked", false);
    }
}

function updateUserData() {
    // TODO: check modal validation
    const [creationDate, creationTime] = String($('#modalInputCreationDetails')).split(' ');
    var requestData = {
        Id: $("#userId").attr('data-userId'),
        FirstName: $("#modalInputFirstName").val(),
        LastName: $("#modalInputLastName").val(),
        Email: $("#modalInputEmail").val(),
        IsAdminApproved: $('#modalInputCheck').prop('checked'),
        userCreationDate: creationDate,
        userCreationTime: creationTime,
    };
    const apiURL = routeURL + '/api/Users/update';
    console.log("**LOG: API URL: ", apiURL);

    $.ajax({
        url: apiURL,
        type: 'POST',
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (response) {
            console.log("**LOG: API Call Success");
            if (response.status == 1) {
                disableModalEdit();
                alert("User details updated");
            }
        },
        error: function (xhr) {
            console.log("**LOG: ## API Call ERROR## = ", xhr);
        }
    });

}

function enableModalEdit() {
    $("#viewEditUsersModalTitle").text("Edit User");
    $("#modalInputFirstName").removeAttr("disabled");
    $("#modalInputLastName").removeAttr("disabled");
    $("#modalInputEmail").removeAttr("disabled");
    $("#modalInputCheck").removeAttr("disabled");
    $("#btnSaveChanges").removeClass("disabled");
    $("#btnSaveChanges").show(); // Show Save Changes button
}

function disableModalEdit() {
    $("#viewEditUsersModalTitle").text("View User"); // Change Modal Title
    $("#modalInputFirstName").prop("disabled", true);
    $("#modalInputLastName").prop("disabled", true);
    $("#modalInputEmail").prop("disabled", true);
    $("#modalInputCheck").prop("disabled", true);
    $("#btnSaveChanges").addClass("disabled");
    $("#btnSaveChanges").hide(); // Hide Save Changes button
    window.location.reload(); // Reload the page to get the latest info
}

function showDemoUserCredentials() {
    $('#demo_credentials').removeAttr('hidden');
}